using Domain.Gameplay;
using Domain.Messages;
using MessagePipe;
using Presentation.UI;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UseCases;
using UseCases.Services;
using VContainer.Unity;

namespace Presentation.Systems
{
    public class BuildingMoveSystem : IInitializable, IDisposable
    {
        private readonly ISubscriber<BuildingMoveRequestMessage> _buildingMoveRequestSubscriber;
        private readonly IBuildingService _buildingService;
        private readonly GridManager _gridManager;
        private readonly GridView _gridView;
        private readonly BuildingFactory _buildingFactory;
        private readonly IPublisher<BuildingMoveStartMessage> _buildingMoveStartPublisher;
        private readonly IPublisher<BuildingMoveCompleteMessage> _buildingMoveCompletePublisher;
        private readonly IPublisher<BuildingMoveCancelMessage> _buildingMoveCancelPublisher;

        private Camera _mainCamera;
        private Building _movingBuilding;
        private bool _isMoving;
        private IDisposable _subscription;

        public BuildingMoveSystem(
            ISubscriber<BuildingMoveRequestMessage> buildingMoveRequestSubscriber,
            IBuildingService buildingService,
            GridManager gridManager,
            GridView gridView,
            BuildingFactory buildingFactory,
            IPublisher<BuildingMoveStartMessage> buildingMoveStartPublisher,
            IPublisher<BuildingMoveCompleteMessage> buildingMoveCompletePublisher,
            IPublisher<BuildingMoveCancelMessage> buildingMoveCancelPublisher)
        {
            this._buildingMoveRequestSubscriber = buildingMoveRequestSubscriber;
            this._buildingService = buildingService;
            this._gridManager = gridManager;
            this._gridView = gridView;
            this._buildingFactory = buildingFactory;
            this._buildingMoveStartPublisher = buildingMoveStartPublisher;
            this._buildingMoveCompletePublisher = buildingMoveCompletePublisher;
            this._buildingMoveCancelPublisher = buildingMoveCancelPublisher;

            this._mainCamera = Camera.main;
        }

        public void Initialize()
        {
            this._subscription = this._buildingMoveRequestSubscriber.Subscribe(this.HandleMoveRequest);
        }

        private void HandleMoveRequest(BuildingMoveRequestMessage message)
        {
            Debug.Log($"BuildingMoveSystem: Move request received for {message.Building.Type}");
            this.StartMovingBuilding(message.Building);
        }

        private void StartMovingBuilding(Building building)
        {
            if (building == null) return;

            this._movingBuilding = building;
            this._isMoving = true;

            this._buildingService.RemoveBuilding(building.Position);
            Vector3 startPosition = this._gridView.GridToWorld(building.Position);
            this._buildingFactory.CreateGhostBuilding(building.Type, startPosition);
            this._buildingMoveStartPublisher.Publish(new BuildingMoveStartMessage
            {
                Building = building
            });

            Debug.Log($"BuildingMoveSystem: Started moving {building.Type} from {building.Position.X},{building.Position.Y}");
        }

        public void Update()
        {
            if (!this._isMoving || this._movingBuilding == null) return;

            this.HandleMovement();
        }

        private void HandleMovement()
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = this._mainCamera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GridPos gridPos = this._gridView.WorldToGrid(hit.point);
                bool isValid = !this._gridManager.IsCellOccupied(gridPos);
                this._gridView.ShowHighlight(gridPos, isValid);
                Vector3 worldPosition = this._gridView.GridToWorld(gridPos);
                this._buildingFactory.UpdateGhostBuildingPosition(worldPosition);

                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    if (isValid)
                    {
                        this.CompleteMove(gridPos);
                    }
                }
                else if (Mouse.current.rightButton.wasPressedThisFrame || Keyboard.current.escapeKey.wasPressedThisFrame)
                {
                    this.CancelMove();
                }
            }
        }

        private void CompleteMove(GridPos newPosition)
        {
            Debug.Log($"BuildingMoveSystem: Completing move to {newPosition.X},{newPosition.Y}");

            Building newBuilding = this._buildingService.PlaceBuilding(this._movingBuilding.Type, newPosition);
            if (newBuilding != null)
            {
                newBuilding.CurrentLevel = this._movingBuilding.CurrentLevel;
                newBuilding.Levels = this._movingBuilding.Levels;
            }

            this._buildingFactory.DestroyGhostBuilding();
            this._gridView.HideHighlight();


            this._buildingMoveCompletePublisher.Publish(new BuildingMoveCompleteMessage
            {
                Building = newBuilding,
                NewPosition = newPosition
            });

            this._movingBuilding = null;
            this._isMoving = false;

            Debug.Log("BuildingMoveSystem: Move completed");
        }

        private void CancelMove()
        {
            Debug.Log("BuildingMoveSystem: Move cancelled");

            this._buildingService.PlaceBuilding(this._movingBuilding.Type, this._movingBuilding.Position);
            this._buildingFactory.DestroyGhostBuilding();
            this._gridView.HideHighlight();

            this._buildingMoveCancelPublisher.Publish(new BuildingMoveCancelMessage
            {
                Building = this._movingBuilding
            });

            this._movingBuilding = null;
            this._isMoving = false;
        }

        public void Dispose()
        {
            this._subscription?.Dispose();

            if (this._isMoving)
            {
                this.CancelMove();
            }
        }
    }
}
