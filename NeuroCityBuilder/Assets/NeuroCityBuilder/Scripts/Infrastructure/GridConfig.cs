using NeuroCityBuilder.Application.Interfaces;
using UnityEngine;

namespace NeuroCityBuilder.Infrastructure
{
    [CreateAssetMenu(fileName = "GridConfig", menuName = "Configs/GridConfig")]
    public class GridConfig : ScriptableObject, IGridConfig
    {
        [SerializeField] private int _width;
        [SerializeField] private int _height;

        public int Width => this._width;

        public int Height => this._height;
    }
}

