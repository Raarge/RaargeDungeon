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
            Console.WriteLine("  E(d)it Monster");
            Console.WriteLine("  (E)xit");
            Console.WriteLine("==================================");


            Console.WriteLine(" ");

            Console.WriteLine("Choice: ");

            input = Console.ReadLine().ToLower();

            while (input != "c" && input != "e" && input != "d" && input.Length != 1)
            {
                Console.WriteLine("Enter a valid value");
                input = Console.ReadLine();
            }

            if (input == "c")
                AddNewMonsters();
            if (input == "d")
                EditMonster(monsters);
            if (input == "e")
                Environment.Exit(0);


        }

        public static void EditMonster(List<Monster> monsters)
        {
            int input = 0;
            int newValue = 0;
            Boolean nvalue = false;
            Monster mSelected = new Monster();

            mSelected = ChooseMonsterToEdit(monsters);

            Console.Clear();
            Console.WriteLine(" ");
            Console.WriteLine("          Monster Editor ");
            Console.WriteLine("======================================");
            Console.WriteLine("      Pick an Attribute to Edit");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine($"       Editing {mSelected.name}");
            Console.WriteLine("---------------------------------------");
            Console.WriteLine($" 1. XP Given: {mSelected.xpGiven} ");
            Console.WriteLine($" 2. Damage Dice Type: {mSelected.attackDice}");
            Console.WriteLine($" 3. Number of Damage Dice: {mSelected.numberAttackDice}");
            Console.WriteLine($" 4. Damage Modifier: {mSelected.attackDiceModifier}");
            Console.WriteLine($" 5. Number of Attacks: {mSelected.numberAttacks}");
            Console.WriteLine($" 6. Armor Class: {mSelected.armorclass}");
            Console.WriteLine($" 7. Damage Resist 1 No 2 Yes: {mSelected.damageResist}");
            Console.WriteLine($" 8. Hit Point Dice: {mSelected.hitDice}");
            Console.WriteLine($" 9. Number of Hit Point Dice: {mSelected.numberHitDie}");
            Console.WriteLine($" 10. Hit Point Dice Modifier: {mSelected.hitDiceModifier}");
            Console.WriteLine($" 11. Is Monster Undead: true or false: { mSelected.isUndead}" );
            Console.WriteLine("=========================================");
            Console.WriteLine(" ");
            Console.WriteLine(" Which attribute do you wish to edit? (enter number)");
            input = Convert.ToInt32(Console.ReadLine());

            while (input < 1 && input > 11)
            {
                Console.WriteLine("You did not choose a valid option. choose again ");
                input = Convert.ToInt32(Console.ReadLine());
            }

            Console.Write("Enter new value: ");
            if (input >= 1 && input <= 10)
                newValue = Convert.ToInt32(Console.ReadLine());
            else if (input == 11)
                nvalue = Convert.ToBoolean(Console.ReadLine());

            if (input == 1)
                mSelected.xpGiven = newValue;
            else if (input == 2)
                mSelected.attackDice = newValue;
            else if (input == 3)
                mSelected.numberAttackDice = newValue;
            else if (input == 4)
                mSelected.attackDiceModifier = newValue;
            else if (input == 5)
                mSelected.numberAttacks = newValue;
            else if (input == 6)
                mSelected.armorclass = newValue;
            else if (input == 7)
                mSelected.damageResist = newValue;
            else if (input == 8)
                mSelected.hitDice = newValue;
            else if (input == 9)
                mSelected.numberHitDie = newValue;
            else if (input == 10)
                mSelected.hitDiceModifier = newValue;
            else if (input == 11)
                mSelected.isUndead = nvalue;

            Console.WriteLine(" ");
            Console.WriteLine("Hit a key to save");
            Console.ReadKey();

            SaveMonster(mSelected);


        }

        public static Monster ChooseMonsterToEdit(List<Monster> monsters)
        {
            string input = "";
            int counter = 0;
            bool inList = false;
            Monster m = new Monster();

            Console.Clear();
            Console.WriteLine("   List of All Monsters");
            Console.WriteLine("===============================");
            foreach (Monster monster in monsters)
            {
                counter++;
                if (counter == 4)
                {
                    Console.Write("\n");
                    counter = 0;
                }
                
                Console.Write($" {monster.name} ");
            }
            Console.WriteLine("=================================");
            Console.WriteLine("   Pick a Monster to Edit");
            Console.WriteLine(" ");
            Console.Write("Which Monster will you edit? ");
            input = Console.ReadLine();

            while (!inList)
            {
                foreach (Monster monster in monsters)
                {
                    if (input == monster.name)
                    {
                        m = monster;
                        inList = true;
                        break;
                    }
                        
                }
                if (!inList)
                {
                    Console.WriteLine("You did not pick a monster from the list.  Enter a monster name ");
                    input = Console.ReadLine();
                }
                
            }

            return m;

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
            Console.Write("Monster Is Undead? true or false: ");
            m.isUndead = Convert.ToBoolean(Console.ReadLine());
            

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
            Console.WriteLine($" Is Monster Undead? {m.isUndead}");
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
