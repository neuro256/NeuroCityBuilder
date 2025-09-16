using Domain.Gameplay;
using UnityEngine;

namespace Presentation.UI
{
    public class GridView : MonoBehaviour
    {
        [Header("Grid Settings")]
        [SerializeField] private int _width = 32;
        [SerializeField] private int _height = 32;

        [Header("Prefabs")]
        [SerializeField] private GridCell _gridCellPrefab;

        private GridCell[,] _gridCells;
        private GridCell _currentHighlightedCell;
        private float _offsetX;

        private void Start()
        {
            this.CreateGrid();
        }

        private void CreateGrid()
        {
            this._gridCells = new GridCell[this._width, this._height];

            this._offsetX = -(this._width) / 2f;

            for (int x = 0; x < this._width; x++)
            {
                for (int y = 0; y < this._height; y++)
                {
                    Vector3 position = new Vector3(x + this._offsetX, 0.01f, y);
                    GridCell cell = Instantiate(this._gridCellPrefab, position, Quaternion.Euler(90, 0, 0), this.transform);
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
            return new Vector3(gridPos.X + this._offsetX, 0, gridPos.Y);
        }

        public GridPos WorldToGrid(Vector3 worldPosition)
        {
            int x = Mathf.FloorToInt(worldPosition.x - this._offsetX);
            int y = Mathf.FloorToInt(worldPosition.z);
            return new GridPos(x, y);
        }
    }
}

