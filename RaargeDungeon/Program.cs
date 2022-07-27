using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using RaargeDungeon.Helpers;
using System.Threading;
using RaargeDungeon.Creatures;
using RaargeDungeon.Encounter;

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
                Encounters.FirstEncounter(currentPlayer);
            }

            while (mainLoop)
            {
                Encounters.RandomEncounter(currentPlayer);
            }
        }

        #region NewCharacterStart
        static Player NewStart(int newId)
        {
            Player p = new Player();

            Console.Clear();
            UIHelpers.Print("Raarge's Dungeon");
            UIHelpers.Print("What is your characters name? ");
            p.name = Console.ReadLine();
            while (p.name == "" || p.name is null || p.name == " ")
            {
                Console.WriteLine("Surely you meant to put in your name?");
                Console.WriteLine("What is your characters name? ");
                p.name = Console.ReadLine();
            }

            UIHelpers.Print("Class: Wizard Ranger Fighter Rogue Cleric Monk");
            bool flag = false;
            while (flag == false)
            {
                flag = true;
                string input = Console.ReadLine().ToLower();
                if(input == "wizard")
                {
                    p.currentClass = Player.PlayerClass.Wizard;
                    p.manaType = Player.ManaType.Mana;
                }
                else if (input == "ranger")
                {
                    p.currentClass = Player.PlayerClass.Ranger;
                    p.manaType = Player.ManaType.Mana;
                }
                else if (input == "fighter")
                {
                    p.currentClass = Player.PlayerClass.Fighter;
                    p.manaType = Player.ManaType.Rage;
                }
                else if (input == "rogue")
                {
                    p.currentClass = Player.PlayerClass.Rogue;
                    p.manaType = Player.ManaType.Kri;
                }
                else if (input == "cleric")
                {
                    p.currentClass = Player.PlayerClass.Cleric;
                    p.manaType = Player.ManaType.Mana;
                }
                else if (input == "monk")
                {
                    p.currentClass = Player.PlayerClass.Monk;
                    p.manaType = Player.ManaType.Chi;
                }
                else
                {
                    Console.WriteLine("Invalid selection, please choose a valid class.");
                    flag = false;
                }
            }

            UIHelpers.Print("Race: Human, Elf, Dwarf, Gnome, Halfling, HalfOrc, Erudite");
            bool raceFlag = false;
            while (raceFlag == false)
            {
                raceFlag = true;
                string race = Console.ReadLine().ToLower();
                if (race == "human")
                    p.race = Player.Race.Human;
                else if (race == "elf")
                    p.race = Player.Race.Elf;
                else if (race == "dwarf")
                    p.race = Player.Race.Dwarf;
                else if (race == "gnome")
                    p.race = Player.Race.Gnome;
                else if (race == "halfling")
                    p.race = Player.Race.Halfling;
                else if (race == "halforc")
                    p.race = Player.Race.HalfOrc;
                else if (race == "erudite")
                    p.race = Player.Race.Erudite;
                else
                {
                    Console.WriteLine("You did not pick a valid race! Please try again.");
                    raceFlag = false;
                }
            }

            // Finish Player Buildout
            p = Player.BuildOutPlayer(p);
            
                            
            // Damage Resistance Setter
            if (p.currentClass == Player.PlayerClass.Fighter || p.race == Player.Race.Dwarf || p.race == Player.Race.Halfling ||
                p.race == Player.Race.Elf)
            {
                p.damageResit = 2;
            }
            else
                p.damageResit = 0;

            // Armor Setter
            if (p.currentClass == Player.PlayerClass.Cleric)
            {
                p.armorclass = Player.GetPlayerArmorClass(p) + Player.GetModifier(p.dexterity);
                p.armorclass += 2; //shield
            }
            if (p.currentClass == Player.PlayerClass.Fighter)
            {
                p.armorclass = Player.GetPlayerArmorClass(p) + 2; // +2 shield
            }
            if (p.currentClass == Player.PlayerClass.Wizard)
            {
                p.armorclass = Player.GetPlayerArmorClass(p) + Player.GetModifier(p.dexterity);
            }
            if (p.currentClass == Player.PlayerClass.Monk)
            {
                p.armorclass = Player.GetPlayerArmorClass(p) + Player.GetModifier(p.dexterity) + Player.GetModifier(p.wisdom);
            }
            if (p.currentClass == Player.PlayerClass.Ranger || p.currentClass == Player.PlayerClass.Rogue)
            {
                p.armorclass = Player.GetPlayerArmorClass(p) + Player.GetModifier(p.dexterity);
            }



            // Player ID Setter
            p.id = newId;

            
            Console.Clear();
            UIHelpers.Print("You awake on a stone slab in a dark room..........", 30);
            UIHelpers.Print($"Welcome {p.name}");
            Console.ReadKey();
            Console.Clear();
            UIHelpers.Print("You grope around in the dark trying to figure out where you are.  Your head is pounding");
            UIHelpers.Print("and you rub the back of your neck only to find a small incision.  You notice a small");
            UIHelpers.Print("strip of light along the floor and realize it is a door.  You move towards it and feel");
            UIHelpers.Print("for the door handle.  You turn the handle and meet with resistance which suddenly gives");
            UIHelpers.Print("way and you hear the door unlatch...");
            Console.ReadKey();
            Console.Clear();

            return p;
        }
        #endregion

        #region Quitting
        public static void Quit()
        {
            
            Save();
            Environment.Exit(0);
        }
        #endregion

        #region Saving
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
        #endregion

        #region Loading
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
                UIHelpers.Print("Choose your player:", 60);

                foreach (Player p in players)
                {
                    Console.WriteLine($"{p.id}: {p.name}");
                }

                UIHelpers.Print("Please input player name or id (id:# or playername).");
                UIHelpers.Print("To Start a New Game type create");
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
        #endregion

        #region HealthBar
        public static void HealthBar(string fillerChar, string backgroundChar, decimal value, int size)
        {
            int diff = (int)((value * (decimal)(size)));

            for(int i = 0; i < size; i++)
            {
                if (i < diff)
                    Console.Write(fillerChar);
                else
                    Console.Write(backgroundChar);
            }
        }
        #endregion

        #region Monster health bar
        public static void MonsterBar(string fillerChar, string backgroundChar, decimal value, int size)
        {
            int diff = (int)((value * (decimal)(size)));

            for (int i = 0; i < size; i++)
            {
                if (i < diff)
                    Console.Write(fillerChar);
                else
                    Console.Write(backgroundChar);
            }
        }
        #endregion
    }
}
