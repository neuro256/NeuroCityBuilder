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

namespace Presentation
{
    public class BuildingPlacementSystem : IInitializable, IDisposable
    {
        private readonly GridManager _gridManager;
        private readonly GridView _gridView;
        private readonly IBuildingService _buildingService;
        private readonly ISubscriber<BuildingSelectedMessage> _buildingSelectedSubscriber;
        private readonly Camera _camera;

        private BuildingType _selectedBuildingType;
        private bool _isPlacingBuilding;
        private IDisposable _subscription;

        public BuildingPlacementSystem(
        GridManager gridManager,
        GridView gridView,
        IBuildingService buildingService,
        ISubscriber<BuildingSelectedMessage> buildingSelectedSubscriber)
        {
            this._gridManager = gridManager;
            this._gridView = gridView;
            this._buildingService = buildingService;
            this._buildingSelectedSubscriber = buildingSelectedSubscriber;
            this._camera = Camera.main;
        }

        public void Initialize()
        {
            DisposableBagBuilder bag = DisposableBag.CreateBuilder();
            this._buildingSelectedSubscriber.Subscribe(this.OnBuildingSelected).AddTo(bag);
            this._subscription = bag.Build();
        }

        private void OnBuildingSelected(BuildingSelectedMessage message)
        {
            this._selectedBuildingType = message.BuildingType;
            this._isPlacingBuilding = true;
            Debug.Log($"Ready to place: {this._selectedBuildingType}");
        }

        public void Update()
        {
            if (!this._isPlacingBuilding) return;

            this.HandleBuildingPlacement();
        }

        private void HandleBuildingPlacement()
        {
            if (!this._isPlacingBuilding) return;

            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Ray ray = this._camera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
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
            Debug.Log($"PlaceBuildingAt: {position}");
            Building building = this._buildingService.PlaceBuilding(this._selectedBuildingType, position);
            if (building != null)
            {
                Debug.Log($"Building placed at {position.X},{position.Y}");
                this._isPlacingBuilding = false;
                this._gridView.HideHighlight();
            }
        }

        private void CancelPlacement()
        {
            this._isPlacingBuilding = false;
            this._gridView.HideHighlight();
            Debug.Log("Building placement canceled");
        }

        public void Dispose()
        {
            this._subscription?.Dispose();
        }
    }
}

