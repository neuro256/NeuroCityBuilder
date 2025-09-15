namespace Domain.Gameplay
{
    public class Building
    {
        public BuildingType Type { get; set; }
        public GridPos Position { get; set; }
        public int CurrentLevel { get; set; }
        public BuildingLevel[] Levels { get; set; }

        public BuildingLevel GetCurrentLevelInfo() => this.Levels[this.CurrentLevel];
    }
}
