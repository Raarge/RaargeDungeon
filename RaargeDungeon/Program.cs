using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

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
            Console.WriteLine("Raarge's Dungeon");
            Console.WriteLine("What is your characters name? ");
            p.name = Console.ReadLine();
            p.id = newId;

            while(p.name == "" || p.name is null || p.name == " ")
            {
                Console.WriteLine("Surely you meant to put in your name?");
                Console.WriteLine("What is your characters name? ");
                p.name = Console.ReadLine();
            }
            Console.Clear();
            Console.WriteLine("You awake on a stone slab in a dark room.");
            Console.WriteLine($"Welcome {p.name}");
            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("You grope around in the dark trying to figure out where you are.  Your head is pounding");
            Console.WriteLine("and you rub the back of your neck only to find a small incision.  You notice a small");
            Console.WriteLine("strip of light along the floor and realize it is a door.  You move towards it and feel");
            Console.WriteLine("for the door handle.  You turn the handle and meet with resistance which suddenly gives");
            Console.WriteLine("way and you hear the door unlatch...");
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

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            XmlSerializer serializer = new XmlSerializer(typeof(Player));
            TextWriter writer = new StreamWriter(path);
            XmlSerializer sr = new XmlSerializer (typeof(Player));
            serializer.Serialize(writer, currentPlayer);
            writer.Close();
            //BinaryFormatter binForm = new BinaryFormatter();
            //FileStream file = File.Open(path, FileMode.OpenOrCreate);
            // research the replacement for binForm Serialize and replace this code
            
            //binForm.Serialize(file, currentPlayer);
            //file.Close();
        }

        public static Player Load(out bool newP)
        {
            newP = false;
            string[] paths = Directory.GetFiles("saves");
            List<Player> players = new List<Player>();
            int idCount = 0;

            //BinaryFormatter binForm = new BinaryFormatter();
            
            FileStream fs;

            foreach (string p in paths)
            {
                //FileStream file = File.Open(p, FileMode.Open);
                //Player player = (Player)binForm.Deserialize(file);
                //file.Close();

                using (fs = new FileStream(p, FileMode.Open))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(Player));
                    Player player;
                    player = (Player)ser.Deserialize(fs);
                    ser.Serialize(fs, player);
                    players.Add(player);
                };

                
            }

            
            
            idCount = players.Count;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Choose your player:");

                foreach (Player p in players)
                {
                    Console.WriteLine($"{p.id}: {p.name}");
                }

                Console.WriteLine("Please input player name or id (id:# or playername).");
                Console.WriteLine("To Start a New Game type create");
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
                            if(player.name == data[0])
                            {
                                return player;
                            }
                            Console.WriteLine("There is no player with that name!");
                            Console.ReadKey();
                        }
                    }
                    
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine("Your id needs t obe a number! Press any key to continue!");
                    Console.ReadKey();  
                }
            }
            
        }
    }
}
