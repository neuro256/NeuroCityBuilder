using NeuroCityBuilder.Application.Interfaces;
using UnityEngine;

namespace NeuroCityBuilder.Infrastructure
{
    public class GridCellFactory : IGridCellFactory
    {
        private readonly IGridPrefabProvider _prefabProvider;

        public GridCellFactory(IGridPrefabProvider prefabProvider) 
        {
            this._prefabProvider = prefabProvider;
        }

        public IGridCell Create(Vector3 position, Transform parent, int x, int y)
        {
            GameObject instance = Object.Instantiate(this._prefabProvider.GridCell, position, Quaternion.Euler(90, 0, 0), parent);
            instance.name = $"GridCell_{x}_{y}";

            return instance.GetComponent<IGridCell>();
        }
    }
}

