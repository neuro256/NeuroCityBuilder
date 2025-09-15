using Domain.Gameplay;
using System.Collections.Generic;
using System.Linq;

namespace UseCases.Services
{
    public class BuildingService : IBuildingService
    {
        private readonly List<Building> _buildings = new();


        public Building MoveBuilding(GridPos startPos, GridPos endPos)
        {
            Building building = this.GetBuildingAt(endPos);
            
            if (building == null)
                return null;
            if (this.GetBuildingAt(startPos) != null)
                return null;

            building.Position = startPos;
            return building;
        }

        public Building PlaceBuilding(BuildingType type, GridPos position)
        {
            if (this.GetBuildingAt(position) == null)
            {
                Building building = this.CreateBuilding(type, position);
                this._buildings.Add(building);
                return building;
            }

            return null;
        }

        public void RemoveBuilding(GridPos position)
        {
            Building building = this.GetBuildingAt(position);
            if (building != null)
            {
                this._buildings.Remove(building);
            }
        }

        public Building UpgradeBuilding(GridPos position)
        {
            Building building = this.GetBuildingAt(position);
            if (building == null) 
                return null;

            if (building.CurrentLevel < building.Levels.Length - 1)
            {
                building.CurrentLevel++;
            }

            return building;
        }

        private Building CreateBuilding(BuildingType type, GridPos position)
        {
            return new Building
            {
                Type = type,
                Position = position,
                CurrentLevel = 0
            };
        }

        private Building GetBuildingAt(GridPos position) => this._buildings.FirstOrDefault(b => b.Position.Equals(position));
    }
}


