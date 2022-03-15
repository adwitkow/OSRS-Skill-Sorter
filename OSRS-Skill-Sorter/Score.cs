namespace OsrsSkillSorter
{
    public record Score(string Name, ScoreType Type, int Rank, int Level, int? Experience)
    {
        public string FriendlyScore
        {
            get => this.Experience is null ? this.FriendlyLevel : this.FriendlyExperience;
        }

        public string FriendlyLevel
        {
            get => this.Level == -1 ? "Unknown" : this.Level.ToString();
        }

        public string FriendlyExperience
        {
            get => this.Experience is null || this.Experience == -1 ? "Unknown" : this.Experience.Value.ToString();
        }
    }
}
