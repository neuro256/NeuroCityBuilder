using Domain.Gameplay;
using System;
using UnityEngine;

namespace UseCases
{
    public class GridManager
    {
        private bool[,] _occupiedCells;
        private int _width;
        private int _height;

        public GridManager(int width, int height)
        {
            this._width = width;
            this._height = height;
            this._occupiedCells = new bool[width, height];
        }

        public bool IsCellOccupied(GridPos pos)
        {
            if (pos.X < 0 || pos.X >= this._width || pos.Y < 0 || pos.Y >= this._height)
                return true;

            return this._occupiedCells[pos.X, pos.Y];
        }

        public void SetCellOccupied(GridPos pos, bool occupied)
        {
            if (pos.X >= 0 && pos.X < this._width && pos.Y >= 0 && pos.Y < this._height)
            {
                this._occupiedCells[pos.X, pos.Y] = occupied;
            }
        }

        public GridPos WorldToGrid(Vector3 worldPosition)
        {
            int x = Mathf.RoundToInt(worldPosition.x);
            int y = Mathf.RoundToInt(worldPosition.z);
            return new GridPos(x, y);
        }

        public Vector3 GridToWorld(GridPos gridPos)
        {
            return new Vector3(gridPos.X, 0, gridPos.Y);
        }

        public void ClearCells()
        {
            if (this._occupiedCells != null)
            {
                this._occupiedCells = new bool[this._width, this._height];
            }
        }
    }
}

