using System;
using UnityEngine;

namespace NeuroCityBuilder.Domain.Gameplay
{
    [Serializable]
    public struct GridPos
    {
        [SerializeField] public int X;
        [SerializeField] public int Y;

        public GridPos(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public override bool Equals(object obj) => obj is GridPos other && this.X == other.X && this.Y == other.Y;

        public override int GetHashCode() => HashCode.Combine(this.X, this.Y);
    }
}

