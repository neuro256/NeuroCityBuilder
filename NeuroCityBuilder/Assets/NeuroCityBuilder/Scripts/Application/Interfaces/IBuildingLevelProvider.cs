using NeuroCityBuilder.Domain.Gameplay;

namespace NeuroCityBuilder.Application.Interfaces
{
    public interface IBuildingLevelProvider
    {
        BuildingLevel[] GetLevels(BuildingType type);
    }
}
