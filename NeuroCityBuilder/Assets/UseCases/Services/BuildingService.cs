using Domain.Gameplay;
using Domain.Messages;
using MessagePipe;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace UseCases.Services
{
    public class BuildingService : IBuildingService
    {
        private readonly GridManager _gridManager;
        private readonly BuildingFactory _buildingFactory;
        private readonly IPublisher<BuildingPlacedMessage> _buildingPlacedPublisher;
        private readonly IPublisher<BuildingDeletedMessage> _buildingDeletedPublisher;
        private readonly IPublisher<BuildingSelectedMessage> _buildingSelectedPublisher;

        private readonly Dictionary<GridPos, Building> _buildings = new();
        private Building _selectedBuilding;

        public BuildingService(
        GridManager gridManager,
        IPublisher<BuildingPlacedMessage> buildingPlacedPublisher,
        IPublisher<BuildingDeletedMessage> buildingDeletedPublisher,
        IPublisher<BuildingSelectedMessage> buildingSelectedPublisher,
        BuildingFactory buildingFactory)
        {
            this._gridManager = gridManager;
            this._buildingPlacedPublisher = buildingPlacedPublisher;
            this._buildingDeletedPublisher = buildingDeletedPublisher;
            this._buildingSelectedPublisher = buildingSelectedPublisher;
            this._buildingFactory = buildingFactory;
        }

        public Building MoveBuilding(GridPos startPos, GridPos endPos)
        {
            if (!this._buildings.TryGetValue(startPos, out Building building))
                return null;

            if (this._gridManager.IsCellOccupied(endPos))
                return null;

            // Перемещаем здание
            building.Position = endPos;

            // Обновляем хранилище
            this._buildings.Remove(startPos);
            this._buildings[endPos] = building;

            // Обновляем сетку
            this._gridManager.SetCellOccupied(startPos, false);
            this._gridManager.SetCellOccupied(endPos, true);

            return building;
        }

        public Building PlaceBuilding(BuildingType type, GridPos position)
        {
            Debug.Log($"PlaceBuilding type: {type} pos: {position}");

            if (this._gridManager.IsCellOccupied(position))
            {
                Debug.LogWarning($"Cell {position.X},{position.Y} is already occupied!");
                return null;
            }

            Building building = this._buildingFactory.CreateBuilding(type, position);

            this._gridManager.SetCellOccupied(position, true);

            this._buildingPlacedPublisher.Publish(new BuildingPlacedMessage
            {
                Building = building,
                Position = position
            });

            this._buildings[position] = building;

            return building;
        }

        public void RemoveBuilding(GridPos position)
        {
            if (this._buildings.TryGetValue(position, out Building building))
            {
                // Сохраняем данные для сообщения перед удалением
                Building deletedBuilding = building;
                GridPos deletedPosition = position;

                // Освобождаем клетку
                this._gridManager.SetCellOccupied(position, false);

                // Удаляем визуальное представление
                this._buildingFactory.RemoveBuildingVisual(position);

                // Удаляем здание из хранилища
                this._buildings.Remove(position);

                // Публикуем сообщение об удалении
                this._buildingDeletedPublisher.Publish(new BuildingDeletedMessage
                {
                    Building = deletedBuilding,
                    Position = deletedPosition
                });

                Debug.Log($"Building removed from position {position.X},{position.Y}");

                // Если удаляем выбранное здание, сбрасываем выбор
                if (this._selectedBuilding != null && this._selectedBuilding.Position.Equals(position))
                {
                    this._selectedBuilding = null;
                }

                this._buildingDeletedPublisher.Publish(new BuildingDeletedMessage
                {
                    Building = building,
                    Position = position
                });
            }
        }

        public Building UpgradeBuilding(GridPos position)
        {
            if (this._buildings.TryGetValue(position, out Building building))
            {
                if (building.CurrentLevel < building.Levels.Length - 1)
                {
                    building.CurrentLevel++;
                    return building;
                }
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
            Debug.Log($"GetBuildingAt: pos={position.X}:{position.Y}");
            this._buildings.TryGetValue(position, out Building building);
            return building;
        }
    }
}


