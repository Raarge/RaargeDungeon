using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace RaargeDungeon
{
    
    public class Encounters
    {
        #region EncounterGeneric
        static Random rand = new Random();
        // Encounter Generic

        #endregion

        #region Encounter
        public static void FirstEncounter()
        {
            string weapon = Helpers.GetWeapon();

            Program.Print($"You throw open the door, grab a {weapon} laying on the table by the door and charge");
            Program.Print("your captor.  He turns...");
            Console.ReadKey();
            Combat(false, "Human Rogue", 1, 4);
        }

        public static void BasicFightEncounter()
        {
            Console.Clear();
            Program.Print("You turn the corner and there you see a ....");
            Console.ReadKey();
            Combat(true, "", 0, 0);
        }

        public static void WizardEncounter()
        {
            Console.Clear();
            Program.Print("The door slowly creaks open as you peer into the dark room.  You see a tall man with a");
            Program.Print("long beard looking at a large tome.  He looks up from his tome and stares at you menacingly....");
            Console.ReadKey();
            Combat(false, "Dark Wizard", rand.Next((1 + Program.currentPlayer.mods),(4 + Program.currentPlayer.mods)), rand.Next((4 + Program.currentPlayer.mods), 
                ((2 * Program.currentPlayer.mods) + 7)));
        }
        #endregion

        #region EncounterTools
        //Encounter Tools
        public static void RandomEncounter()
        {
            switch(rand.Next(0, 2))
            {
                case 0:
                    BasicFightEncounter();
                    break;
                case 1:
                    WizardEncounter();
                    break;
            }
        }
        public static void Combat(bool random, string name, int power, int health)
        {
            string nm = "";
            int pwr = 0;
            int hlt = 0;
            int baseHlt = 0;
            

            if (random)
            {
                //Console.WriteLine("Made it to Combat");

                nm = Helpers.GetName();
                pwr = Program.currentPlayer.GetPower() + ((nm == "Bugbear" || nm == "Ogre")?2:0);
                hlt = Program.currentPlayer.GetHealth() + ((nm == "Bugbear" || nm == "Ogre")?Program.currentPlayer.mods * 2: 0);
                baseHlt = hlt;
            }
            else
            {
                nm = name;
                pwr = power;
                hlt = health;
                baseHlt = hlt;

            }
            string leaderMob = Helpers.GetLeader(pwr);
            bool monsterAlive = true;

            while (hlt > 0)
            {
                Console.Clear();
                Console.WriteLine($"A {leaderMob} {nm} is here, looking menacingly at you");
                Console.Write($" {nm}'s Health Bar: ");
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.Red;
                Program.MonsterBar("<", "-", ((decimal)hlt / (decimal)baseHlt), 25);
                Console.ResetColor();
                Console.WriteLine("]");
                Console.WriteLine(" ");
                Console.WriteLine("============");
                Console.WriteLine("| (A)ttack |");
                Console.WriteLine("| (D)efend |");
                Console.WriteLine("| (R)un    |");
                Console.WriteLine("| (H)eal   |");
                Console.WriteLine("============");
                Console.WriteLine($"Potions: {Program.currentPlayer.potion} Coins: {Program.currentPlayer.coins}");
                Console.WriteLine($"Level: {Program.currentPlayer.level}");
                Console.Write(" Health Bar: ");
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.Red;
                Program.ProgressBar("<", "-", ((decimal)Program.currentPlayer.health / (decimal)Program.currentPlayer.baseHealth), 25);
                Console.ResetColor();
                Console.WriteLine("]");
                Console.Write(" XP Bar: ");
                Console.Write("    [");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Program.ProgressBar(">", "-", ((decimal)Program.currentPlayer.xp / (decimal)Program.currentPlayer.GetLevelUpValue()), 25);
                Console.ResetColor();
                Console.WriteLine("]");
                string action = Console.ReadLine();

                int tester = 0;
                if (action.ToLower() == "a")
                    tester = 1;
                else if (action.ToLower() == "d")
                    tester = 2;
                else if (action.ToLower() == "r")
                    tester = 3;
                else if (action.ToLower() == "h")
                    tester = 4;
                else
                    tester = 5;

                while (tester != 1 && tester != 2 && tester != 3 && tester != 4 && action.Length > 1)
                {
                    Console.WriteLine("Invalid selection, choose again");
                    action = Console.ReadLine();
                }

                int damage = 0;
                bool monsterCrit = false;
                int critCheck = rand.Next(1, 7);

                // Monster Damage
                if(critCheck == 3)
                {
                    damage = rand.Next(1, pwr) - (Program.currentPlayer.armorValue + Program.currentPlayer.damageResit);
                    damage = damage * 2;
                    if (damage < 0) damage = 0;
                    monsterCrit = true;
                }
                else
                {
                    damage = rand.Next(1, pwr) - (Program.currentPlayer.armorValue + Program.currentPlayer.damageResit);
                    if (damage < 0) damage = 0;

                }
                

                if (action.ToLower() == "a")
                {

                    int attack = 0;
                    string leader = "";
                    string style = "";
                    string companion = "";
                    string spellType = "";




                    // Ranger, Halfling, Elf 33% Crit Chance, 2X Damage
                    if ((Program.currentPlayer.currentClass == Player.PlayerClass.Ranger || Program.currentPlayer.race == Player.Race.Halfling ||
                        Program.currentPlayer.race == Player.Race.Elf) && rand.Next(0, 3) == 2)
                    {


                        attack = rand.Next(0, Program.currentPlayer.weaponValue) + rand.Next(1, 4) + ((Program.currentPlayer.currentClass == Player.PlayerClass.Warrior ||
                            Program.currentPlayer.race == Player.Race.HalfOrc) ? 2 + (Program.currentPlayer.level / 2) : 0);
                        attack = attack * 2;
                        leader = Helpers.GetAttackStart(attack);
                        style = Helpers.GetWeaponAttackStyle();

                        // attack flavor text 
                        Program.Print($"{leader}, {style} at {nm}, who attacks in return.");
                        Helpers.GetMonsterHitLine(nm, damage, monsterCrit, action.ToLower());

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($"**Critical Hit ** you deal {attack} damage to {nm}.");
                        Console.ResetColor();

                        hlt -= attack;
                        if (hlt <= 0)
                        {
                            //Console.WriteLine($"{nm} was Slain!!");
                            monsterAlive = false;
                        }
                    }   // Rogue Crit Chance 25%, 2x Damage
                    else if (Program.currentPlayer.currentClass == Player.PlayerClass.Rogue && rand.Next(1, 5) == 3)
                    {

                        attack = rand.Next(0, Program.currentPlayer.weaponValue) + rand.Next(1, 4) + ((Program.currentPlayer.race == Player.Race.HalfOrc) ? 2 + (Program.currentPlayer.level / 2) : 0);
                        attack = attack * 2;
                        leader = Helpers.GetAttackStart(attack);
                        style = Helpers.GetWeaponAttackStyle();

                        //Console.WriteLine($"Attack Rogue Crit: {attack}");



                        // attack flavor text 
                        Program.Print($"{leader}, {style} at {nm}, who attacks in return.");
                        Helpers.GetMonsterHitLine(nm, damage, monsterCrit, action.ToLower());

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"**Critical Hit ** you deal {attack} damage to {nm}.");
                        Console.ResetColor();

                        hlt -= attack;
                        if (hlt <= 0)
                        {
                            //Console.WriteLine($"{nm} was Slain!!");
                            monsterAlive = false;
                        }
                    }   // Everyone not covered above Crit Chance 10%, 2x Damage
                    else if (Program.currentPlayer.currentClass != Player.PlayerClass.Ranger && Program.currentPlayer.race != Player.Race.Halfling &&
                        Program.currentPlayer.race != Player.Race.Elf && Program.currentPlayer.currentClass != Player.PlayerClass.Rogue && rand.Next(0, 10) == 2)
                    {

                        attack = rand.Next(0, Program.currentPlayer.weaponValue) + rand.Next(1, 4) + ((Program.currentPlayer.race == Player.Race.HalfOrc) ? 2 + (Program.currentPlayer.level / 2) : 0);
                        attack = attack * 2;
                        leader = Helpers.GetAttackStart(attack);
                        style = Helpers.GetWeaponAttackStyle();

                        // attack flavor text 
                        Program.Print($"{leader}, {style} at {nm}, who attacks in return.");
                        Helpers.GetMonsterHitLine(nm, damage, monsterCrit, action.ToLower());

                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine($"**Critical Hit ** you deal {attack} damage to {nm}.");
                        Console.ResetColor();

                        hlt -= attack;
                        if (hlt <= 0)
                        {
                            // Console.WriteLine($"{nm} was Slain!!");
                            monsterAlive = false;
                        }
                    }
                    else // standard damage no multiplier
                    {

                        attack = rand.Next(0, Program.currentPlayer.weaponValue) + rand.Next(1, 4) + ((Program.currentPlayer.currentClass == Player.PlayerClass.Warrior ||
                            Program.currentPlayer.race == Player.Race.HalfOrc) ? 2 + (Program.currentPlayer.level / 2) : 0);
                        leader = Helpers.GetAttackStart(attack);
                        style = Helpers.GetWeaponAttackStyle();

                        // attack  
                        Program.Print($"{leader}, {style} at {nm}, who attacks in return.");
                        Console.WriteLine($"You deal {attack} damage to {nm}.");
                        Helpers.GetMonsterHitLine(nm, damage, monsterCrit, action.ToLower());

                        hlt -= attack;
                        if (hlt <= 0)
                        {
                            //Console.WriteLine($"{nm} was Slain!!");
                            monsterAlive = false;
                        }
                    }


                    // Rogue backstab
                    Skills.BackStab(nm, ref hlt, ref monsterAlive, attack, ref leader, ref style);

                    // Ranger Animal Call 25% chance
                    Skills.AnimalCall(nm, ref hlt, ref monsterAlive, attack, ref leader, ref style, ref companion);

                    Skills.SpellBlast(nm, ref hlt, ref monsterAlive, attack, ref leader, ref style, ref spellType);

                    Skills.CombatStealing(nm);
                }
                else if (action.ToLower() == "d")
                {
                    // defend
                    int attack = (rand.Next(0, Program.currentPlayer.weaponValue) + rand.Next(1, 4)) / 2; // half damage dealt defending
                    var weapon = Helpers.GetWeapon();

                    Program.Print($"As {nm} moves forward to attack, you ready your {weapon} to defend.");
                    Console.WriteLine($"You deal {attack} damage to {nm}.");
                    Helpers.GetMonsterHitLine(nm, damage, monsterCrit, action.ToLower());

                    hlt -= attack;
                    if (hlt <= 0)
                    {
                        
                        monsterAlive = false;
                    }
                }
                else if (action.ToLower() == "r" && Program.currentPlayer.race != Player.Race.Dwarf)
                {
                    // run
                    if (Program.currentPlayer.currentClass != Player.PlayerClass.Ranger && rand.Next(0, 2) == 0 &&
                        Program.currentPlayer.race != Player.Race.Halfling && Program.currentPlayer.race != Player.Race.Elf)
                    {
                        Program.Print($"As you sprint away from {nm}, its strike catched you in the back, knocking you down.");
                        Helpers.GetMonsterHitLine(nm, damage, monsterCrit, action.ToLower());
                        Console.ReadKey();
                        Program.currentPlayer.health -= damage;
                    }
                    else
                    {
                        Program.Print($"You use your crazy moves to evade the {nm} and you successfully escape!");
                        Console.ReadKey();
                        Shop.loadShop(Program.currentPlayer);
                    };
                }
                else if (action.ToLower() == "r" && Program.currentPlayer.race == Player.Race.Dwarf)
                {
                    if (Program.currentPlayer.currentClass != Player.PlayerClass.Ranger && rand.Next(0, 3) == 0)
                    {
                        Program.Print($"As you sprint away from {nm}, its strike catched you in the back, knocking you down.");
                        Helpers.GetMonsterHitLine(nm, damage, monsterCrit, action.ToLower());
                        Console.ReadKey();
                        Program.currentPlayer.health -= damage;
                    }
                    else
                    {
                        Program.Print($"You use your crazy moves to evade the {nm} and you successfully escape!");
                        Console.ReadKey();
                        Shop.loadShop(Program.currentPlayer);
                    };
                }
                else if (action.ToLower() == "h")
                {
                    // heal
                    PotionHealing(nm, pwr, monsterCrit, action.ToLower());

                    if (monsterAlive)
                    {
                        Console.ReadKey();
                    }

                }
                CheckForDeath(nm);

                if (monsterAlive)
                    Console.ReadKey();
            }

            Console.WriteLine($"{nm} was Slain!!");

            int xp = Program.currentPlayer.GetXP();
            int cn = Program.currentPlayer.GetCoins();
            Program.Print($"As you stand victorious over the {nm}, its body dissolves into {cn} gold coins!");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Program.Print($"You gain {xp} XP!");
            Console.WriteLine($"You quickly pocket {cn} coins and go down a long hall");
            Console.ResetColor();
            Program.currentPlayer.coins += cn;
            Program.currentPlayer.xp += xp;

            if (Program.currentPlayer.CanLevelUp())
                Program.currentPlayer.LevelUp();

            Console.ReadKey();
        }                

        private static void PotionHealing(string nm, int pwr, bool monsterCrit, string action)
        {
            if (Program.currentPlayer.potion == 0)
            {
                // out of potions
                int damage = (pwr / 2) - (Program.currentPlayer.armorValue + Program.currentPlayer.damageResit);
                if (damage < 0)
                    damage = 0;

                Program.Print("You feel around but find no potions!");
                Console.WriteLine($"The {nm} strikes you with a blow and you lose {damage} health!");
                Program.currentPlayer.health -= damage;
            }
            else
            {
                // have potions
                int damage = (pwr / 2) - (Program.currentPlayer.armorValue + Program.currentPlayer.damageResit);
                if (damage < 0)
                    damage = 0;

                int heal = Program.currentPlayer.GetPotionHealValue();

                Program.Print("You reach into your bag and pull out a pulsing read flask.  You take a drink from it");
                Console.WriteLine($"You feel better and gain {heal} health!");
                Program.currentPlayer.health += heal;

                if (Program.currentPlayer.health > Program.currentPlayer.baseHealth)
                    Program.currentPlayer.health = Program.currentPlayer.baseHealth;
                Program.currentPlayer.potion -= 1;

                if (rand.Next(0, 2) == 0)
                {
                    Program.Print($"As you were occupied {nm} advanced and attacked you.");
                    Helpers.GetMonsterHitLine(nm, damage, monsterCrit, action);
                }
                else
                {
                    Program.Print($"As you were occupied {nm} advanced and attacked you.");
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
                Program.Print($"As the {nm} delivers a massive blow you collapse to the floor.  Your conciousness fades, you have been slain!");
                Console.ReadKey();

                if(Program.currentPlayer.favors > 0)
                {
                    Program.Print("You are suddenly surrounded by a blinding light. You are lifted into the air and given");
                    Program.Print("a new lease on life!");
                    Program.currentPlayer.favors--;
                    Program.currentPlayer.health = Program.currentPlayer.baseHealth;
                }
                else
                    CheckPlayAgain();

            }
        }  
        #endregion

        public static void CheckPlayAgain()
        {
            Console.Clear();
            Program.Print("Would you like to start a new game? Yes or No");
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
                Program.Print("Thank you for playing. Goodbye!");
                System.Environment.Exit(0);
            }
            else 
                System.Environment.Exit(0);
        }
    }
}
