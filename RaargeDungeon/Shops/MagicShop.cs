using RaargeDungeon.Creatures;
using RaargeDungeon.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaargeDungeon.Shops
{
    public class MagicShop
    {
        static Random rando = new Random();

        public static void RunMagicShop(Player p)
        {
            Console.Clear();
            Console.WriteLine("     Ye Olde Magic Shop  ");
            Console.WriteLine("============================");
            Console.WriteLine(" (E)xit Shop");
            Console.WriteLine("============================");
            Console.WriteLine(" ");
            Console.WriteLine($" Name: {p.name} Class: {p.currentClass}");
            Console.WriteLine($" Level: {p.level}");

            //-- Health Bar --
            UIHelpers.GenerateStatusBar("Health", "<", "-", ConsoleColor.Red, p.health, p.baseHealth);

            // -- Experiance Bar --
            UIHelpers.GenerateStatusBar("XP", ">", "-", ConsoleColor.Yellow, p.xp, p.GetLevelUpValue(p.level));

            // -- Mana Bar --
            UIHelpers.GenerateStatusBar(p.manaType.ToString(), "*", " ", ConsoleColor.Blue, p.energy, p.baseEnergy);

            Console.WriteLine(" ");

            string input = Console.ReadLine().ToLower();

            while (input != "e" && input.Length != 1)
            {
                Console.WriteLine("You did not enter a valid value. Please reenter.");
                input = Console.ReadLine().ToLower();
            }

            if (input == "e")
                return;
                
               
        }
    }
}
