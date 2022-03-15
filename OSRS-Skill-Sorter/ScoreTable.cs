using System.Text;

namespace OsrsSkillSorter
{
    public class ScoreTable
    {
        private const int SeparatorWidth = 3;

        public IEnumerable<Score> Scores { get; }

        public ScoreType ScoreType { get; }

        public Func<Score, int> OrderBy { get; set; }

        public string Title { get; }

        private List<ScoreTableColumn> Columns;

        public ScoreTable(string title, ScoreType scoreType, IEnumerable<Score> scores)
        {
            this.ScoreType = scoreType;
            this.Scores = scores;
            this.Title = title;

            this.Columns = new List<ScoreTableColumn>();
            this.OrderBy = score => score.Level;
        }

        public void AddColumn(string columnName, Func<Score, string> property)
        {
            var values = this.Scores
                .Where(score => score.Type == this.ScoreType)
                .Select(property);
            var column = new ScoreTableColumn(columnName, property);
            column.Width = Math.Max(columnName.Length, values.Max(val => val.Length));

            this.Columns.Add(column);
        }

        public string GenerateTable()
        {
            var orderedScores = this.Scores
                .Where(score => score.Type == this.ScoreType)
                .OrderByDescending(this.OrderBy)
                .ThenBy(score => score.Name)
                .ToList();
            var overallWidth = this.Columns.Sum(column => column.Width) + (SeparatorWidth * (this.Columns.Count - 1));

            var builder = new StringBuilder();

            var tableTitleLine = CenterString(this.Title, overallWidth, '-');
            builder.AppendLine(tableTitleLine);

            var formattedHeaders = this.Columns.Select(column => CenterString(column.Name, column.Width, ' '));
            var headerLine = string.Join(" | ", formattedHeaders);
            builder.AppendLine(headerLine);
            builder.AppendLine(new string('-', overallWidth));

            foreach (var score in orderedScores)
            {
                var formattedValues = this.Columns.Select(column => column.Property(score).PadRight(column.Width));
                var valuesLine = string.Join(" | ", formattedValues);
                builder.AppendLine(valuesLine);
            }

            return builder.ToString();
        }

        private static string CenterString(string stringToCenter, int totalLength, char paddingCharacter)
        {
            return stringToCenter.PadLeft(
                ((totalLength - stringToCenter.Length) / 2) + stringToCenter.Length,
                  paddingCharacter).PadRight(totalLength, paddingCharacter);
        }
    }
}
