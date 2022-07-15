using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using RaargeDungeon.Helpers;
using RaargeDungeon.Shops;
using RaargeDungeon.Items;
using System.Threading;
using System.Collections;
using RaargeDungeon.Creatures;
using RaargeDungeon.Combat;

namespace RaargeDungeon
{

    public class Encounters
    {
        #region EncounterGeneric
        static Random rand = new Random();
        // Encounter Generic

        #endregion

        #region Encounter
        public static void FirstEncounter(Player plyr)
        {
            //Player plyr = new Player();

            string weapon = TextHelpers.GetWeapon();
            if (Program.currentPlayer.currentClass == Player.PlayerClass.Mage)
            {
                SpellScroll MagicMissile = new SpellScroll();
                MagicMissile.Name = "Magic Missile";
                MagicMissile.ShortName = "mm";
                MagicMissile.Description = "Arcane missiles that do not miss.";
                MagicMissile.RequiredLevel = 1;
                MagicMissile.damage = 4;
                MagicMissile.type = "Arcane";
                MagicMissile.critMultiplier = 3;
                MagicMissile.SpellCost = 10;
                MagicMissile.minCost = 6;
                MagicMissile.flavorText = $"bolts of {MagicMissile.type} energy slam into your opponent.";
                MagicMissile.CurrentSpellCost = MagicMissile.SpellCost;

                UIHelpers.Print($"You throw open the door, grab a {weapon} and a Spell Scroll from a table.");
                UIHelpers.Print("As your hand touches the scroll it vanishes and you realize you know a spell.");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("You learn Magic Missile!");
                Console.ResetColor();
                Console.WriteLine("You quickly regain control and charge you captor...");
                Console.WriteLine(" ");

                plyr.spells.Add(MagicMissile);

                // test
                //Console.WriteLine("You know the following spells:");

                //foreach (SpellScroll spell in Program.currentPlayer.spells)
                //{
                //    Console.WriteLine($"{spell.Name} spell");
                //}
                //Console.ReadKey();
                // end of test
            }
            else
            {
                UIHelpers.Print($"You throw open the door, grab a {weapon} ");
                UIHelpers.Print("laying on the table by the door and charge your captor...");
            }
                

            
            
            Console.ReadKey();
            Combat(false, plyr);
        }

        public static void BasicFightEncounter(Player plyr)
        {
            Console.Clear();
            UIHelpers.Print("You turn the corner and there you see a ....");
            Console.ReadKey();
            Combat(true, plyr);
        }

        public static void WizardEncounter(Player p)
        {
            Console.Clear();
            UIHelpers.Print("The door slowly creaks open as you peer into the dark room.  You see a tall man with a");
            UIHelpers.Print("long beard looking at a large tome.  He looks up from his tome and stares at you menacingly....");
            Console.ReadKey();
            Combat(false, p);
        }
        #endregion

        #region EncounterTools
        //Encounter Tools
        public static void RandomEncounter(Player player)
        {
            //switch(rand.Next(0, 2))
            //{
                //case 0:
                    BasicFightEncounter(player);
                    //break;
                //case 1:
                  //  WizardEncounter(player);
                  //  break;
            //}
        }

