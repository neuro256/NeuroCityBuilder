using Domain.Messages;
using Domain.Gameplay;
using MessagePipe;
using Presentation.UI;
using System;
using UnityEngine;
using UseCases;
using UseCases.Services;
using VContainer.Unity;
using UnityEngine.InputSystem;

namespace Presentation.System
{
    public class BuildingInteractionSystem : IInitializable, IDisposable
    {
        private readonly GridManager _gridManager;
        private readonly GridView _gridView;
        private readonly IBuildingService _buildingService;
        private readonly ISubscriber<BuildingTypeSelectedMessage> _buildingTypeSelectedSubscriber;
        private readonly IPublisher<BuildingDeletedMessage> _buildingDeletedPublisher;
        private readonly Camera _camera;

        private BuildingType _selectedBuildingType;
        private bool _isPlacingBuilding;
        private IDisposable _subscription;
        private LayerMask _gridLayerMask = 1 << LayerMask.NameToLayer("Grid");

        public BuildingInteractionSystem(
        GridManager gridManager,
        GridView gridView,
        IBuildingService buildingService,
        ISubscriber<BuildingTypeSelectedMessage> buildingTypeSelectedSubscriber,
        IPublisher<BuildingDeletedMessage> buildingDeletedPublisher)
        {
            this._gridManager = gridManager;
            this._gridView = gridView;
            this._buildingService = buildingService;
            this._buildingTypeSelectedSubscriber = buildingTypeSelectedSubscriber;
            this._buildingDeletedPublisher = buildingDeletedPublisher;
            this._camera = Camera.main;
        }

        public void Initialize()
        {
            DisposableBagBuilder bag = DisposableBag.CreateBuilder();
            this._buildingTypeSelectedSubscriber.Subscribe(this.OnBuildingTypeSelected).AddTo(bag);
            this._subscription = bag.Build();
        }

        private void OnBuildingTypeSelected(BuildingTypeSelectedMessage message)
        {
            this._selectedBuildingType = message.BuildingType;
            this._isPlacingBuilding = true;
        }

        public void Update()
        {
            if(this._isPlacingBuilding) 
                this.HandleBuildingPlacement();
            else
                this.HandleBuildingSelection();
        }

        private void HandleBuildingPlacement()
        {
            if (!this._isPlacingBuilding) return;

            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = this._camera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, this._gridLayerMask))
            {
                GridPos gridPos = this._gridView.WorldToGrid(hit.point);
                bool isValid = !this._gridManager.IsCellOccupied(gridPos);
                this._gridView.ShowHighlight(gridPos, isValid);

                if (Mouse.current.leftButton.wasPressedThisFrame && isValid)
                {
                    this.PlaceBuildingAt(gridPos);
                }

                if (Mouse.current.rightButton.wasPressedThisFrame || Keyboard.current.escapeKey.wasPressedThisFrame)
                {
                    this.CancelPlacement();
                }
            }
        }

        private void PlaceBuildingAt(GridPos position)
        {
            Building building = this._buildingService.PlaceBuilding(this._selectedBuildingType, position, true);
            if (building != null)
            {
                this._gridView.HideHighlight();
            }

            this.CancelPlacement();
        }

        private void CancelPlacement()
        {
            this._isPlacingBuilding = false;
            this._gridView.HideHighlight();
            Debug.Log("Building placement canceled");
        }

        private void HandleBuildingSelection()
        {
            if (this._isPlacingBuilding) return;

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Vector2 mousePosition = Mouse.current.position.ReadValue();
                Ray ray = this._camera.ScreenPointToRay(mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, 100f, this._gridLayerMask))
                {
                    GridPos gridPos = this._gridView.WorldToGrid(hit.point);
                    Building building = this._buildingService.GetBuildingAt(gridPos);

                    if (building != null)
                    {
                        this._gridView.ShowHighlight(gridPos, true);
                        this._buildingService.SelectBuilding(building);
                    }
                    else
                    {
                        this._gridView.HideHighlight();
                        this._buildingService.DeselectBuilding();
                    }
                }
            }
        }

        public void Dispose()
        {
            this._subscription?.Dispose();
        }
    }
}

