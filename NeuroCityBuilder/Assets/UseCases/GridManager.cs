using Domain.Gameplay;
using UnityEngine;

namespace UseCases
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private int _width = 32;
        [SerializeField] private int _height = 32;
        [SerializeField] private float _cellSize = 1f;

        private bool[,] _occupiedCells;

        private void Awake()
        {
            this._occupiedCells = new bool[this._width, this._height];
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
            int x = Mathf.FloorToInt(worldPosition.x / this._cellSize);
            int y = Mathf.FloorToInt(worldPosition.z / this._cellSize);
            return new GridPos(x, y);
        }

        public Vector3 GridToWorld(GridPos gridPos)
        {
            return new Vector3(gridPos.X * this._cellSize, 0, gridPos.Y * this._cellSize);
        }
    }
}

