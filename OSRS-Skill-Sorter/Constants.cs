using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsrsSkillSorter
{
    public static class Constants
    {
        public static IEnumerable<string> SkillNames = new[]
        {
            "Overall", "Attack", "Defence", "Strength", "Hitpoints", "Ranged", "Prayer",
            "Magic", "Cooking", "Woodcutting", "Fletching", "Fishing", "Firemaking",
            "Crafting", "Smithing", "Mining", "Herblore", "Agility", "Thieving",
            "Slayer", "Farming", "Runecrafting", "Hunter", "Construction"
        };

        public static IEnumerable<string> ActivityNames = new[]
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
    }
}