        public static void Combat(bool random, Player plyr)
        {
            Monster mstr = new Monster();

            mstr = Monster.SpawnMonster(mstr, random, plyr.level);
            
            string leaderMob = TextHelpers.GetLeader(mstr.level);
            

            while (mstr.health > 0)
            {
                Console.Clear();
                Console.WriteLine($" A {leaderMob} {mstr.name} is here, looking menacingly at you");
                Console.Write($" {mstr.name}'s Health Bar: ");
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.Red;
                Program.MonsterBar("<", "-", ((decimal)mstr.health / (decimal)mstr.baseHealth), 25);
                Console.ResetColor();
                Console.WriteLine("]");
                Console.WriteLine(" ");
                Console.WriteLine(" =====================");
                Console.WriteLine(" | (A)ttack  (D)efend |");

                if(Program.currentPlayer.currentClass == Player.PlayerClass.Mage)
                {
                    Console.WriteLine(" | (C)ast             |");
                }
                

                Console.WriteLine(" | (R)un              |");
                Console.WriteLine(" | (H)eal    (M)Mana  |");
                Console.WriteLine(" |                    |");
                Console.WriteLine(" =====================");
                Console.WriteLine($" Level: {plyr.level} Class: {plyr.currentClass} Coins: {plyr.coins}");
                Console.WriteLine($" Potions: {plyr.potion} Mana Potions: {plyr.manaPotion}");

                // -- Health Bar --
                UIHelpers.GenerateStatusBar("Health", "<", "-", ConsoleColor.Red, plyr.health, plyr.baseHealth);
                
                // -- Experiance Bar --
                UIHelpers.GenerateStatusBar("XP", ">", "-", ConsoleColor.Yellow, plyr.xp, plyr.GetLevelUpValue());

                // -- Mana Bar --
                UIHelpers.GenerateStatusBar(plyr.manaType.ToString(), "*", " ", ConsoleColor.Blue, plyr.energy, plyr.baseEnergy);

                //Console.WriteLine($"XP:{Program.currentPlayer.xp} Needed XP: {Program.currentPlayer.GetLevelUpValue()} ");
                string action = Console.ReadLine().ToLower();

                if(Program.currentPlayer.currentClass != Player.PlayerClass.Mage)
                {
                    while (action != "a" && action != "d" && action != "r" && action != "h" && action != "m" && action.Length > 1)
                    {
                        Console.WriteLine("Invalid selection, choose again");
                        action = Console.ReadLine();
                    }
                }
                else 
                {
                    while (action != "a" && action != "d" && action != "r" && action != "h" && action != "m" && action != "c" && action.Length > 1)
                    {
                        Console.WriteLine("Invalid selection, choose again");
                        action = Console.ReadLine();
                    }
                }


                
                
                if (action.ToLower() == "a")
                {

                    string leader = "";
                    string style = "";
                    string companion = "";
                    string spellType = "";


                    Combatants c = new Combatants();

                    MartialCombat.DoAttack(plyr, mstr, "melee", action);

                    //special attacks


                    // Rogue backstab
                    mstr = Skills.BackStab(mstr, ref leader, ref style);

                    // Ranger Animal Call 25% chance
                    mstr = Skills.AnimalCall(mstr, ref leader, ref style, ref companion);

                    // Mage SpellBlast 
                    mstr = Skills.SpellBlast(mstr, ref leader, ref style, ref spellType);

                    // Combat Stealing
                    Skills.CombatStealing(mstr.name);

                    // Monk Chi Strike
                    mstr = Skills.ChiStrike(mstr, ref leader, ref style);

                    // Cleric Holy Strike
                    mstr = Skills.HolyStrike(mstr, ref leader, ref style);
                }
                else if (action.ToLower() == "d")
                {
                    // defend
                    int attack = Randomizer.GetRandomDieRoll(plyr.attackDie, plyr.numberAttackDie) / 2; // half damage dealt defending
                    var weapon = TextHelpers.GetWeapon();

                    UIHelpers.Print($"As {mstr.name} moves forward to attack, you ready your {weapon} to defend.");
                    Console.WriteLine($"You deal {attack} damage to {mstr.name}.");
    //                TextHelpers.GetMonsterHitLine(mstr.name, damage, monsterCrit, action.ToLower());

                    mstr.health -= attack;
                    if (mstr.health <= 0)
                    {
                        
                        mstr.IsAlive = false;
                    }
                }
                else if (action == "c")
                {
                    SpellScroll chosenSpell = new SpellScroll();
                    bool spellCrit = false;
                    bool spellFail = false;
                    int upper = Checkers.GetSpellFailUpper();
                    

                    chosenSpell = Pickers.GetChosenSpell();
                    int spellCost = Checkers.GetSpellCost(chosenSpell);

                    int spellAttack = (int)(Math.Ceiling((decimal)Randomizer.GetRandomDieRoll(chosenSpell.damage) + Program.currentPlayer.magicMastery)); 
                    // monster damage

                    if (rand.Next(0, upper) == 0)
                        spellFail = true;

                    if (rand.Next(1,11) == 3)
                        spellCrit = true;
                    // check for spell failure

                    if (spellFail)
                    {
                        Console.WriteLine($"You begin casting {chosenSpell.Name} but it backfires.");
                        spellAttack = 0;
                    }
                    else
                    {
                        if (spellCrit)
                        {
                            spellAttack = spellAttack * chosenSpell.critMultiplier;
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"{chosenSpell.Name} Critical!");
                            Console.ResetColor();
                        }

                        Console.WriteLine($"You begin casting {chosenSpell.Name}, you thrust your hand forward and");
                        Console.WriteLine(chosenSpell.flavorText);
                        Console.WriteLine($"A {mstr.name} attacks in return.");
                        Console.WriteLine($"You deal {spellAttack} {chosenSpell.type} damage to the {mstr.name}.");
                        
                    }
                    

 //                   TextHelpers.GetMonsterHitLine(mstr.name, damage, monsterCrit, action.ToLower());
                    Console.ReadKey();

                    Program.currentPlayer.energy -= chosenSpell.SpellCost;
                    Program.currentPlayer.spellCasting = Checkers.GetSkillXPGain(mstr.level, chosenSpell.SpellCost, chosenSpell.RequiredLevel, "cast");
                    Program.currentPlayer.spellChanneling = Checkers.GetSkillXPGain(mstr.level, chosenSpell.SpellCost, chosenSpell.RequiredLevel, "channel");
                    Program.currentPlayer.magicMastery = Checkers.GetSkillXPGain(mstr.level, chosenSpell.SpellCost, chosenSpell.RequiredLevel, "magic");

                    mstr.health -= spellAttack;
                    if (mstr.health <= 0)
                    {
                        //Console.WriteLine($"{nm} was Slain!!");
                        mstr.IsAlive = false;
                    }

                }
                else if (action.ToLower() == "r" && Program.currentPlayer.race != Player.Race.Dwarf)
                {
                    // run
                    if (Program.currentPlayer.currentClass != Player.PlayerClass.Ranger && rand.Next(0, 2) == 0 &&
                        Program.currentPlayer.race != Player.Race.Halfling && Program.currentPlayer.race != Player.Race.Elf)
                    {
                        UIHelpers.Print($"As you sprint away from {mstr.name}, its strike catched you in the back, knocking you down.");
 //                       TextHelpers.GetMonsterHitLine(mstr.name, damage, monsterCrit, action.ToLower());
                        Console.ReadKey();
 //                       Program.currentPlayer.health -= damage;
                    }
                    else
                    {
                        UIHelpers.Print($"You use your crazy moves to evade the {mstr.name} and you successfully escape!");
                        Console.ReadKey();
                        Shop.loadShop(Program.currentPlayer);
                    };
                }
                else if (action.ToLower() == "r" && Program.currentPlayer.race == Player.Race.Dwarf)
                {
                    if (Program.currentPlayer.currentClass != Player.PlayerClass.Ranger && rand.Next(0, 3) == 0)
                    {
                        UIHelpers.Print($"As you sprint away from {mstr.name}, its strike catched you in the back, knocking you down.");
  //                      TextHelpers.GetMonsterHitLine(mstr.name, damage, monsterCrit, action.ToLower());
                        Console.ReadKey();
  //                      Program.currentPlayer.health -= damage;
                    }
                    else
                    {
                        UIHelpers.Print($"You use your crazy moves to evade the {mstr.name} and you successfully escape!");
                        Console.ReadKey();
                        Shop.loadShop(Program.currentPlayer);
                    };
                }
                else if (action.ToLower() == "h")
                {
                    bool monsterCrit = false;
                    // heal
                    PotionHealing(mstr.name, mstr.level, monsterCrit, action.ToLower(), "health");

                    if (mstr.IsAlive)
                    {
                        Console.ReadKey();
                    }

                }
                else if (action.ToLower() == "m")
                {
 //                   PotionHealing(mstr.name, mstr.level, monsterCrit, action.ToLower(), "mana");

                    if (mstr.IsAlive)
                    {
                        Console.ReadKey();
                    }
                }
                CheckForDeath(mstr.name);

                if (mstr.IsAlive)
                    Console.ReadKey();
            }

