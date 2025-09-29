using NeuroCityBuilder.Domain.Gameplay;
using UnityEngine;

namespace NeuroCityBuilder.Application.Interfaces
{
    public interface IBuildingPrefabProvider
    {
        Transform Parent { get; }
        GameObject GetBuildingPrefab(BuildingType type);
        GameObject GetGhostPrefab(BuildingType type);
    }
}
