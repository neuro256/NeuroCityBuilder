using System;

namespace Domain.Gameplay
{
    [Serializable]
    public class BuildingSaveData
    {
        public BuildingType Type;
        public GridPos Position;
        public int CurrentLevel;
    }
}