            Console.WriteLine($"{mstr.name} was Slain!!");

            int xp = plyr.GetXP();
            int cn = plyr.GetCoins();
            UIHelpers.Print($"As you stand victorious over the {mstr.name}, its body dissolves into {cn} gold coins!");
            Console.ForegroundColor = ConsoleColor.Yellow;
            UIHelpers.Print($"You gain {xp} XP!");
            Console.WriteLine($"You quickly pocket {cn} coins and go down a long hall");
            Console.ResetColor();
            plyr.coins += cn;
            plyr.xp += xp;

            if (Program.currentPlayer.CanLevelUp())
                Program.currentPlayer.LevelUp();

            Console.ReadKey();
        }                

        private static void PotionHealing(string nm, int pwr, bool monsterCrit, string action, string type)
        {
            if (Program.currentPlayer.potion == 0)
            {
                // out of potions
                int damage = (pwr / 2) - (Program.currentPlayer.armorValue + Program.currentPlayer.damageResit);
                if (damage < 0)
                    damage = 0;

                UIHelpers.Print("You feel around but find no potions!");
                Console.WriteLine($"The {nm} strikes you with a blow and you lose {damage} health!");
                Program.currentPlayer.health -= damage;
            }
            else
            {
                // have potions
                int damage = (pwr / 2) - (Program.currentPlayer.armorValue + Program.currentPlayer.damageResit);
                if (damage < 0)
                    damage = 0;

                if (type == "health")
                {
                    int heal = Program.currentPlayer.GetPotionHealValue("health");

                    if (heal > Program.currentPlayer.baseHealth - Program.currentPlayer.health)
                        heal = Program.currentPlayer.baseHealth - Program.currentPlayer.health;

                    UIHelpers.Print("You reach into your bag and pull out a pulsing red flask.  You take a drink from it");
                    if (heal != 0)
                        Console.WriteLine($"You feel better and gain {heal} health!");
                    else
                        Console.WriteLine($"You feel better but gain {heal} health.");

                    Program.currentPlayer.health += heal;

                    if (Program.currentPlayer.health > Program.currentPlayer.baseHealth)
                        Program.currentPlayer.health = Program.currentPlayer.baseHealth;

                    Program.currentPlayer.potion -= 1;
                }
                else if (type == "mana")
                {
                    int mana = Program.currentPlayer.GetPotionHealValue("mana");

                    if (mana > Program.currentPlayer.baseEnergy - Program.currentPlayer.baseEnergy)
                        mana = Program.currentPlayer.baseEnergy - Program.currentPlayer.energy;

                    UIHelpers.Print("You reach into your bag and pull out a pulsing blue flask.  You take a drink from it.");
                    if (mana != 0)
                        Console.WriteLine($"You feel your power rise and gain {mana} {Program.currentPlayer.manaType}");
                    else
                        Console.WriteLine($"You feel your power attempt to rise but gain {mana} {Program.currentPlayer.manaType}");

                    Program.currentPlayer.energy += mana;

                    if (Program.currentPlayer.energy > Program.currentPlayer.baseEnergy)
                        Program.currentPlayer.energy = Program.currentPlayer.baseEnergy;

                    Program.currentPlayer.potion -= 1;
                }
                

                if (rand.Next(0, 2) == 0)
                {
                    UIHelpers.Print($"As you were occupied {nm} advanced and attacked you.");
                    TextHelpers.GetMonsterHitLine(nm, damage, monsterCrit, action);
                }
                else
                {
                    UIHelpers.Print($"As you were occupied {nm} advanced and attacked you.");
                    Console.WriteLine($"You quickly step out of the way.");
                }

                //Program.currentPlayer.health -= damage;
            }
        }

