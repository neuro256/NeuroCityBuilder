using MessagePipe;
using NeuroCityBuilder.Application;
using NeuroCityBuilder.Application.Services;
using NeuroCityBuilder.Domain.Gameplay;
using NeuroCityBuilder.Domain.Messages;
using NeuroCityBuilder.Presentation.UI.Presenters;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using VContainer.Unity;

namespace NeuroCityBuilder.Presentation.System
{
    public class BuildingMoveSystem : IInitializable, IDisposable
    {
        private readonly ISubscriber<BuildingMoveRequestMessage> _buildingMoveRequestSubscriber;
        private readonly IBuildingService _buildingService;
        private readonly GridManager _gridManager;
        private readonly GridPresenter _gridPresenter;
        private readonly BuildingFactory _buildingFactory;
        private readonly IPublisher<BuildingMoveStartMessage> _buildingMoveStartPublisher;
        private readonly IPublisher<BuildingMoveCompleteMessage> _buildingMoveCompletePublisher;
        private readonly IPublisher<BuildingMoveCancelMessage> _buildingMoveCancelPublisher;

        private LayerMask _gridLayerMask = 1 << LayerMask.NameToLayer("Grid");
        private Camera _mainCamera;
        private Building _movingBuilding;
        private bool _isMoving;
        private IDisposable _subscription;

        public BuildingMoveSystem(
            ISubscriber<BuildingMoveRequestMessage> buildingMoveRequestSubscriber,
            IBuildingService buildingService,
            GridManager gridManager,
            GridPresenter gridPresenter,
            BuildingFactory buildingFactory,
            IPublisher<BuildingMoveStartMessage> buildingMoveStartPublisher,
            IPublisher<BuildingMoveCompleteMessage> buildingMoveCompletePublisher,
            IPublisher<BuildingMoveCancelMessage> buildingMoveCancelPublisher)
        {
            this._buildingMoveRequestSubscriber = buildingMoveRequestSubscriber;
            this._buildingService = buildingService;
            this._gridManager = gridManager;
            this._gridPresenter = gridPresenter;
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

            this._buildingService.RemoveBuilding(building);
            Vector3 startPosition = this._gridManager.GridToWorld(building.Position);
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
            if (EventSystem.current?.IsPointerOverGameObject() == true)
                return;

            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = this._mainCamera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, this._gridLayerMask))
            {
                GridPos gridPos = this._gridManager.WorldToGrid(hit.point);
                bool isValid = !this._gridManager.IsCellOccupied(gridPos);
                this._gridPresenter.ShowHighlight(gridPos, isValid);
                this._buildingFactory.UpdateGhostBuildingPosition(hit.point);

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
            this._gridPresenter.HideHighlight();


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
            this._gridPresenter.HideHighlight();

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
