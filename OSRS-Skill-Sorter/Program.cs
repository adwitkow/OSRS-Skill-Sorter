using OsrsSkillSorter;

var scores = await FetchScoresAsync("Stupid Goose");

var skillsTable = new ScoreTable("Sorted Skills", ScoreType.Skill, scores);
skillsTable.OrderBy = score => score.Experience!.Value;

skillsTable.AddColumn("Skill", score => score.Name);
skillsTable.AddColumn("Level", score => score.FriendlyLevel);
skillsTable.AddColumn("Experience", score => score.FriendlyExperience);

Console.WriteLine(skillsTable.GenerateTable());

Console.WriteLine();

var activityTable = new ScoreTable("Sorted Activities", ScoreType.Activity, scores);
activityTable.OrderBy = score => score.Level;

activityTable.AddColumn("Activity", score => score.Name);
activityTable.AddColumn("Kill Count", score => score.FriendlyLevel);

Console.WriteLine(activityTable.GenerateTable());

async Task<IEnumerable<Score>> FetchScoresAsync(string nick)
{
    using var client = new HttpClient();
    var response = await client.GetStringAsync($"https://secure.runescape.com/m=hiscore_oldschool/index_lite.ws?player={nick}");

    var scorePairs = Constants.SkillNames
        .Select(skill => (Name: skill, Type: ScoreType.Skill))
        .Concat(Constants.ActivityNames
            .Select(activity => (Name: activity, Type: ScoreType.Activity)))
        .ToArray();
    var scores = new List<Score>();

    using var reader = new StringReader(response);
    string line;
    int index = 0;
    while ((line = reader.ReadLine()!) != null)
    {
        var segments = line.Split(',')
            .Select(segment => Convert.ToInt32(segment))
            .ToArray();
        var scorePair = scorePairs[index];
        scores.Add(new Score(scorePair.Name, scorePair.Type, segments[0], segments[1], segments.Length > 2 ? segments[2] : null));
        index++;
    }

    return scores;
}