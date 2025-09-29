using UnityEngine;

namespace NeuroCityBuilder.Domain.Gameplay
{
    [System.Serializable]
    public class BuildingLevel
    {
        [SerializeField]
        private int _level;
        [SerializeField]
        private int _cost;
        [SerializeField]
        private int _income;


        public int Level => this._level;
        public int Cost => this._cost;
        public int Income => this._income;
    }
}

