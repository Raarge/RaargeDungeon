using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using RaargeDungeon.Creatures;

namespace RaargeDungeon.Builder
{
    public static class MonsterBuilder
    {
        public static void AdminInterface()
        {
            string input = "";
            List<Monster> monsters = new List<Monster>();

            monsters = LoadMonsters(monsters);

            Console.Clear();
            Console.WriteLine("         Admin Interfact ");
            Console.WriteLine("==================================");
            Console.WriteLine("  (C)reate New Monster");
            Console.WriteLine("  (E)xit");
            Console.WriteLine("==================================");
            Console.WriteLine(" ");
            Console.WriteLine("Choice: ");

            input = Console.ReadLine().ToLower();

            while (input != "c" && input != "e" && input.Length != 1)
            {
                Console.WriteLine("Enter a valid value");
                input = Console.ReadLine();
            }

            if (input == "c")
                AddNewMonsters();
            if (input == "e")
                Environment.Exit(0);


        }

        public static List<Monster> LoadMonsters(List<Monster> monsters)
        {
            string[] paths = Directory.GetFiles("monsters");

            FileStream fs;

            foreach (string p in paths)
            {
                using (fs = new FileStream(p, FileMode.Open, FileAccess.Read))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(Monster));
                    Monster m = (Monster)ser.Deserialize(fs);
                    monsters.Add(m);
                }
            }

            return monsters;
        }

        public static void AddNewMonsters()
        {
            string input = "";
            Monster m = new Monster();

            Console.Clear();
            Console.Write("Monster Name: ");
            m.name = Console.ReadLine();
            Console.Write("XP Given: ");
            m.xpGiven = Convert.ToInt32(Console.ReadLine());
            Console.Write("Damage Dice Type: ");
            m.attackDice = Convert.ToInt32(Console.ReadLine());
            Console.Write("Number of Damage Dice: ");
            m.numberAttackDice = Convert.ToInt32(Console.ReadLine());
            Console.Write("Damage Dice Modifier: ");
            m.attackDiceModifier = Convert.ToInt32(Console.ReadLine());
            Console.Write("To Hit Bonus: ");
            m.toHitBonus = Convert.ToInt32(Console.ReadLine());
            Console.Write("Armor Class: ");
            m.armorclass = Convert.ToInt32(Console.ReadLine());
            Console.Write("Does Monster have Damage Resist (1)No (2) yes: ");
            m.damageResist = Convert.ToInt32(Console.ReadLine());
            Console.Write("Hit Point Dice: ");
            m.hitDice = Convert.ToInt32(Console.ReadLine());
            Console.Write("Number of Hit Point Dice: ");
            m.numberHitDie = Convert.ToInt32(Console.ReadLine());
            Console.Write("Hit Point Dice Modifier: ");
            m.hitDiceModifier = Convert.ToInt32(Console.ReadLine());
            Console.Write("Monster Number of Attacks: ");
            m.numberAttacks = Convert.ToInt32(Console.ReadLine());
            

            Console.Clear();
            Console.WriteLine($"        {m.name}");
            Console.WriteLine("==================================");
            Console.WriteLine($" Monster gives: {m.xpGiven}");
            Console.WriteLine($" Damage Dice: d{m.attackDice}");
            Console.WriteLine($" Damage Dice Modifier: +{m.attackDiceModifier}");
            Console.WriteLine($" To Hit Roll Modifier: +{m.toHitBonus}");
            Console.WriteLine($" Monster Armor Class: {m.armorclass}");
            Console.WriteLine($" Damage Resit? {m.damageResist} If 1 then no, if 2 then yes");
            Console.WriteLine($" Hit Point Dice: d{m.hitDice}");
            Console.WriteLine($" Number of Hit Point Dice: {m.numberHitDie}");
            Console.WriteLine($" Hit Point Dice Modifier: +{m.hitDiceModifier}");
            Console.WriteLine($" Monster Number of Attacks: {m.numberAttacks}");
            Console.WriteLine($"");
            Console.WriteLine("===================================");
            Console.WriteLine(" ");
            Console.WriteLine("Save this monster?");
            input = Console.ReadLine().ToLower();

            while (input != "y" && input != "n" && input.Length != 1)
            {
                Console.WriteLine("You didn't enter a valid value. Please Enter (y)es or (n)o");
                input = Console.ReadLine().ToLower();
            }

            if (input == "y")
            {
                SaveMonster(m);
            }

        }

        public static void SaveMonster(Monster m)
        {

            string path = $"monsters/{m.name}.xml";
            string fileName = m.name;
            string[] paths = Directory.GetFiles("monsters");

            XmlSerializer serializer = new XmlSerializer(typeof(Monster));
            TextWriter writer = new StreamWriter(path);
            XmlSerializer sr = new XmlSerializer(typeof(Monster));
            serializer.Serialize(writer, m);
            writer.Close();

            AdminInterface();
        }
    }
}
