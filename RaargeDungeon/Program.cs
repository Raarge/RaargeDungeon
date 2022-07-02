using System;

namespace RaargeDungeon
{
    class Program
    {
        public static Player currentPlayer = new Player();
        public static bool mainLoop = true;

        static void Main(string[] args)
        {
            Start();
            Encounters.FirstEncounter();
            while (mainLoop)
            {
                Encounters.RandomEncounter();
            }
        }

        static void Start()
        {
            Console.WriteLine("Raarge's Dungeon");
            Console.WriteLine("What is your characters name? ");
            currentPlayer.name = Console.ReadLine();
            while(currentPlayer.name == "" || currentPlayer.name is null || currentPlayer.name == " ")
            {
                Console.WriteLine("Surely you meant to put in your name?");
                Console.WriteLine("What is your characters name? ");
                currentPlayer.name = Console.ReadLine();
            }
            Console.Clear();
            Console.WriteLine("You awake on a stone slab in a dark room.");
            Console.WriteLine($"Welcome {currentPlayer.name}");
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("You grope around in the dark trying to figure out where you are.  Your head is pounding");
            Console.WriteLine("and you rub the back of your neck only to find a small incision.  You notice a small");
            Console.WriteLine("strip of light along the floor and realize it is a door.  You move towards it and feel");
            Console.WriteLine("for the door handle.  You turn the handle and meet with resistance which suddenly gives");
            Console.WriteLine("way and you hear the door unlatch...");
            Console.ReadKey();
            Console.Clear();

        }
    }
}
