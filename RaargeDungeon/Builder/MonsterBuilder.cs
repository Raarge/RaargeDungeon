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
            int count25 = 0;
            int count50 = 0;
            int count75 = 0;
            int count100 = 0;
            int count200 = 0;
            int count450 = 0;
            int count700 = 0;
            int count1100 = 0;
            int count1800 = 0;
            int count2300 = 0;
            int count2900 = 0;
            int count3900 = 0;
            int count5000 = 0;
            int count5900 = 0;
            int count7200 = 0;
            int count8400 = 0;
            int issues = 0;
            List<Monster> monsters = new List<Monster>();
            List<Monster> mIssues = new List<Monster>();

            monsters = LoadMonsters(monsters);

            foreach (Monster m in monsters)
            {
                if (m.xpGiven == 25)
                    count25++;
                else if (m.xpGiven == 50)
                    count50++;
                else if (m.xpGiven == 75)
                    count75++;
                else if (m.xpGiven == 100)
                    count100++;
                else if (m.xpGiven == 200)
                    count200++;
                else if (m.xpGiven == 450)
                    count450++;
                else if (m.xpGiven == 700)
                    count700++;
                else if (m.xpGiven == 1100)
                    count1100++;
                else if (m.xpGiven == 1800)
                    count1800++;
                else if (m.xpGiven == 2300)
                    count2300++;
                else if (m.xpGiven == 2900)
                    count2900++;
                else if (m.xpGiven == 3900)
                    count3900++;
                else if (m.xpGiven == 5000)
                    count5000++;
                else if (m.xpGiven == 5900)
                    count5900++;
                else if (m.xpGiven == 7200)
                    count7200++;
                else if (m.xpGiven == 8400)
                    count8400++;
                else
                {
                    issues++;
                    mIssues.Add(m);
                }
            }

            Console.Clear();
            Console.WriteLine("         Admin Interface ");
            Console.WriteLine("====================================");
            Console.WriteLine("         Monster Counts");
            Console.WriteLine("------------------------------------");
            Console.WriteLine($"   25xp:   {count25} 50xp:   {count50}");
            Console.WriteLine($"   75xp:   {count75} 100xp:  {count100}");
            Console.WriteLine($"   200xp:  {count200} 450xp:  {count450}");
            Console.WriteLine($"   700xp:  {count700} 1100xp: {count1100}");
            Console.WriteLine($"   1800xp: {count1800} 2300xp: {count2300}");
            Console.WriteLine($"   2900xp: {count2900} 3900xp: {count3900}");
            Console.WriteLine($"   5000xp: {count5000} 5900xp: {count5900}");
            Console.WriteLine($"   7200xp: {count7200} 8400xp: {count8400}");
            Console.WriteLine($"===================================");
            Console.WriteLine("          Monster Issues");
            Console.WriteLine("------------------------------------");
            Console.WriteLine($"          Issue Count: {issues}");
            foreach (var issue in mIssues)
            {
                Console.WriteLine($"  Name: {issue.name} Xp: {issue.xpGiven}");
            }
            Console.WriteLine("====================================");
            Console.WriteLine("            Comands");
            Console.WriteLine("------------------------------------");
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
