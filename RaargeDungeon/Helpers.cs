using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaargeDungeon
{
    public class Helpers
    {
        public static Random rand = new Random();

        public static string GetSpellType()
        {
            string spellName = "";

            spellName = Program.currentPlayer.level switch
            {
                1 => "Magic Missile",
                2 => "Magic Missile",
                3 => "Dinazar Olken",
                4 => "Dinazar Olken",
                5 => "Dinazar Olken",
                6 => "Fireball",
                7 => "Fireball",
                8 => "Fireball",
                9 => "Fireball",
                _ => "Eldrich Blast",
            };

            return spellName;
        }

        public static string GetAnimalCompanion()
        {
            string comp = "";

            comp = Program.currentPlayer.level switch
            {
                1 => "Badger",
                2 => "Badger",
                3 => "Hyena",
                4 => "Hyena",
                5 => "Hyena",
                6 => "Wolf",
                7 => "Wolf",
                8 => "Wolf",
                9 => "Wolf",
                _ => "Dire Wolf",
            };

            return comp;
        }

        public static string GetAttackStart(int att)
        {
            string attackStart = "";

            //Console.WriteLine($"att = {att}");
            //Console.ReadKey();

            if (att == 0)
            {
                switch (rand.Next(0, 5))
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
                switch (rand.Next(0, 3))
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
                        style = "you swing widly with your staff";
                        break;
                    case 1:
                        style = "you thrust your staff forward";
                        break;
                    case 2:
                        style = "you chop downward with your staff";
                        break;
                    default:
                        style = "You swing your staff";
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

        public static void GetMonsterHitLine(string nm, int damage, bool monsterCrit, string action)
        {
            if (monsterCrit == true)
            { 
                if(damage != 0)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{nm} delivers a **Critical Hit**");
                    Console.ResetColor();
                }
                
            }

            if (damage == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                if (Program.currentPlayer.currentClass == Player.PlayerClass.Monk)
                    Program.Print("You quickly dodge out of the way.");
                else if (Program.currentPlayer.currentClass == Player.PlayerClass.Rogue || Program.currentPlayer.currentClass == Player.PlayerClass.Ranger)
                    Program.Print("At the last second, you lean out of the way of the incomming blow.");
                else
                    Program.Print("However, your armor protects you.");
                Console.ResetColor();
            }

            if (action == "r")
            {
                Console.WriteLine($"You lose {damage} health and are unable to escape");
            }
            else
                Console.WriteLine($"You lose {damage} health. ");

            Program.currentPlayer.health -= damage;
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

        public static string GetWeapon()
        {
            string weapon = "";

            if (Program.currentPlayer.currentClass.ToString() == "Warrior")
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
    }
}
