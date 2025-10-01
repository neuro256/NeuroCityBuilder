using NeuroCityBuilder.Application.Interfaces;
using NeuroCityBuilder.Domain.Gameplay;
using NeuroCityBuilder.Presentation.UI.Views;
using UnityEngine;
using VContainer;

namespace NeuroCityBuilder.Presentation.UI.Presenters
{
    public class GridPresenter : IGridPresenter
    {
        private readonly IGridView _view;
        private readonly IGridConfig _gridConfig;

        private readonly IGridCell[,] _gridCells;
        private IGridCell _currentHighlightedCell;

        [Inject]
        public GridPresenter(IGridView view, IGridConfig gridConfig)
        {
            this._view = view;
            this._gridConfig = gridConfig;

            this._gridCells = new GridCell[this._gridConfig.Width, this._gridConfig.Height];
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

            IGridCell cell = this._gridCells[gridPos.X, gridPos.Y];
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
            for (int x = 0; x < this._gridConfig.Width; x++)
            {
                for (int y = 0; y < this._gridConfig.Height; y++)
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
            return pos.X >= 0 && pos.X < this._gridConfig.Width && pos.Y >= 0 && pos.Y < this._gridConfig.Height;
        }

        private void CreateGrid()
        {
            this.ClearAllCells();

            for (int x = 0; x < this._gridConfig.Width; x++)
            {
                for (int y = 0; y < this._gridConfig.Height; y++)
                {
                    Vector3 position = new Vector3(x, 0.01f, y);
                    IGridCell cell = this._view.CreateCell(x, y, position);
                    this._gridCells[x, y] = cell;
                }
            }
        }
    }
}