        public static void CheckForDeath(string nm)
        {
            if (Program.currentPlayer.health <= 0)
            {
                // Death code
                UIHelpers.Print($"As the {nm} delivers a massive blow you collapse to the floor.  Your conciousness fades, you have been slain!");
                Console.ReadKey();

                if(Program.currentPlayer.favors > 0)
                {
                    UIHelpers.Print("You are suddenly surrounded by a blinding light. You are lifted into the air and given");
                    UIHelpers.Print("a new lease on life!");
                    Program.currentPlayer.favors--;
                    Program.currentPlayer.health = Program.currentPlayer.baseHealth;
                    Program.currentPlayer.deaths += 1;
                }
                else
                    CheckPlayAgain();

            }
        }  
        #endregion

        public static void CheckPlayAgain()
        {
            Console.Clear();
            UIHelpers.Print("Would you like to start a new game? Yes or No");
            string answer = Console.ReadLine().ToLower();

            bool tester = false;

            while(tester == false)
            if (answer == "yes" || answer == "y")
            {
                    System.Diagnostics.Process.Start(System.AppDomain.CurrentDomain.FriendlyName);
                    Environment.Exit(0);
            }
            else if (answer == "no" || answer == "n")
            {
                UIHelpers.Print("Thank you for playing. Goodbye!");
                System.Environment.Exit(0);
            }
            else 
                System.Environment.Exit(0);
        }
    }
}
