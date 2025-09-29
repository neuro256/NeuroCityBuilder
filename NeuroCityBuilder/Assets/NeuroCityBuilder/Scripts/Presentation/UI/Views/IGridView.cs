using UnityEngine;

namespace NeuroCityBuilder.Presentation.UI.Views
{
    public interface IGridView
    {
        GridCell CreateCell(int x, int y, Vector3 position);
        void RemoveCell(GridCell cell);
        void ClearAllCells();
    }
}

