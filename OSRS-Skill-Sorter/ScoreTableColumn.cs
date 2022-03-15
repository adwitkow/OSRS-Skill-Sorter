namespace OsrsSkillSorter
{
    public class ScoreTableColumn
    {
        public ScoreTableColumn(string name, Func<Score, string> property)
        {
            this.Name = name;
            this.Property = property;
        }

        public Func<Score, string> Property { get; }

        public string Name { get; set; }

        public int Width { get; set; }
    }
}
