using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaargeDungeon
{
    public class Shop
    {
        public static void loadShop(Player p)
        {
            RunShop(p);
        }

        public static void RunShop(Player p)
        {
            int potionP;
            int armorP;
            int weaponP;
            int diffP;
            int healthP;

            while (true)
            {
                potionP = 20 + (10 * p.mods);
                armorP = 100 * (p.armorValue + 1);
                weaponP = 100 * p.weaponValue;
                diffP = 300 * (100 * p.mods);
                healthP = 40 * p.baseHealth;

                Console.Clear();
                Console.WriteLine("        SHOP        ");
                Console.WriteLine("=====================");
                Console.WriteLine($" (W)eapon:     ${weaponP}    ");
                Console.WriteLine($" (A)rmor:      ${armorP}    ");
                Console.WriteLine($" (D)ifficulty: ${diffP}    ");
                Console.WriteLine($" (P)otions:    ${potionP}    ");
                Console.WriteLine($" (H)ealth:     ${healthP}    ");
                Console.WriteLine($" (E)xit Shop:    ");
                Console.WriteLine($" (Q)uit Game:    ");
                Console.WriteLine("=====================");

                Console.WriteLine(" ");
                Console.WriteLine($"    {p.name} Stats     ");
                Console.WriteLine("=====================");
                Console.WriteLine($" Current Health: {p.health}");
                Console.WriteLine($" Base Health: {p.baseHealth}");
                Console.WriteLine($" Current Coins: {p.coins}");
                Console.WriteLine($" Weapon Strength: {p.weaponValue}    ");
                Console.WriteLine($" Armor Class: {p.armorValue}    ");
                Console.WriteLine($" Potions: {p.potion}    ");
                Console.WriteLine($" Difficulty Mods: {p.mods}    ");
                Console.WriteLine("=====================");
                //wait for input
                string input = Console.ReadLine().ToLower();
                while (input != "w" && input != "a" && input != "d" && input != "p" && input != "e" && input != "h" && input != "q" && input.Length != 1)
                {
                    Console.WriteLine("You did not enter a valid value.  Please reenter.");
                    input = Console.ReadLine().ToLower();
                }
                if (input == "w")
                {
                    TryBuy("weapon", weaponP, p);
                }
                else if (input == "a")
                {
                    TryBuy("armor", armorP, p);
                }
                else if (input == "d")
                {
                    TryBuy("diff", diffP, p);
                }
                else if (input == "p")
                {
                    TryBuy("potion", potionP, p);
                }
                else if (input == "h")
                {
                    TryBuy("health", healthP, p);
                }
                else if (input == "e")
                {
                    break;
                }
                else if (input == "q")
                {
                    Program.Quit();
                }
            }
        }



        static void TryBuy(string item, int cost, Player p)
        {
            if (p.coins >= cost)
            {
                if (item == "potion")
                    p.potion++;
                else if (item == "weapon")
                    p.weaponValue++;
                else if (item == "armor")
                    p.armorValue++;
                else if (item == "diff")
                    p.mods++;
                else if (item == "health")
                {
                    p.health++;
                    p.baseHealth++;
                }
                    

                p.coins -= cost;
            }
            else
            {
                Console.WriteLine($"You don't have enough got for {item}s.");
                Console.ReadKey();
            }
        }
    }
}

