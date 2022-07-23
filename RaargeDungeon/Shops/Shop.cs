using RaargeDungeon.Creatures;
using RaargeDungeon.Helpers;
using RaargeDungeon.Encounter;
using RaargeDungeon.Shops;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaargeDungeon.Shops
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
            int manaP;
            int armorP;
            int weaponP;
            int diffP;
            int healthP;
            int lsP;

            while (true)
            {
                potionP = GetPotionCost(p, "health");
                manaP = GetPotionCost(p, "mana");
                armorP = GetArmorCost(p);
                weaponP = GetWeaponCost(p);
                diffP = GetDiffpCost(p);
                healthP = GetHealthCost(p);
                lsP = GetLifesageCost(p);

                Console.Clear();
                Console.WriteLine("           SHOP        ");
                Console.WriteLine("=========================================================");
                Console.WriteLine($" (W)eapon:      ${weaponP} (A)rmor:       ${armorP}   ");
                Console.WriteLine($" (D)ifficulty:  ${diffP}   (P)otions:     ${potionP}");
                if (p.currentClass == Player.PlayerClass.Monk)
                    Console.WriteLine($" (H)ealth:      ${healthP}   ");
                else
                    Console.WriteLine($" (M){p.manaType} Potion: ${potionP} (H)ealth:      ${healthP}   ");
                Console.WriteLine($" (L)ifesage:    ${lsP}    ");
                Console.WriteLine("=========================================================");
                Console.WriteLine("  Magic (S)hop - Coming soon Stat (T)raining - Comming soon");
                Console.WriteLine("=========================================================");
                Console.WriteLine($" (E)xit Shop              (Q)uit Game ");
                Console.WriteLine("=========================================================");

                Console.WriteLine(" ");
                Console.WriteLine($" {p.name} Stats     ");
                Console.WriteLine($" Level: {p.level} Class: {p.currentClass} Race: {p.race}");

                //-- Health Bar --
                UIHelpers.GenerateStatusBar("Health", "<", "-", ConsoleColor.Red, p.health, p.baseHealth);

                // -- Experiance Bar --
                UIHelpers.GenerateStatusBar("XP", ">", "-", ConsoleColor.Yellow, p.xp, p.GetLevelUpValue(p.level));

                // -- Mana Bar --
                UIHelpers.GenerateStatusBar(p.manaType.ToString(), "*", " ", ConsoleColor.Blue, p.energy, p.baseEnergy);

                Console.WriteLine("==============================================================");
                Console.WriteLine($" Current Health: {p.health} Base Health: {p.baseHealth}");
                Console.WriteLine($" Current Coins: {p.coins}   Armor Value: {p.armorValue}");
                Console.WriteLine($" Weapon Strength: {p.weaponValue} Armor Class: {p.armorclass}   ");
                Console.WriteLine($" Favors: {p.favors} Lifetime Favors: {p.lifetimeFavors}   ");
                if(p.currentClass == Player.PlayerClass.Monk)
                    Console.WriteLine($" Healing Potions: {p.potion} ");
                else
                    Console.WriteLine($" Healing Potions: {p.potion} {p.manaType} Potions: {p.manaPotion}   ");
                Console.WriteLine($" Difficulty Mods: {p.mods}    ");

                Console.WriteLine("===============================================================");
                //wait for input
                string input = Console.ReadLine().ToLower();
                while (input != "w" && input != "a" && input != "d" && input != "p" && input != "e" && input != "h" && input != "q" && input != "l" && input != "m" && 
                    input != "s" && input.Length != 1)
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
                else if (input == "m")
                    TryBuy("mana", potionP, p);
                else if (input == "h")
                {
                    TryBuy("health", healthP, p);
                }
                else if (input == "e")
                {
                    //break;
                    Encounters.RandomEncounter(p);
                }
                else if (input == "q")
                {
                    Program.Quit();
                }
                else if (input == "l")
                {
                    TryBuy("lifesage", lsP, p);
                }
                else if (input == "s")
                {
                    MagicShop.RunMagicShop(p);
                }
            }
        }
        #endregion

        #region Get Lifesage Cost
        public static int GetLifesageCost(Player p)
        {
            int lsCost = 0;

            lsCost = p.level * (p.lifetimeFavors + 25);

            return lsCost;
        }
        #endregion

        #region TryBuying
        public static void TryBuy(string item, int cost, Player p)
        {
            if (p.coins >= cost)
            {
                if (item == "potion")
                    p.potion++;
                else if (item == "mana")
                    p.manaPotion++;
                else if (item == "weapon")
                    p.weaponValue++;
                else if (item == "armor")
                {
                    p.armorValue++;
                    p.armorclass += (p.armorValue/4);
                }
                else if (item == "diff")
                    p.mods++;
                else if (item == "health")
                {
                    p.health++;
                    p.baseHealth++;
                }
                else if (item == "lifesage")
                {
                    TryFavor(p);
                }


                p.coins -= cost;
            }
            else
            {
                Console.WriteLine($"You don't have enough got for {item}s.");
                Console.ReadKey();
            }
        }

        public static void TryFavor(Player p)
        {
            int tryFav = p.GetFavor();

            UIHelpers.Print("You purchase lifesage and place it on the altar by the counter");
            UIHelpers.Print("You quietly chant a prayer to your god");
            Console.WriteLine(" ");
            UIHelpers.Print("The lifesage slowly fades away.....");
            Console.ForegroundColor = ConsoleColor.Green;

            if (tryFav == 0)
            {
                UIHelpers.Print("You feel a sense of lonliness in the cosmos.");
            }
            else if (tryFav == 1)
            {
                UIHelpers.Print("You feel a sudden sense of joy and realize you are not alone in the universe.");
                UIHelpers.Print("You wipe the tears of joy from your eyes.");
            }
            Console.ResetColor();
            Console.ReadKey();

        }
        #endregion

        #region Set Potion costs with modifiers
        public static int GetPotionCost(Player p, string type)
        {
            int cost = 0;


            if (Program.currentPlayer.race == Player.Race.Human)
            {
                cost = (int)((20.0m + 10 * p.mods) * 0.90m);
            }
            else if (Program.currentPlayer.race == Player.Race.HalfOrc)
            {
                cost = (int)((20m + 10m * p.mods) * 1.1m);
            }
            else if (Program.currentPlayer.race == Player.Race.Halfling && rando.Next(1, 21) == 5)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                UIHelpers.Print($"You notice a {type} potion and are certain you can steal it without being seen.");
                Console.ReadKey();
                Console.ResetColor();

                cost = 0;

            }
            else if (Program.currentPlayer.currentClass == Player.PlayerClass.Rogue && rando.Next(1, 16) == 5)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                UIHelpers.Print($"You notice a {type} potion and are certain you can steal it without being seen..");
                Console.ReadKey();
                Console.ResetColor();

                cost = 0;

            }
            else
                cost = 20 + 10 * p.mods;

            return cost;
        }
        #endregion

        #region Get Armor Cost with Modifiers
        public static int GetArmorCost(Player p)
        {
            int cost = 0;

            if (Program.currentPlayer.race == Player.Race.Human)
            {
                if (Program.currentPlayer.currentClass == Player.PlayerClass.Warrior || Program.currentPlayer.currentClass == Player.PlayerClass.Cleric)
                    cost = 90 * p.armorValue;
                else if (Program.currentPlayer.currentClass == Player.PlayerClass.Monk)
                    cost = 90 * (p.armorValue - 1);
                else
                    cost = 90 * (p.armorValue + 1);
            }
            else if (Program.currentPlayer.race == Player.Race.HalfOrc)
            {
                if (Program.currentPlayer.currentClass == Player.PlayerClass.Warrior || Program.currentPlayer.currentClass == Player.PlayerClass.Cleric)
                    cost = 110 * p.armorValue;
                else if (Program.currentPlayer.currentClass == Player.PlayerClass.Monk)
                    cost = 110 * (p.armorValue - 1);
                else
                    cost = 110 * (p.armorValue + 1);
            }
            else if (Program.currentPlayer.currentClass == Player.PlayerClass.Warrior || Program.currentPlayer.currentClass == Player.PlayerClass.Cleric)
                cost = 100 * p.armorValue;
            else if (Program.currentPlayer.currentClass == Player.PlayerClass.Monk)
                cost = 100 * (p.armorValue - 1);
            else if (Program.currentPlayer.race == Player.Race.Halfling && rando.Next(1, 21) == 5)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                UIHelpers.Print("You notice some armor and are certain you can steal it without being seen.");
                Console.ReadKey();
                Console.ResetColor();

                cost = 0;

            }
            else if (Program.currentPlayer.currentClass == Player.PlayerClass.Rogue && rando.Next(1, 16) == 5)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                UIHelpers.Print("You notice some armor and are certain you can steal it without being seen..");
                Console.ReadKey();
                Console.ResetColor();

                cost = 0;

            }
            else
                cost = 100 * (p.armorValue + 1);

            return cost;
        }
        #endregion

        public static int GetWeaponCost(Player p)
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
                UIHelpers.Print("You notice a weapon and are certain you can steal it without being seen..");
                Console.ReadKey();
                Console.ResetColor();

                cost = 0;

            }
            else if (Program.currentPlayer.currentClass == Player.PlayerClass.Rogue && rando.Next(1, 16) == 5)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                UIHelpers.Print("You notice a weapon and are certain you can steal it without being seen..");
                Console.ReadKey();
                Console.ResetColor();

                cost = 0;

            }
            else
                cost = 100 * p.weaponValue;

            return cost;
        }

        public static int GetDiffpCost(Player p)
        {
            int cost = 0;


            if (Program.currentPlayer.race == Player.Race.Human)
            {
                cost = 270 * 90 * (p.mods + 1);
            }
            else if (Program.currentPlayer.race == Player.Race.HalfOrc)
            {
                cost = 330 * 110 * (p.mods + 1);
            }
            else
                cost = 300 * 100 * (p.mods + 1);


            return cost;
        }

        public static int GetHealthCost(Player p)
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

