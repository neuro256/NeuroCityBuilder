using UnityEngine;

namespace NeuroCityBuilder.Application.Interfaces
{
    public interface IGridCellFactory
    {
        IGridCell Create(Vector3 position, Transform parent, int x, int y);
    }
}

