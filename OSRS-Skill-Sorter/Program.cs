using OsrsSkillSorter;

var scores = await FetchScoresAsync("Stupid Goose");

if (!scores.Any())
{
    return;
}

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
    var scorePairs = Constants.SkillNames
        .Select(skill => (Name: skill, Type: ScoreType.Skill))
        .Concat(Constants.ActivityNames
            .Select(activity => (Name: activity, Type: ScoreType.Activity)))
        .ToArray();
    var scores = new List<Score>();

    using var client = new HttpClient();

    string? response;
    try
    {
        response = await client.GetStringAsync($"https://secure.runescape.com/m=hiscore_oldschool/index_lite.ws?player={nick}");
    }
    catch (HttpRequestException ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"An error has occurred when trying to fetch the hiscores for nickname: '{nick}'");
        Console.WriteLine("It is possible that this player does not exist or that the Oldschool Runescape hiscores service is unavailable.");
        Console.WriteLine();
        Console.WriteLine("More detailed stacktrace can be found below.");
        Console.WriteLine();
        Console.Write(ex);
        Console.ResetColor();

        return scores;
    }
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