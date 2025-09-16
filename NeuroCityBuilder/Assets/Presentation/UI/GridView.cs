using Domain.Gameplay;
using UnityEngine;
using UseCases;

namespace Presentation.UI
{
    public class GridView : MonoBehaviour
    {
        [Header("Parent")]
        [SerializeField] private Transform _parent;
        [Header("Prefabs")]
        [SerializeField] private GridCell _gridCellPrefab;
        [Header("Grid Manager")]
        [SerializeField] private GridManager _gridManager;

        private GridCell[,] _gridCells;
        private GridCell _currentHighlightedCell;
        private int _width;
        private int _height;

        private void Start()
        {
            this.CreateGrid();
        }

        private void CreateGrid()
        {
            this._width = this._gridManager.GridWidth;
            this._height = this._gridManager.GridHeight;

            this._gridCells = new GridCell[this._width, this._height];

            for (int x = 0; x < this._width; x++)
            {
                for (int y = 0; y < this._height; y++)
                {
                    Vector3 position = new Vector3(x, 0.01f, y);// + this._parent.transform.position;
                    GridCell cell = Instantiate(this._gridCellPrefab, position, Quaternion.Euler(90, 0, 0), this._parent);
                    cell.name = $"GridCell_{x}_{y}";

                    GridCell gridCell = cell.GetComponent<GridCell>();
                    gridCell?.SetDefault();

                    this._gridCells[x, y] = cell;
                }
            }
        }

        public void ShowHighlight(GridPos gridPos, bool isValid)
        {
            if (!this.IsValidPosition(gridPos)) return;

            this.HideHighlight();

            GridCell cell = this._gridCells[gridPos.X, gridPos.Y];
            if (cell != null)
            {
                GridCell gridCell = cell.GetComponent<GridCell>();
                if (gridCell != null)
                {
                    if (isValid)
                        gridCell.SetHighlightValid();
                    else
                        gridCell.SetHighlightInvalid();
                }

                this._currentHighlightedCell = cell;
            }
        }

        public void HideHighlight()
        {
            if (this._currentHighlightedCell != null)
            {
                GridCell gridCell = this._currentHighlightedCell.GetComponent<GridCell>();
                gridCell?.SetDefault();
                this._currentHighlightedCell = null;
            }
        }

        private bool IsValidPosition(GridPos pos)
        {
            return pos.X >= 0 && pos.X < this._width && pos.Y >= 0 && pos.Y < this._height;
        }

        public Vector3 GridToWorld(GridPos gridPos)
        {
            return new Vector3(gridPos.X, 0, gridPos.Y);
        }

        public GridPos WorldToGrid(Vector3 worldPosition)
        {
            int x = Mathf.RoundToInt(worldPosition.x);
            int y = Mathf.RoundToInt(worldPosition.z);
            return new GridPos(x, y);
        }
    }
}

