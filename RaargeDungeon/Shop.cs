using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaargeDungeon
{
    public class Shop
    {
        static Random rando = new Random();
        public static void loadShop(Player p)
        {
            RunShop(p);
        }

        #region RunShop
        public static void RunShop(Player p)
        {
            int potionP;
            int armorP;
            int weaponP;
            int diffP;
            int healthP;

            while (true)
            {
                potionP = GetPotionCost(p);
                armorP = GetArmorCost(p);
                weaponP = GetWeaponCost(p);
                diffP = GetDiffpCost(p);
                healthP = GetHealthCost(p); 

                Console.Clear();
                Console.WriteLine("           SHOP        ");
                Console.WriteLine("==========================");
                Console.WriteLine($" (W)eapon:     ${weaponP}    ");
                Console.WriteLine($" (A)rmor:      ${armorP}    ");
                Console.WriteLine($" (D)ifficulty: ${diffP}    ");
                Console.WriteLine($" (P)otions:    ${potionP}    ");
                Console.WriteLine($" (H)ealth:     ${healthP}    ");
                Console.WriteLine("==========================");
                Console.WriteLine($" (E)xit Shop:    ");
                Console.WriteLine($" (Q)uit Game:    ");
                Console.WriteLine("==========================");

                Console.WriteLine(" ");
                Console.WriteLine($" {p.name} Stats     ");
                Console.WriteLine($" Class: {p.currentClass}");
                Console.WriteLine($" Level: {p.level}");
                Console.Write(" Health Bar: ");
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.Red;
                Program.ProgressBar("<", "-", ((decimal)Program.currentPlayer.health / (decimal)Program.currentPlayer.baseHealth), 25);
                Console.ResetColor();
                Console.WriteLine("]");
                Console.Write(" XP Bar: ");
                Console.Write("    [");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Program.ProgressBar(">", "-", ((decimal)p.xp / (decimal)(p.GetLevelUpValue())), 25);
                Console.ResetColor();
                Console.WriteLine("]");
                Console.WriteLine("==========================");
                Console.WriteLine($" Current Health: {p.health}");
                Console.WriteLine($" Base Health: {p.baseHealth}");
                Console.WriteLine($" Current Coins: {p.coins}");
                Console.WriteLine($" Weapon Strength: {p.weaponValue}    ");
                Console.WriteLine($" Armor Class: {p.armorValue}    ");
                Console.WriteLine($" Potions: {p.potion}    ");
                Console.WriteLine($" Difficulty Mods: {p.mods}    ");
                          
                Console.WriteLine("==========================");
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
        #endregion


        #region TryBuying
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
        #endregion

        #region Set Potion costs with modifiers
        static int GetPotionCost(Player p)
        {
            int cost = 0;
            

            if (Program.currentPlayer.race == Player.Race.Human)
            {
                cost = 18 + (8 * p.mods);
            }
            else if (Program.currentPlayer.race == Player.Race.HalfOrc)
            {
                cost = 22 + (11 * p.mods);
            }
            else if (Program.currentPlayer.race == Player.Race.Halfling && rando.Next(1, 21) == 5)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Program.Print("You notice a potion and are certain you can steal it without being seen.");
                Console.ReadKey();
                Console.ResetColor();

                cost = 0;
                
            }
            else if (Program.currentPlayer.currentClass == Player.PlayerClass.Rogue && rando.Next(1, 16) == 5)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Program.Print("You notice a potion and are certain you can steal it without being seen..");
                Console.ReadKey();
                Console.ResetColor();

                cost = 0;

            }
            else
                cost = 20 + (10 * p.mods);

            return cost;
        }
        #endregion

        #region Get Armor Cost with Modifiers
        static int GetArmorCost(Player p)
        {
            int cost = 0;
            

            if (Program.currentPlayer.race == Player.Race.Human)
            {
                cost = 90 * (p.armorValue + 1);
            }
            else if (Program.currentPlayer.race == Player.Race.HalfOrc)
            {
                cost = 110 * (p.armorValue + 1);
            }
            else if (Program.currentPlayer.race == Player.Race.Halfling && rando.Next(1, 21) == 5)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Program.Print("You notice some armor and are certain you can steal it without being seen.");
                Console.ReadKey();
                Console.ResetColor();

                cost = 0;
                
            }
            else if (Program.currentPlayer.currentClass == Player.PlayerClass.Rogue && rando.Next(1, 16) == 5)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Program.Print("You notice some armor and are certain you can steal it without being seen..");
                Console.ReadKey();
                Console.ResetColor();

                cost = 0;

            }
            else
                cost = 100 * (p.armorValue + 1);

            return cost;
        }
        #endregion

        static int GetWeaponCost(Player p)
        {
            int cost = 0;
            
            if (Program.currentPlayer.race == Player.Race.Human)
            {
                cost = 90 * p.weaponValue;
            }
            else if (Program.currentPlayer.race == Player.Race.HalfOrc)
            {
                cost = 110 * p.weaponValue;
            }
            else if (Program.currentPlayer.race == Player.Race.Halfling && rando.Next(1, 21) == 5)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Program.Print("You notice a weapon and are certain you can steal it without being seen..");
                Console.ReadKey();
                Console.ResetColor();

                cost = 0;
                
            }
            else if (Program.currentPlayer.currentClass == Player.PlayerClass.Rogue && rando.Next(1, 16) == 5)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Program.Print("You notice a weapon and are certain you can steal it without being seen..");
                Console.ReadKey();
                Console.ResetColor();

                cost = 0;

            }
            else
                cost = 100 * p.weaponValue;

            return cost;
        }

        static int GetDiffpCost(Player p)
        {
            int cost = 0;
            

            if (Program.currentPlayer.race == Player.Race.Human)
            {
                cost = 270 * (90 * (p.mods + 1));
            }
            else if (Program.currentPlayer.race == Player.Race.HalfOrc)
            {
                cost = 330 * (110 * (p.mods + 1));
            }
            else
                cost = 300 * (100 * (p.mods + 1));

            
            return cost;
        }

        static int GetHealthCost(Player p)
        {
            int cost = 0;
            
            if (Program.currentPlayer.race == Player.Race.Human)
            {
                cost = 36 * p.baseHealth;
            }
            else if (Program.currentPlayer.race == Player.Race.HalfOrc)
            {
                cost = 44 * p.baseHealth;
            }
            else
                cost = 40 * p.baseHealth;

            return cost;
        }
    }
}

