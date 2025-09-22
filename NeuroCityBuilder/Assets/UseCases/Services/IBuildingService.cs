using Domain.Gameplay;
using System.Collections.Generic;

namespace UseCases.Services
{
    public interface IBuildingService
    {
        Building PlaceBuilding(BuildingType type, GridPos position, bool isNewBuilding = false);
        void RemoveBuilding(Building building);
        Building UpgradeBuilding(Building building);
        Building GetBuildingAt(GridPos position);
        void SelectBuilding(Building building);
        void DeselectBuilding();
        List<Building> GetAllBuildings();
        void ClearAllBuildings();
    }
}
