using System.Text;

namespace OsrsSkillSorter
{
    public class ScoreTable
    {
        private const string LeftSeparator = "| ";
        private const string RightSeparator = " |";
        private const string MiddleSeparator = " | ";

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

            var middleSeparatorWidth = MiddleSeparator.Length;
            var leftSeparatorWidth = LeftSeparator.Length;
            var rightSeparatorWidth = RightSeparator.Length;

            var overallWidth = CalculateTableWidth(middleSeparatorWidth, leftSeparatorWidth, rightSeparatorWidth);

            var builder = new StringBuilder();

            var tableTitleLine = CenterString(this.Title, overallWidth, '-');
            builder.AppendLine(tableTitleLine);

            var formattedHeaders = FormatColumnHeaders();
            var headerLine = CreateValuesLine(formattedHeaders);
            builder.AppendLine(headerLine);
            builder.AppendLine(new string('-', overallWidth));

            foreach (var score in orderedScores)
            {
                var formattedValues = FormatScoreValues(score);
                var valuesLine = CreateValuesLine(formattedValues);
                builder.AppendLine(valuesLine);
            }

            builder.AppendLine(new string('-', overallWidth));

            return builder.ToString();
        }

        private IEnumerable<string> FormatScoreValues(Score score)
        {
            return this.Columns.Select(column => column.Property(score).PadRight(column.Width));
        }

        private IEnumerable<string> FormatColumnHeaders()
        {
            return this.Columns.Select(column => CenterString(column.Name, column.Width, ' '));
        }

        private int CalculateTableWidth(int middleSeparatorWidth, int leftSeparatorWidth, int rightSeparatorWidth)
        {
            return this.Columns.Sum(column => column.Width) + (middleSeparatorWidth * (this.Columns.Count - 1)) + leftSeparatorWidth + rightSeparatorWidth;
        }

        private static string CreateValuesLine(IEnumerable<string> formattedValues)
        {
            return LeftSeparator + string.Join(MiddleSeparator, formattedValues) + RightSeparator;
        }

        private static string CenterString(string stringToCenter, int totalLength, char paddingCharacter)
        {
            return stringToCenter.PadLeft(
                ((totalLength - stringToCenter.Length) / 2) + stringToCenter.Length,
                  paddingCharacter).PadRight(totalLength, paddingCharacter);
        }
    }
}
