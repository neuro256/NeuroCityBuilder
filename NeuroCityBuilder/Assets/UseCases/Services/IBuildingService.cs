using Domain.Gameplay;
using System.Collections.Generic;

namespace UseCases.Services
{
    public interface IBuildingService
    {
        Building PlaceBuilding(BuildingType type, GridPos position);
        void RemoveBuilding(GridPos position);
        Building MoveBuilding(GridPos startPos, GridPos endPos);
        Building UpgradeBuilding(GridPos position);
        Building GetBuildingAt(GridPos position);
        void SelectBuilding(Building building);
        void DeselectBuilding();
        List<Building> GetAllBuildings();
    }
}
