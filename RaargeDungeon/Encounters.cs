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
            string weapon = GetWeapon();

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

                nm = GetName();
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
            string leaderMob = GetLeader(pwr);
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
                else if(action.ToLower() == "d")
                    tester = 2;
                else if(action.ToLower() == "r")
                    tester = 3;
                else if(action.ToLower() == "h")
                    tester = 4;
                else 
                    tester = 5;
                                
                while(tester != 1 && tester != 2 && tester != 3 && tester != 4 && action.Length > 1)
                {
                    Console.WriteLine("Invalid selection, choose again");
                    action = Console.ReadLine();
                }
                if (action.ToLower() == "a")
                {
                    int damage = rand.Next(1, pwr) - (Program.currentPlayer.armorValue + Program.currentPlayer.damageResit);
                    if (damage < 0 ) damage = 0;
                    int attack = rand.Next(0, Program.currentPlayer.weaponValue) + rand.Next(1, 4) + ((Program.currentPlayer.currentClass == Player.PlayerClass.Warrior || 
                        Program.currentPlayer.race == Player.Race.HalfOrc)?2:0) * 
                        ((Program.currentPlayer.currentClass == Player.PlayerClass.Ranger && rand.Next(1,4) == 3)?2:0) * ((Program.currentPlayer.race == Player.Race.Halfling && rand.Next(1, 4) == 3) ? 2 : 0) *
                        ((Program.currentPlayer.race == Player.Race.Elf && rand.Next(1, 4) == 3) ? 2 : 0) * ((Program.currentPlayer.currentClass == Player.PlayerClass.Rogue && rand.Next(1,4) == 3)?2:0) ;
                    string leader = GetAttackStart(attack);
                    string style = GetWeaponAttackStyle();

                    // attack  
                    Program.Print($"{leader}, {style} at {nm}, who attacks in return.");
                    Console.WriteLine($"You lose {damage} health.  However, you deal {attack} damage to {nm}.");
                    Program.currentPlayer.health -= damage;
                    hlt -= attack;
                    if (hlt <= 0)
                    {
                        Console.WriteLine($"{nm} was Slain!!");
                        monsterAlive = false;
                    }

                    // Rogue backstab
                    if (Program.currentPlayer.currentClass == Player.PlayerClass.Rogue && Program.currentPlayer.rand.Next(1, 6) == 5)
                    {
                        int backStabDamage = rand.Next(1, Program.currentPlayer.weaponValue) + Program.currentPlayer.level;

                        leader = GetAttackStart(attack);
                        style = GetWeaponAttackStyle();

                        // attack
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Program.Print($"Quickly stepping behind, {style} at {nm} dealing {backStabDamage} damage.");
                        Console.ResetColor();
                        hlt -= attack;
                        if (hlt <= 0)
                        {
                            Console.WriteLine($"{nm} was Slain!!");
                            monsterAlive = false;
                        }
                    }

                    if (Program.currentPlayer.race == Player.Race.Halfling && rand.Next(1, 11) == 5)
                    {
                        int purseCoins = rand.Next(1, 15) * ((Program.currentPlayer.mods * 1) + 10);

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Program.Print($"You notice a small purse hanging from {nm}'s belt");
                        Console.WriteLine($"You reach out and slit the purse swiftly catching {purseCoins} gold coins! ");
                        Console.ResetColor();
                        Program.currentPlayer.coins += purseCoins;
                    }
                    else if (Program.currentPlayer.currentClass == Player.PlayerClass.Rogue && rand.Next(1, 11) == 5)
                    {
                        int purseCoins = rand.Next(1, 15) * ((Program.currentPlayer.mods * 1) + 10);

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Program.Print($"You notice a small purse hanging from {nm}'s belt");
                        Console.WriteLine($"You reach out and slit the purse swiftly catching {purseCoins} gold coins! ");
                        Console.ResetColor();
                        Program.currentPlayer.coins += purseCoins;
                    }
                }
                else if (action.ToLower() == "d")
                {
                    // defend
                    int damage = (rand.Next(1, pwr) / 4) - (Program.currentPlayer.armorValue + Program.currentPlayer.damageResit); // incomming damage is 25% while defending
                    if (damage < 0 ) damage = 0;
                    int attack = (rand.Next(0, Program.currentPlayer.weaponValue) + rand.Next(1, 4))/2; // half damage dealt defending
                    var weapon = GetWeapon();

                    Program.Print($"As {nm} moves forward to attack, you ready your {weapon} to defend.");
                    Console.WriteLine($"You lose {damage} health.  However, you deal {attack} damage to {nm}.");
                    Program.currentPlayer.health -= damage;
                    hlt -= attack;
                    if(hlt <= 0)
                    {
                        Console.WriteLine($"{nm} was Slain!!");
                        monsterAlive = false;
                    }
                }
                else if (action.ToLower() == "r" && Program.currentPlayer.race != Player.Race.Dwarf)
                {
                    // run
                    if(Program.currentPlayer.currentClass != Player.PlayerClass.Ranger && rand.Next(0,2) == 0 &&
                        Program.currentPlayer.race != Player.Race.Halfling && Program.currentPlayer.race != Player.Race.Elf) 
                    {
                        int damage = rand.Next(1, pwr) - (Program.currentPlayer.armorValue + Program.currentPlayer.damageResit);
                        if (damage < 0 ) damage = 0;

                        Program.Print($"As you sprint away from {nm}, its strike catched you in the back, knocking you down.");
                        Console.WriteLine($"You lose {damage} health and are unable to escape");
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
                    if (Program.currentPlayer.currentClass != Player.PlayerClass.Ranger && rand.Next(0, 3) == 0 )
                    {
                        int damage = rand.Next(1, pwr) - (Program.currentPlayer.armorValue + Program.currentPlayer.damageResit);
                        if (damage < 0) damage = 0;

                        Program.Print($"As you sprint away from {nm}, its strike catched you in the back, knocking you down.");
                        Console.WriteLine($"You lose {damage} health and are unable to escape");
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
                    if(Program.currentPlayer.potion == 0)
                    {
                        // out of potions
                        int damage = (pwr/2) - (Program.currentPlayer.armorValue + Program.currentPlayer.damageResit);
                        if (damage < 0) 
                            damage = 0;

                        Program.Print("You feel around but find no potions!");
                        Console.WriteLine($"The {nm} strikes you with a blow and you lose {damage} health!");
                        Program.currentPlayer.health -= damage;
                    }
                    else
                    {
                        // have potions
                        int damage = (pwr/2)- (Program.currentPlayer.armorValue + Program.currentPlayer.damageResit);
                        if( damage < 0) 
                            damage = 0;

                        int heal = Program.currentPlayer.GetPotionHealValue();

                        Program.Print("You reach into your bag and pull out a pulsing read flask.  You take a drink from it");
                        Console.WriteLine($"You feel better and gain {heal} health!");
                        Program.currentPlayer.health += heal;

                        if (Program.currentPlayer.health > Program.currentPlayer.baseHealth) 
                            Program.currentPlayer.health = Program.currentPlayer.baseHealth;
                        Program.currentPlayer.potion -= 1;
                        Program.Print($"As you were occupied {nm} advanced and attacked you.");
                        Console.WriteLine($"You lose {damage} health.");
                        Program.currentPlayer.health -= damage;
                    }
                    if (monsterAlive)
                    {
                        Console.ReadKey();
                    }

                }
                if (Program.currentPlayer.health <= 0)
                {
                    // Death code
                    Program.Print($"As the {nm} delivers a massive blow you collapse to the floor.  Your conciousness fades, you have been slain!");
                    Console.ReadKey();
                    CheckPlayAgain();
                    
                }
                if (monsterAlive)
                    Console.ReadKey();
            }

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

        public static string GetLeader(int pwr)
        {
            var tempLeader = "";

            switch (pwr)
            {
                case 0:
                case 1:
                    tempLeader = "sickly";
                    break;
                case 2:
                case 3:
                    tempLeader = "young";
                    break;
                case 4:
                case 5:
                    tempLeader = "elder";
                    break;
                case 6:
                case 7:
                case 8:
                    tempLeader = "burly";
                    break;
                case 9:
                case 10:
                case 12:
                    tempLeader = "legendary";
                    break;
                default:
                    tempLeader = "mythical";
                    break;
            }

            return tempLeader;

        }

        public static string GetName()
        {
            Console.WriteLine("Made it to Get Name");
            string creature = "";
            switch (rand.Next(0, 5))
            {
                case 0:
                    creature = "Skeleton";
                    break;
                case 1:
                    creature = "Kobold";
                    break;
                case 2:
                    creature = "Ogre";
                    break;
                case 3:
                    creature = "Bugbear";
                    break;
                case 4:
                    creature = "Madman";
                    break;
                default:
                    creature = "Elemental";
                    break;

            }

            return creature;
        }

        public static string GetAttackStart(int att)
        {
            string attackStart = "";

            //Console.WriteLine($"att = {att}");
            //Console.ReadKey();

            if (att == 0)
            {
                switch(rand.Next(0, 5))
                {
                    case 0:
                        attackStart = "With the speed of a startled slug";
                        break;
                    case 1:
                        attackStart = "Fumbling as if you were half asleep";
                        break;
                    case 2:
                        attackStart = "You wield your weapon awkwardly";
                        break;
                    case 3:
                        attackStart = "Caught completely offguard";
                        break;
                    case 4:
                        attackStart = "Failing to get any momentum";
                        break;
                    default:
                        attackStart = "Feeling as if luck were not on your side";
                        break;
                }
            }
            else if (att == 1 || att == 2)
            {
                switch(rand.Next(0, 3))
                {
                    case 0:
                        attackStart = "Looking as if you stumbled";
                        break;
                    case 1:
                        attackStart = "Moving with a purpose";
                        break;
                    case 2:
                        attackStart = "You know lady luck was on your side";
                        break;
                    default:
                        attackStart = "Happening to do the right thing";
                        break;
                }
            }
            else if (att >= 3)
            {
                switch (rand.Next(0, 3))
                {
                    case 0:
                        attackStart = "With the ferocity of a stiking cobra";
                        break;
                    case 1:
                        attackStart = "Swooping with the termerity of a falcon";
                        break;
                    case 2:
                        attackStart = "Moving as a force of nature";
                        break;
                    default:
                        attackStart = "Flowing like a raging waterfall";
                        break;
                }
            }

            return attackStart;
        }

        public static string GetWeaponAttackStyle()
        {
            var style = "";

            //Console.WriteLine(Program.currentPlayer.currentClass);
            if (Program.currentPlayer.currentClass.ToString() == "Warrior")
            {
                style = rand.Next(0, 3) switch
                {
                    0 => "you chop with your sword",
                    1 => "you thrust your sword",
                    2 => "you swing in a wide arching strike",
                    _ => "you swing your sword",
                };
            }
            else if (Program.currentPlayer.currentClass.ToString() == "Ranger")
            {
                switch (rand.Next(0, 3))
                {
                    case 0:
                        style = "you leap backwards firing your bow";
                        break;
                    case 1:
                        style = "you quickly draw firing your bow";
                        break;
                    case 2:
                        style = "you notch an arrow and loose it";
                        break;
                    default:
                        style = "you fire an arrow";
                        break;
                }
                
            }
            else if (Program.currentPlayer.currentClass.ToString() == "Mage")
            {
                switch (rand.Next(0, 3))
                {
                    case 0:
                        style = "you gesture while chanting";
                        break;
                    case 1:
                        style = "you gesture while squeezing a component in your other hand";
                        break;
                    case 2:
                        style = "you quickly cast a spell";
                        break;
                    default:
                        style = "you gesture";
                        break;
                }
            }
            else if (Program.currentPlayer.currentClass.ToString() == "Rogue")
            {
                switch (rand.Next(0, 3))
                {
                    case 0:
                        style = "you dance hypnotically with your blades";
                        break;
                    case 1:
                        style = "you lead high and low with your blades";
                        break;
                    case 2:
                        style = "you swing your blades with a flourish";
                        break;
                    default:
                        style = "you stab with your blades";
                        break;
                }
            }
            //Console.WriteLine($"Style: {style}");
            return style;
        }

        public static string GetWeapon()
        {
            string weapon = "";

            if(Program.currentPlayer.currentClass.ToString() == "Warrior")
            {
                weapon = "sword";
            }
            else if (Program.currentPlayer.currentClass.ToString() == "Ranger")
            {
                weapon = "longbow";
            }
            else if (Program.currentPlayer.currentClass.ToString() == "Mage")
            {
                weapon = "staff";
            }
            else if (Program.currentPlayer.currentClass.ToString() == "Rogue")
            {
                weapon = "twin daggers";
            }

            return weapon;

        }

        #endregion

        public static void CheckPlayAgain()
        {
            Console.Clear();
            Program.Print("Would you like to start a new game? Yes or No");
            string answer = Console.ReadLine().ToLower();

            bool tester = false;

            while(tester == false)
            if (answer == "yes")
            {
                    System.Diagnostics.Process.Start(System.AppDomain.CurrentDomain.FriendlyName);
                    Environment.Exit(0);
            }
            else if (answer == "no")
            {
                Program.Print("Thank you for playing. Goodbye!");
                System.Environment.Exit(0);
            }
            else 
                System.Environment.Exit(0);
        }
    }
}
