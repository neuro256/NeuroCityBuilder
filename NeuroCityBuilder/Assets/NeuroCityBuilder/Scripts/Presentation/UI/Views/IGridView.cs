using NeuroCityBuilder.Application.Interfaces;
using UnityEngine;

namespace NeuroCityBuilder.Presentation.UI.Views
{
    public interface IGridView
    {
        IGridCell CreateCell(int x, int y, Vector3 position);
        void RemoveCell(IGridCell cell);
        void ClearAllCells();
    }
}

