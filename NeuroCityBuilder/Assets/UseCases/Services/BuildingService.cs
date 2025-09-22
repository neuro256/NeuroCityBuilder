using Domain.Gameplay;
using Domain.Messages;
using MessagePipe;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UseCases.Services
{
    public class BuildingService : IBuildingService
    {
        private readonly GridManager _gridManager;
        private readonly BuildingFactory _buildingFactory;
        private readonly IPublisher<BuildingPlacedMessage> _buildingPlacedPublisher;
        private readonly IPublisher<BuildingDeletedMessage> _buildingDeletedPublisher;
        private readonly IPublisher<BuildingSelectedMessage> _buildingSelectedPublisher;
        private readonly IPublisher<BuildingDeselectedMessage> _buildingDeselectedPublisher;
        private readonly ISubscriber<BuildingDeleteRequestMessage> _buildingDeleteRequestSubscriber;
        private readonly IPublisher<BuildingUpgradedMessage> _buildingUpgradedPublisher;
        private readonly ISubscriber<BuildingUpgradeRequestMessage> _buildingUpgradeRequestSubscriber;
        private readonly ISubscriber<BuildingMoveRequestMessage> _buildingMoveRequestSubscriber;

        private readonly IResourceService _resourceService;
        private readonly List<Building> _buildings = new();
        private Building _selectedBuilding;
        private IDisposable _deleteSubscription;
        private IDisposable _upgradeSubscription;
        private IDisposable _moveSubscription;

        public BuildingService(
        GridManager gridManager,
        IPublisher<BuildingPlacedMessage> buildingPlacedPublisher,
        IPublisher<BuildingDeletedMessage> buildingDeletedPublisher,
        IPublisher<BuildingSelectedMessage> buildingSelectedPublisher,
        IPublisher<BuildingDeselectedMessage> buildingDeselectPublisher,
        ISubscriber<BuildingDeleteRequestMessage> buildingDeleteRequestSubscriber,
        IPublisher<BuildingUpgradedMessage> buildingUpgradedPublisher,
        ISubscriber<BuildingUpgradeRequestMessage> buildingUpgradeRequestSubscriber,
        ISubscriber<BuildingMoveRequestMessage> buildingMoveRequestSubscriber,
        IResourceService resourceService,
        BuildingFactory buildingFactory)
        {
            this._gridManager = gridManager;
            this._buildingPlacedPublisher = buildingPlacedPublisher;
            this._buildingDeletedPublisher = buildingDeletedPublisher;
            this._buildingSelectedPublisher = buildingSelectedPublisher;
            this._buildingDeselectedPublisher = buildingDeselectPublisher;
            this._buildingDeleteRequestSubscriber = buildingDeleteRequestSubscriber;
            this._buildingUpgradedPublisher = buildingUpgradedPublisher;
            this._buildingUpgradeRequestSubscriber = buildingUpgradeRequestSubscriber;
            this._buildingMoveRequestSubscriber = buildingMoveRequestSubscriber;
            this._buildingFactory = buildingFactory;
            this._resourceService = resourceService;

            this._deleteSubscription = this._buildingDeleteRequestSubscriber.Subscribe(this.HandleDeleteRequest);
            this._upgradeSubscription = this._buildingUpgradeRequestSubscriber.Subscribe(this.HandleUpgradeRequest);
            this._moveSubscription = this._buildingMoveRequestSubscriber.Subscribe(this.HandleMoveRequest);
        }

        public Building PlaceBuilding(BuildingType type, GridPos position, bool isNewBuilding = false)
        {
            Debug.Log($"PlaceBuilding type: {type} pos: {position}");

            if (this._gridManager.IsCellOccupied(position))
            {
                Debug.LogWarning($"Cell {position.X},{position.Y} is already occupied!");
                return null;
            }

            int buildingCost = this.GetBuildingCost(this._buildingFactory.GetBuildingLevels(type), 0);

            if (!this._resourceService.CanAfford(buildingCost) && isNewBuilding)
            {
                return null;
            }

            Building building = this._buildingFactory.CreateBuilding(type, position);
            this._gridManager.SetCellOccupied(position, true);

            if (isNewBuilding) this._resourceService.SpendGold(buildingCost);

            this._buildingPlacedPublisher.Publish(new BuildingPlacedMessage
            {
                Building = building,
                Position = position
            });

            if(!this._buildings.Contains(building))
                this._buildings.Add(building);

            return building;
        }

        private int GetBuildingCost(BuildingLevel[] levels, int level)
        {
            if (levels == null || level > levels.Length) return 0;

            return levels[level].Cost;
        }

        public void RemoveBuilding(Building building)
        {
            if (!this._buildings.Contains(building))
                return;

            Building deletedBuilding = building;
            GridPos deletedPosition = building.Position;

            this._gridManager.SetCellOccupied(building.Position, false);
            this._buildingFactory.RemoveBuildingVisual(deletedPosition);

            Debug.Log($"Building removed from position {deletedPosition.X},{deletedPosition.Y}");

            this._buildings.Remove(building);

            

            if (this._selectedBuilding != null && this._selectedBuilding.Position.Equals(deletedPosition))
            {
                this._selectedBuilding = null;
            }

            this._buildingDeletedPublisher.Publish(new BuildingDeletedMessage
            {
                Building = deletedBuilding,
                Position = deletedPosition
            });
        }

        public Building UpgradeBuilding(Building building)
        {
            if (!this._buildings.Contains(building))
                return null;

            if (building.CurrentLevel < building.Levels.Length - 1)
            {
                int nextLevelIndex = building.CurrentLevel + 1;
                if (nextLevelIndex < building.Levels.Length)
                {
                    int upgradeCost = building.Levels[nextLevelIndex].Cost;

                    if (this._resourceService.CanAfford(upgradeCost))
                    {
                        this._resourceService.SpendGold(upgradeCost);
                        building.CurrentLevel++;

                        this._buildingUpgradedPublisher.Publish(new BuildingUpgradedMessage
                        {
                            Building = building
                        });

                        return building;
                    }
                }
            }
            else
            {
                Debug.Log("Building is already at max level");
            }

            return null;
        }

        public void SelectBuilding(Building building)
        {
            Debug.Log($"SelectedBuilding type: {building.Type} pos={building.Position.X}:{building.Position.Y}");
            this._selectedBuilding = building;

            this._buildingSelectedPublisher.Publish(new BuildingSelectedMessage
            {
                Building = building
            });
        }

        public Building GetBuildingAt(GridPos position)
        {
            Building findedBuilding = this._buildings.Find(b => b.Position.Equals(position));

            Debug.Log($"GetBuildingAt: pos={position.X}:{position.Y} result={findedBuilding != null} ");
            return findedBuilding;
        }

        public void DeselectBuilding()
        {
            this._selectedBuilding = null;

            this._buildingDeselectedPublisher.Publish(new BuildingDeselectedMessage());
        }

        private void HandleDeleteRequest(BuildingDeleteRequestMessage message)
        {
            Debug.Log($"HandleDeleteRequest: type={message.Building.Type} pos={message.Building.Position.X}:{message.Building.Position.Y}");
            this.RemoveBuilding(message.Building);
        }

        private void HandleUpgradeRequest(BuildingUpgradeRequestMessage message)
        {
            Debug.Log($"BuildingService: Upgrade request received for building {message.Building.Type} at {message.Building.Position.X},{message.Building.Position.Y}");
            this.UpgradeBuilding(message.Building);
        }

        private void HandleMoveRequest(BuildingMoveRequestMessage message)
        {
            Debug.Log($"BuildingService: Move request received for {message.Building.Type}");
            // Перемещение обрабатывается в BuildingMoveSystem
        }

        public void Dispose()
        {
            this._deleteSubscription?.Dispose();
            this._upgradeSubscription?.Dispose();
            this._moveSubscription?.Dispose();
            this._buildings.Clear();
        }

        public List<Building> GetAllBuildings()
        {
            return this._buildings;
        }

        public void ClearAllBuildings()
        {
            foreach (Building building in this._buildings)
            {
                this.RemoveBuilding(building);
            }

            this._buildings.Clear();
        }
    }
}


