// See https://aka.ms/new-console-template for more information

var skillNames = new[]
{
    "Overall", "Attack", "Defence", "Strength", "Hitpoints", "Ranged", "Prayer",
    "Magic", "Cooking", "Woodcutting", "Fletching", "Fishing", "Firemaking",
    "Crafting", "Smithing", "Mining", "Herblore", "Agility", "Thieving",
    "Slayer", "Farming", "Runecrafting", "Hunter", "Construction"
};

var activityNames = new[]
{
    "League Points", "Bounty Hunter - Hunter", "Bounty Hunter - Rogue",
    "Clue Scrolls (all)", "Clue Scrolls (beginner)", "Clue Scrolls (easy)",
    "Clue Scrolls (medium)", "Clue Scrolls (hard)", "Clue Scrolls (elite)",
    "Clue Scrolls (master)", "LMS - Rank", "Soul Wars Zeal", "Abyssal Sire",
    "Alchemical Hydra", "Barrows Chests", "Bryophyta", "Callisto", "Cerberus",
    "Chambers of Xeric", "Chambers of Xeric: Challenge Mode", "Chaos Elemental",
    "Chaos Fanatic", "Commander Zilyana", "Corporeal Beast", "Crazy Archaeologist",
    "Dagannoth Prime", "Dagannoth Rex", "Dagannoth Supreme", "Deranged Archaeologist",
    "General Graardor", "Giant Mole", "Grotesque Guardians", "Hespori", "Kalphite Queen",
    "King Black Dragon", "Kraken", "Kree'Arra", "K'ril Tsutsaroth", "Mimic", "Nex",
    "Nightmare", "Phosani's Nightmare", "Obor", "Sarachnis", "Scorpia", "Skotizo",
    "Tempoross", "The Gauntlet", "The Corrupted Gauntlet", "Theatre of Blood",
    "Theatre of Blood: Hard Mode", "Thermonuclear Smoke Devil", "TzKal-Zuk", "TzTok-Jad",
    "Venenatis", "Vet'ion", "Vorkath", "Wintertodt", "Zalcano", "Zulrah"
};

using var client = new HttpClient();
var response = await client.GetStringAsync("https://secure.runescape.com/m=hiscore_oldschool/index_lite.ws?player=stupid goose");

var scorePairs = skillNames
    .Select(skill => (Name: skill, Type: ScoreType.Skill))
    .Concat(activityNames
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

var separatorWidth = 3;
var skillsTitle = " Sorted Skills ";
var activitiesTitle = " Sorted Activities ";

var skills = scores.Where(score => score.Type == ScoreType.Skill)
    .OrderByDescending(score => score.Experience)
    .ThenBy(score => score.Name);
var skillNameWidth = Math.Max("Skill".Length, skills.Max(skill => skill.Name.Length));
var levelWidth = Math.Max("Level".Length, skills.Max(skill => skill.FriendlyLevel.Length));
var expWidth = Math.Max("Experience".Length, skills.Max(skill => skill.FriendlyExperience.Length));
var overallSkillsWidth = skillNameWidth + levelWidth + expWidth + separatorWidth * 2;

Console.WriteLine(CenterString(skillsTitle, overallSkillsWidth, '-'));
Console.WriteLine($"{CenterString("Skill", skillNameWidth, ' ')} | {"Level".PadRight(levelWidth)} | {"Experience".PadRight(expWidth)}");
Console.WriteLine($"{new string('-', skillNameWidth)}-|-{new string('-', levelWidth)}-|-{new string('-', expWidth)}");
foreach (var skill in skills)
{
    var paddedName = skill.Name.PadRight(skillNameWidth);
    var paddedLevel = skill.FriendlyLevel.PadLeft(levelWidth);
    var paddedExp = skill.FriendlyExperience.PadLeft(expWidth);
    Console.WriteLine($"{paddedName} | {paddedLevel} | {paddedExp}");
}

Console.WriteLine();

var activities = scores.Where(score => score.Type == ScoreType.Activity)
    .OrderByDescending(score => score.Level)
    .ThenBy(score => score.Name);
var activityNameWidth = Math.Max("Activity".Length, activities.Max(activity => activity.Name.Length));
var killCountWidth = Math.Max("Kill Count".Length, activities.Max(activity => activity.FriendlyScore.Length));
var overallActivityWidth = activityNameWidth + killCountWidth + separatorWidth;

Console.WriteLine(CenterString(activitiesTitle, overallActivityWidth, '-'));
Console.WriteLine($"{CenterString("Name", activityNameWidth, ' ')} | {"Kill Count".PadRight(killCountWidth)}");
Console.WriteLine($"{new string('-', activityNameWidth)}-|-{new string('-', killCountWidth)}");
foreach (var activity in activities)
{
    var paddedName = activity.Name.PadRight(activityNameWidth);
    var paddedKillCount = activity.FriendlyScore.PadLeft(killCountWidth);
    Console.WriteLine($"{paddedName} | {paddedKillCount}");
}

static string CenterString(string stringToCenter, int totalLength, char paddingCharacter)
{
    return stringToCenter.PadLeft(
        ((totalLength - stringToCenter.Length) / 2) + stringToCenter.Length,
          paddingCharacter).PadRight(totalLength, paddingCharacter);
}

record Score(string Name, ScoreType Type, int Rank, int Level, int? Experience)
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

enum ScoreType
{
    Skill,
    Activity,
}