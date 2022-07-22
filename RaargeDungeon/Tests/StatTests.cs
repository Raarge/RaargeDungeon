using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaargeDungeon.Creatures;

namespace RaargeDungeon.Tests
{
    public class StatTests
    {
        public static void DisplayProficiencyAndSaveScoresCalculated(Player p)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("******************************************************");
            Console.WriteLine("     Proficiency Modifier & Saving Scores            ");
            Console.WriteLine("******************************************************");
            Console.WriteLine($"  Proficiency Modifier:       {p.proficiencyBonus}");
            Console.WriteLine("*******************************************************");
            Console.WriteLine($"  Strength Save Modifier:     {p.strengthSave}");
            Console.WriteLine($"  Constitution Save Modifier: {p.constitutionSave}");
            Console.WriteLine($"  Dexterity Save Modifier:    {p.dexteritySave}");
            Console.WriteLine($"  Intelligence Save Modifier: {p.intelligenceSave}");
            Console.WriteLine($"  Wisdom Save Modifier:       {p.wisdomSave}");
            Console.WriteLine($"  Charisma Save Modifier:     {p.charismaSave}");
            Console.WriteLine("*******************************************************");
            Console.ResetColor();

            Console.ReadKey();
        }
    }
}
