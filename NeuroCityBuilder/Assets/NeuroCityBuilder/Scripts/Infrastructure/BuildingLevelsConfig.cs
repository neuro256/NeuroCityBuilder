using NeuroCityBuilder.Application.Interfaces;
using NeuroCityBuilder.Domain.Gameplay;
using UnityEngine;

namespace NeuroCityBuilder.Infrastructure
{
    [CreateAssetMenu(fileName = "BuildingLevelConfig", menuName = "Configs/BuildingLevels")]
    public class BuildingLevelsConfig : ScriptableObject, IBuildingLevelProvider
    {
        [System.Serializable]
        public class BuildingLevelSet
        {
            public BuildingType Type;
            public BuildingLevel[] Levels;
        }

        [SerializeField] private BuildingLevelSet[] _levelSets;

        public BuildingLevel[] GetLevels(BuildingType type)
        {
            foreach (BuildingLevelSet set in this._levelSets)
                if (set.Type == type)
                    return set.Levels;

            return new BuildingLevel[0];
        }
    }
}

