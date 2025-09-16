using Domain.Messages;
using Domain.Gameplay;
using MessagePipe;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UseCases.Services
{
    public class BuildingService : IBuildingService
    {
        private readonly GridManager _gridManager;
        private readonly BuildingFactory _buildingFactory;
        private readonly IPublisher<BuildingPlacedMessage> _buildingPlacedPublisher;

        public BuildingService(
        GridManager gridManager,
        IPublisher<BuildingPlacedMessage> buildingPlacedPublisher,
        BuildingFactory buildingFactory)
        {
            this._gridManager = gridManager;
            this._buildingPlacedPublisher = buildingPlacedPublisher;
            this._buildingFactory = buildingFactory;
        }

        public Building MoveBuilding(GridPos startPos, GridPos endPos)
        {
            if (this._gridManager.IsCellOccupied(endPos))
                return null;

            this._gridManager.SetCellOccupied(startPos, false);
            this._gridManager.SetCellOccupied(endPos, true);

            // В реальной реализации здесь бы обновлялась позиция здания
            return null;
        }

        public Building PlaceBuilding(BuildingType type, GridPos position)
        {
            Debug.Log($"PlaceBuilding type: {type} pos: {position}");

            if (this._gridManager.IsCellOccupied(position))
            {
                Debug.LogWarning($"Cell {position.X},{position.Y} is already occupied!");
                return null;
            }

            // Создаем здание через фабрику
            Building building = this._buildingFactory.CreateBuilding(type, position);

            // Занимаем клетку
            this._gridManager.SetCellOccupied(position, true);

            // Публикуем сообщение о размещении
            this._buildingPlacedPublisher.Publish(new BuildingPlacedMessage
            {
                Building = building,
                Position = position
            });

            return building;
        }

        public void RemoveBuilding(GridPos position)
        {
            this._gridManager.SetCellOccupied(position, false);
        }

        public Building UpgradeBuilding(GridPos position)
        {
            return null;
        }
    }
}


