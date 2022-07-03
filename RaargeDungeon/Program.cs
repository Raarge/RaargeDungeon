using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Threading;

namespace RaargeDungeon
{
    class Program
    {
        public static Player currentPlayer = new Player();
        public static bool mainLoop = true;

        static void Main(string[] args)
        {
            if (!Directory.Exists("saves"))
            {
                Directory.CreateDirectory("saves");
            }

            currentPlayer = Load(out bool newP);
            if (newP)
            {
                Encounters.FirstEncounter();
            }

            while (mainLoop)
            {
                Encounters.RandomEncounter();
            }
        }

        static Player NewStart(int newId)
        {
            Player p = new Player();

            Console.Clear();
            Print("Raarge's Dungeon");
            Print("What is your characters name? ");
            p.name = Console.ReadLine();
            Print("Class: Mage Ranger Warrior");
            bool flag = false;
            while (flag == false)
            {
                flag = true;
                string input = Console.ReadLine().ToLower();
                if(input == "mage")
                {
                    p.currentClass = Player.PlayerClass.Mage;

                }
                else if (input == "ranger")
                {
                    p.currentClass = Player.PlayerClass.Ranger;
                }
                else if (input == "warrior")
                {
                    p.currentClass = Player.PlayerClass.Warrior;
                }
                else
                {
                    Console.WriteLine("Invalid selection, please choose a valid class.");
                    flag = false;
                }
            }
            p.id = newId;

            while(p.name == "" || p.name is null || p.name == " ")
            {
                Console.WriteLine("Surely you meant to put in your name?");
                Console.WriteLine("What is your characters name? ");
                p.name = Console.ReadLine();
            }
            Console.Clear();
            Print("You awake on a stone slab in a dark room..........", 30);
            Print($"Welcome {p.name}");
            Console.ReadKey();
            Console.Clear();
            Print("You grope around in the dark trying to figure out where you are.  Your head is pounding");
            Print("and you rub the back of your neck only to find a small incision.  You notice a small");
            Print("strip of light along the floor and realize it is a door.  You move towards it and feel");
            Print("for the door handle.  You turn the handle and meet with resistance which suddenly gives");
            Print("way and you hear the door unlatch...");
            Console.ReadKey();
            Console.Clear();

            return p;
        }

        public static void Quit()
        {
            
            Save();
            Environment.Exit(0);
        }

        public static void Save()
        {
            string path = $"saves/{currentPlayer.id.ToString()}.xml";
            string fileName = currentPlayer.id.ToString();
            string[] paths = Directory.GetFiles("saves");

            XmlSerializer serializer = new XmlSerializer(typeof(Player));
            TextWriter writer = new StreamWriter(path);
            XmlSerializer sr = new XmlSerializer (typeof(Player));
            serializer.Serialize(writer, currentPlayer);
            writer.Close();
            
        }

        public static Player Load(out bool newP)
        {
            newP = false;
            string[] paths = Directory.GetFiles("saves");
            List<Player> players = new List<Player>();
            int idCount = 0;

            
            
            FileStream fs;

            foreach (string p in paths)
            {
                

                using (fs = new FileStream(p, FileMode.Open, FileAccess.Read))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(Player));
                    Player player;
                    player = (Player)ser.Deserialize(fs);
                    //ser.Serialize(fs, player);
                    players.Add(player);
                };

                
            }

            
            
            idCount = players.Count;

            while (true)
            {
                Console.Clear();
                Print("Choose your player:", 60);

                foreach (Player p in players)
                {
                    Console.WriteLine($"{p.id}: {p.name}");
                }

                Print("Please input player name or id (id:# or playername).");
                Print("To Start a New Game type create");
                string[] data = Console.ReadLine().ToLower().Split(':');

                try
                {
                    if (data[0] == "id")
                    {
                        if (int.TryParse(data[1], out int id))
                        {
                            foreach (Player player in players)
                            {
                                if(player.id == id)
                                {
                                    return player;
                                }
                            }
                            Console.WriteLine("There is no player with that id!");
                            Console.ReadKey();
                        }
                        else
                        {
                            Console.WriteLine("Your id needs to be a number! Press any key to continue!");
                            Console.ReadKey();
                        }
                    }
                    else if (data[0] == "create")
                    {
                        Player newPlayer = NewStart(idCount);
                        newP = true;
                        return newPlayer;
                        
                    }
                    else
                    {
                        foreach (Player player in players)
                        {
                            Console.WriteLine($"Player name: {player.name.ToLower()} | Selected name: {data[0]}");
                            
                            if(player.name.ToLower() == data[0].ToLower())
                            {
                                return player;
                            }
                                                        
                        }
                        Console.WriteLine("There is no player with that name!");
                        Console.ReadKey();
                    }
                    
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine("Your id needs to be a number! Press any key to continue!");
                    Console.ReadKey();  
                }
            }
            
        }

        public static void Print(string text, int speed = 10)
        {
            foreach (char c in text)
            {
                Console.Write(c);
                Thread.Sleep(speed);
            }
            Console.WriteLine("");
        }
    }
}
