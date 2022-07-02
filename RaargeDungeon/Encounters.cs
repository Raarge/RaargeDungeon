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
        static Random rand = new Random();
        // Encounter Generic


        // Encounters
        public static void FirstEncounter()
        {
            Console.WriteLine("You throw open the door, grab a rusty sword laying on the table by the door and charge");
            Console.WriteLine("your captor.  He turns...");
            Console.ReadKey();
            Combat(false, "Human Rogue", 1, 4);
        }

        public static void BasicFightEncounter()
        {
            Console.Clear();
            Console.WriteLine("You turn the corner and there you see a ....");
            Console.ReadKey();
            Combat(true, "", 0, 0);
        }

        public static void WizardEncounter()
        {
            Console.Clear();
            Console.WriteLine("The door slowly creaks open as you peer into the dark room.  You see a tall man with a");
            Console.WriteLine("long beard looking at a large tome.  He looks up from his tome and stares at you menacingly....");
            Console.ReadKey();
            Combat(false, "A Dark Wizard", 4, 2);
        }

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

            if (random)
            {
                //Console.WriteLine("Made it to Combat");
                nm = GetName();
                pwr = Program.currentPlayer.GetPower();
                hlt = Program.currentPlayer.GetHealth();
            }
            else
            {
                nm = name;
                pwr = power;
                hlt = health;

            }
            while (hlt > 0)
            {
                Console.Clear();
                Console.WriteLine($"{nm} is here, looking menacingly at you");
                Console.WriteLine($"Power: {pwr} Health: {hlt}");
                Console.WriteLine("============");
                Console.WriteLine("| (A)ttack |");
                Console.WriteLine("| (D)efend |");
                Console.WriteLine("| (R)un    |");
                Console.WriteLine("| (H)eal   |");
                Console.WriteLine("============");
                Console.WriteLine($"Potions: {Program.currentPlayer.potion} Health: {Program.currentPlayer.health} Coins: {Program.currentPlayer.coins}");
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
                    int damage = rand.Next(1, pwr) - Program.currentPlayer.armorValue;
                    if (damage < 0 ) damage = 0;
                    int attack = rand.Next(0, Program.currentPlayer.weaponValue) + rand.Next(1, 4);
                    string leader = GetAttackStart(attack);
                    // attack  // add random first parts later and random return attacks
                    Program.Print($"{leader}, you swing your weapon at {nm}, who attacks in return.");
                    Program.Print($"You lose {damage} health.  However, you deal {attack} damage to {nm}.");
                    Program.currentPlayer.health -= damage;
                    hlt -= attack;
                    if (hlt <= 0)
                    {
                        Console.WriteLine($"{nm} was Slain!!");
                        // go to store
                    }
                }
                else if (action.ToLower() == "d")
                {
                    // defend
                    int damage = (rand.Next(1, pwr) / 4) - Program.currentPlayer.armorValue; // incomming damage is 25% while defending
                    if (damage < 0 ) damage = 0;
                    int attack = (rand.Next(0, Program.currentPlayer.weaponValue) + rand.Next(1, 4))/2; // half damage dealt defending

                    Program.Print($"As {nm} moves forward to attack, you ready your weapon to defend.");
                    Program.Print($"You lose {damage} health.  However, you deal {attack} damage to {nm}.");
                    Program.currentPlayer.health -= damage;
                    hlt -= attack;
                    if(hlt <= 0)
                    {
                        Console.WriteLine($"{nm} was Slain!!");
                        // go to store
                    }
                }
                else if (action.ToLower() == "r")
                {
                    // run
                    if(rand.Next(0,2) == 0)
                    {
                        int damage = rand.Next(1, pwr) - Program.currentPlayer.armorValue;
                        if (damage < 0 ) damage = 0;

                        Program.Print($"As you sprint away from {nm}, its strike catched you in the back, knocking you down.");
                        Program.Print($"You lose {damage} health and are unable to escape");
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
                        int damage = (pwr/2) - Program.currentPlayer.armorValue;
                        if (damage < 0) damage = 0;
                        Program.Print("You feel around but find no potions!");
                        Program.Print($"The {nm} strikes you with a blow and you lose {damage} health!");
                        Program.currentPlayer.health -= damage;
                    }
                    else
                    {
                        int damage = (pwr/2)-Program.currentPlayer.armorValue;
                        if( damage < 0) damage = 0;
                        int heal = Program.currentPlayer.GetPotionHealValue();
                        Program.Print("You reach into your bag and pull out a pulsing read flask.  You take a drink from it");
                        Program.Print($"You gain {heal} health!");
                        Program.currentPlayer.health += heal;
                        if (Program.currentPlayer.health > Program.currentPlayer.baseHealth) Program.currentPlayer.health = Program.currentPlayer.baseHealth;
                        Program.currentPlayer.potion -= 1;
                        Program.Print($"As you were occupied {nm} advanced and attacked you.");
                        Program.Print($"You lose {damage} health.");
                        Program.currentPlayer.health -= damage;
                    }
                    Console.ReadKey();
                }
                if (Program.currentPlayer.health <= 0)
                {
                    // Death code
                    Program.Print($"As the {nm} delivers a massive blow you collapse to the floor.  Your conciousness fades, you have been slain!");
                    Console.ReadKey();
                    System.Environment.Exit(0);
                }
                Console.ReadKey();
            }
            int cn = Program.currentPlayer.GetCoins();
            Program.Print($"As you stand victorious over the {nm}, its body dissolves into {cn} gold coins!");
            Program.currentPlayer.coins += cn;
            Console.ReadKey();
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
                        attackStart = "Looking as if you stumbled into a good thing";
                        break;
                    case 1:
                        attackStart = "Moving with a purpose";
                        break;
                    case 2:
                        attackStart = "Grinning because you know lady luck was on your side";
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
                        attackStart = "Moving with the termerity of a swooping falcon";
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
    }
}
