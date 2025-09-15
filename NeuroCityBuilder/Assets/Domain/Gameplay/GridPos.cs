namespace Domain.Gameplay
{
    public class GridPos
    {
        public int X { get; set; }
        public int Y { get; set; }

        public GridPos(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public override bool Equals(object obj)
        {
            return obj is GridPos pos && this.X == pos.X && this.Y == pos.Y;
        }

        public override int GetHashCode()
        {
            return this.X.GetHashCode() ^ this.Y.GetHashCode();
        }
    }
}

