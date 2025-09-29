using NeuroCityBuilder.Domain.Gameplay;
using NeuroCityBuilder.Presentation.UI.Views;
using UnityEngine;
using VContainer;

namespace NeuroCityBuilder.Presentation.UI.Presenters
{
    public class GridPresenter : IGridPresenter
    {
        private readonly IGridView _view;

        private readonly int _width;
        private readonly int _height;

        private readonly GridCell[,] _gridCells;
        private GridCell _currentHighlightedCell;

        [Inject]
        public GridPresenter(IGridView view, int width, int height)
        {
            this._view = view;

            this._width = width;
            this._height = height;

            this._gridCells = new GridCell[this._width, this._height];
        }

        public void Initialize()
        {
            this.CreateGrid();
        }

        public void Dispose()
        {
            this.ClearAllCells();
        }

        public void ShowHighlight(GridPos gridPos, bool isValid)
        {
            if (!this.IsValidPosition(gridPos)) return;

            this.HideHighlight();

            GridCell cell = this._gridCells[gridPos.X, gridPos.Y];
            if (cell != null)
            {
                if (isValid)
                    cell.SetHighlightValid();
                else
                    cell.SetHighlightInvalid();
                this._currentHighlightedCell = cell;
            }
        }

        public void HideHighlight()
        {
            this._currentHighlightedCell?.SetDefault();
            this._currentHighlightedCell = null;
        }

        private void ClearAllCells()
        {
            for (int x = 0; x < this._width; x++)
            {
                for (int y = 0; y < this._height; y++)
                {
                    if (this._gridCells[x, y] != null)
                    {
                        this._view.RemoveCell(this._gridCells[x, y]);
                        this._gridCells[x, y] = null;
                    }
                }
            }

            this._view.ClearAllCells();
            this._currentHighlightedCell = null;
        }

        private bool IsValidPosition(GridPos pos)
        {
            return pos.X >= 0 && pos.X < this._width && pos.Y >= 0 && pos.Y < this._height;
        }

        private void CreateGrid()
        {
            this.ClearAllCells();

            for (int x = 0; x < this._width; x++)
            {
                for (int y = 0; y < this._height; y++)
                {
                    Vector3 position = new Vector3(x, 0.01f, y);
                    GridCell cell = this._view.CreateCell(x, y, position);
                    this._gridCells[x, y] = cell;
                }
            }
        }
    }
}

