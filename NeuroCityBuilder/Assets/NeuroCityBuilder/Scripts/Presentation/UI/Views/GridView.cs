using NeuroCityBuilder.Application.Interfaces;
using UnityEngine;
using VContainer;

namespace NeuroCityBuilder.Presentation.UI.Views
{
    public class GridView : MonoBehaviour, IGridView
    {
        [Header("Parent")]
        [SerializeField] private Transform _parent;

        private IGridCellFactory _cellFactory;
        private IObjectDestroyer _destroyer;

        [Inject]
        public void Construct(IGridCellFactory cellFactory, IObjectDestroyer destroyer)
        {
            this._cellFactory = cellFactory;
            this._destroyer = destroyer;
        }

        public IGridCell CreateCell(int x, int y, Vector3 position)
        {
            return this._cellFactory.Create(position, this._parent, x, y);
        }

        public void RemoveCell(IGridCell cell)
        {
            GridCell gridCell = cell as GridCell;
            
            if (gridCell != null && gridCell.gameObject != null)
            {
                this._destroyer.Destroy(gridCell.gameObject);
            }
        }

        public void ClearAllCells()
        {
            if (this._parent == null) return;

            foreach (Transform child in this._parent)
            {
                if (child != null && child.gameObject != null)
                {
                    this._destroyer.Destroy(child.gameObject);
                }
            }
        }
    }
}

