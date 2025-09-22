using Domain.Gameplay;
using UnityEngine;
using UseCases;

namespace Presentation.UI.Views
{
    public class GridView : MonoBehaviour, IGridView
    {
        [Header("Parent")]
        [SerializeField] private Transform _parent;
        [Header("Prefab")]
        [SerializeField] private GridCell _gridCellPrefab;

        public GridCell CreateCell(int x, int y, Vector3 position)
        {
            GridCell cell = Instantiate(this._gridCellPrefab, position, Quaternion.Euler(90, 0, 0), this._parent);
            cell.name = $"GridCell_{x}_{y}";
            return cell;
        }

        public void RemoveCell(GridCell cell)
        {
            if (cell != null && cell.gameObject != null)
            {
                Destroy(cell.gameObject);
            }
        }

        public void ClearAllCells()
        {
            if (this._parent == null) return;

            foreach (Transform child in this._parent)
            {
                if (child != null && child.gameObject != null)
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }
}

