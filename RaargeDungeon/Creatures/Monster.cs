using System;
using System.Collections.Generic;
using RaargeDungeon.Items;
using RaargeDungeon.Helpers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaargeDungeon.Creatures
{
    public class Monster : BaseCreature
    { 

        public List<SpellScroll> spells = new List<SpellScroll>();

        // Need to build
        public int coins { get; set; }
        public int level { get; set; }
        public int xpGiven { get; set; }
        public int health { get; set; }
        public int baseHealth { get; set; }
        public int energy { get; set; }
        public int baseEnergy { get; set; }
        public int damageResist { get; set; }
        public int attackDice { get; set; }
        public int numberAttackDice = 1;
        public int armorclass { get; set; }
        public int hitDice { get; set; }
        public int numberHitDie = 1;
        public bool IsAlive { get; set; }
               

        public enum monsterRace { Orc, Goblin, Ogre, Madman, Kobold, Wolf, Imp, Spider, Skeleton, Sprite, Zombie, Merfolk, Monodrone, TwigBlight, Mastiff, Snake, Blight,
        Bullywug, WildGnoll, Svirfneblin, WarGoblin, DeepGnome, DustMephit, GasSpore, Gnoll, Hobgoblin, Worg, Bugbear, DireWolf, Dryad, Ghoul, HalfOgre, Spectre, Centaur,
        Gargoyle, Griffon, SpinedDevil, Wererat, Basilisk, BugbearChief, Grell, HellHound, Manticore, Minotaur, Mummy, Owlbear, Yeti}
        public monsterRace race = monsterRace.Sprite;

        
        public static string GetMonsterName()
        {
            string name = "";

            string[] mRaces = Enum.GetNames(typeof(monsterRace));
            
            int indexer = Randomizer.GetRandomNumber(mRaces.Length - 1,0);

            name = mRaces[indexer];

            return name;
        }
        
        public static Monster SpawnMonster(Monster mstr, bool random, int plrLevel, int eclChosen)
        {
            mstr.strength = Randomizer.GetPlayerStats();
            mstr.constitution = Randomizer.GetPlayerStats();
            mstr.dexterity = Randomizer.GetPlayerStats();
            mstr.intelligence = Randomizer.GetPlayerStats();
            mstr.wisdom = Randomizer.GetPlayerStats();
            mstr.charisma = Randomizer.GetPlayerStats();
            mstr.level = Randomizer.GetRandomNumber(plrLevel + 2);

            // build list of acceptable monsters
            mstr = FinishMonsterBuildOut(mstr, eclChosen);

            if (random)
            {
                //Console.WriteLine("Made it to Combat");
                mstr.race = (monsterRace)Enum.Parse(typeof(monsterRace), mstr.name);
                mstr = Monster.GetCombatStats(mstr);
                mstr.baseHealth = Randomizer.GetHealth(mstr.hitDice, mstr.hitDice, Monster.GetModifier(mstr.constitution));
                mstr.health = mstr.baseHealth;

            }
            else
            {


                mstr.name = "Evil Human Rogue";
                mstr = Monster.GetCombatStats(mstr);
                mstr.baseHealth = Randomizer.GetHealth(mstr.hitDice, mstr.numberHitDie, Monster.GetModifier(mstr.constitution));
                mstr.health = mstr.baseHealth;

            }
            mstr.spellDcCheck = Monster.GetSpellDcCheck(mstr.intelligence, mstr.level);
            mstr.IsAlive = true;

            return mstr;
        }

        public static Monster FinishMonsterBuildOut(Monster mstr, int eclChosen)
        {
            // get a initial list of monsters by xp level
            List<Monster> mstrList = new List<Monster>();

            mstrList.Add(new Monster() { name = "Orc", xpGiven = 100 });
            mstrList.Add(new Monster() { name = "Goblin", xpGiven = 50 });
            mstrList.Add(new Monster() { name = "Ogre", xpGiven = 450 });
            mstrList.Add(new Monster() { name = "Madman", xpGiven = 50 });
            mstrList.Add(new Monster() { name = "Kobold", xpGiven = 25 });
            mstrList.Add(new Monster() { name = "Wolf", xpGiven = 50 });
            mstrList.Add(new Monster() { name = "Imp", xpGiven = 200 });
            mstrList.Add(new Monster() { name = "Spider", xpGiven = 200 });
            mstrList.Add(new Monster() { name = "Skeleton", xpGiven = 50 });
            mstrList.Add(new Monster() { name = "Sprite", xpGiven = 50 });
            mstrList.Add(new Monster() { name = "Zombie", xpGiven = 50 });
            mstrList.Add(new Monster() { name = "Merfolk", xpGiven = 25 });
            mstrList.Add(new Monster() { name = "Monodrone", xpGiven = 25 });
            mstrList.Add(new Monster() { name = "TwigBlight", xpGiven = 25 });
            mstrList.Add(new Monster() { name = "Mastiff", xpGiven = 25 });
            mstrList.Add(new Monster() { name = "Snake", xpGiven = 25 });
            mstrList.Add(new Monster() { name = "Blight", xpGiven = 75 });
            mstrList.Add(new Monster() { name = "Bullywug", xpGiven = 75 });
            mstrList.Add(new Monster() { name = "WildGnoll", xpGiven = 75 });
            mstrList.Add(new Monster() { name = "Svirfneblin", xpGiven = 75 });
            mstrList.Add(new Monster() { name = "WarGoblin", xpGiven = 75 });
            mstrList.Add(new Monster() { name = "DeepGnome", xpGiven = 100 });
            mstrList.Add(new Monster() { name = "DustMephit", xpGiven = 100 });
            mstrList.Add(new Monster() { name = "GasSpore", xpGiven = 100 });
            mstrList.Add(new Monster() { name = "Gnoll", xpGiven = 100 });
            mstrList.Add(new Monster() { name = "Hobgoblin", xpGiven = 100 });
            mstrList.Add(new Monster() { name = "Worg", xpGiven = 100 });
            mstrList.Add(new Monster() { name = "Bugbear", xpGiven = 200 });
            mstrList.Add(new Monster() { name = "DireWolf", xpGiven = 200 });
            mstrList.Add(new Monster() { name = "Dryad", xpGiven = 200 });
            mstrList.Add(new Monster() { name = "Ghoul", xpGiven = 200 });
            mstrList.Add(new Monster() { name = "HalfOgre", xpGiven = 200 });
            mstrList.Add(new Monster() { name = "Spectre", xpGiven = 200 });
            mstrList.Add(new Monster() { name = "Centaur", xpGiven = 450 });
            mstrList.Add(new Monster() { name = "Gargoyle", xpGiven = 450 });
            mstrList.Add(new Monster() { name = "Griffon", xpGiven = 450 });
            mstrList.Add(new Monster() { name = "SpinedDevil", xpGiven = 450 });
            mstrList.Add(new Monster() { name = "Wererat", xpGiven = 450 });
            mstrList.Add(new Monster() { name = "Basilisk", xpGiven = 700 });
            mstrList.Add(new Monster() { name = "BugbearChief", xpGiven = 700 });
            mstrList.Add(new Monster() { name = "Grell", xpGiven = 700 });
            mstrList.Add(new Monster() { name = "HellHound", xpGiven = 700 });
            mstrList.Add(new Monster() { name = "Manticore", xpGiven = 700 });
            mstrList.Add(new Monster() { name = "Minotaur", xpGiven = 700 });
            mstrList.Add(new Monster() { name = "Mummy", xpGiven = 700 });
            mstrList.Add(new Monster() { name = "Owlbear", xpGiven = 700 });
            mstrList.Add(new Monster() { name = "Yeti", xpGiven = 700 });

            //Add more monsters after level 2

            mstrList = GetEncounterLevelChosenList(mstrList, eclChosen);

            // pick monster from list
            mstr = GetMonsterFromList(mstrList);

            // return monster
            return mstr;
        }

        public static Monster GetMonsterFromList(List<Monster> mList)
        {
            Monster[] monsters = new Monster[mList.Count];

            monsters = mList.ToArray();

            int pick = Randomizer.GetRandomNumber(monsters.Length) - 1;

            return monsters[pick];

            

        }

        public static List<Monster> GetEncounterLevelChosenList(List<Monster> mstrList, int eclChosen)
        {
            List<Monster> FullListMonster = new List<Monster>();

            foreach (Monster m in mstrList)
            {
                if(eclChosen == m.xpGiven)
                    FullListMonster.Add(m);

            }
            
            return FullListMonster;
        }

        public static Monster GetCombatStats(Monster m)
        {
            // Fix to randomize this choice

            switch (m.name.ToString())
            {
                case "Orc":
                    m.attackDice = 6;
                    m.armorclass = 13;
                    m.damageResist = 1;
                    m.hitDice = 8;
                    m.numberHitDie = 1;
                    break;
                case "Svirfneblin":
                    m.attackDice = 8;
                    m.armorclass = 15;
                    m.damageResist = 1;
                    m.hitDice = 4;
                    m.numberHitDie = 3;
                    break;
                case "DeepGnome":
                    m.attackDice = 8;
                    m.armorclass = 15;
                    m.damageResist = 1;
                    m.hitDice = 6;
                    m.numberHitDie = 3;
                    break;
                case "DustMephit":
                    m.attackDice = 4;
                    m.armorclass = 12;
                    m.damageResist = 1;
                    m.hitDice = 6;
                    m.numberHitDie = 5;
                    break;
                case "Worg":
                    m.attackDice = 6;
                    m.numberAttackDice = 2;
                    m.armorclass = 13;
                    m.damageResist = 1;
                    m.hitDice = 10;
                    m.numberHitDie = 4;
                    break;
                case "Gnoll":
                    m.attackDice = 8;
                    m.armorclass = 15;
                    m.damageResist = 1;
                    m.hitDice = 8;
                    m.numberHitDie = 5;
                    break;
                case "GasSpore":
                    m.attackDice = 8;
                    m.armorclass = 5;
                    m.damageResist = 1;
                    m.hitDice = 10;
                    m.numberHitDie = 1;
                    break;
                case "Goblin":
                    m.attackDice = 6;
                    m.armorclass = 15;
                    m.damageResist = 1;
                    m.hitDice = 6;
                    m.numberHitDie = 2;
                    break;
                case "Hobgoblin":
                    m.attackDice = 6;
                    m.numberAttackDice = 2;
                    m.armorclass = 18;
                    m.damageResist = 1;
                    m.hitDice = 8;
                    m.numberHitDie = 2;
                    break;
                case "WarGoblin":
                    m.attackDice = 4;
                    m.numberAttackDice = 2;
                    m.armorclass = 16;
                    m.damageResist = 1;
                    m.hitDice = 6;
                    m.numberHitDie = 3;
                    break;
                case "WildGnoll":
                    m.attackDice = 8;
                    m.armorclass = 14;
                    m.damageResist = 1;
                    m.hitDice = 8;
                    m.numberHitDie = 3;
                    break;
                case "Ogre":
                    m.attackDice = 12;
                    m.numberAttackDice = 2;
                    m.armorclass = 11;
                    m.damageResist = 1;
                    m.hitDice = 10;
                    m.numberHitDie = 7;
                    break;
                case "TwigBlight":
                    m.attackDice = 4;
                    m.armorclass = 13;
                    m.damageResist = 1;
                    m.hitDice = 6;
                    break;
                case "Mastiff":
                    m.attackDice = 6;
                    m.armorclass = 12;
                    m.damageResist = 1;
                    m.hitDice = 8;
                    break;
                case "Snake":
                    m.attackDice = 4;
                    m.numberAttackDice = 2;
                    m.armorclass = 13;
                    m.damageResist = 1;
                    m.hitDice = 4;
                    break;
                case "Blight":
                    m.attackDice = 4;
                    m.numberAttackDice = 2;
                    m.armorclass = 12;
                    m.damageResist = 1;
                    m.hitDice = 8;
                    m.numberHitDie = 3;
                    break;
                case "Bullywig":
                    m.attackDice = 8;
                    m.armorclass = 12;
                    m.damageResist = 1;
                    m.hitDice = 8;
                    break;
                case "Merfolk":
                    m.attackDice = 6;
                    m.armorclass = 11;
                    m.damageResist = 1;
                    m.hitDice = 8;
                    m.numberHitDie = 2;
                    break;
                case "Monodrone":
                    m.attackDice = 4;
                    m.armorclass = 15;
                    m.damageResist = 1;
                    m.hitDice = 8;
                    break;
                case "Madman":
                    m.attackDice = 6;
                    m.armorclass = 12;
                    m.damageResist = 1;
                    m.hitDice = 8;
                    break;
                case "Kobold":
                    m.attackDice = 4;
                    m.armorclass = 12;
                    m.damageResist = 1;
                    m.hitDice = 6;
                    m.numberHitDie = 2;
                    break;
                case "Wolf":
                    m.attackDice = 4;
                    m.numberAttackDice = 2;
                    m.armorclass = 13;
                    m.damageResist = 1;
                    m.hitDice = 8;
                    m.numberHitDie = 2;
                    break;
                case "Imp":
                    m.attackDice = 4;
                    m.armorclass = 13;
                    m.damageResist = 2;
                    m.hitDice = 4;
                    m.numberHitDie = 3;
                    break;
                case "Spider":
                    m.attackDice = 8;
                    m.armorclass = 14;
                    m.damageResist = 1;
                    m.hitDice = 10;
                    m.numberHitDie = 4;
                    break;
                case "Skeleton":
                    m.attackDice = 6;
                    m.armorclass = 13;
                    m.damageResist = 1;
                    m.hitDice = 8;
                    m.numberHitDie = 2;
                    
                    break;
                case "Sprite":
                    m.attackDice = 4;
                    m.armorclass = 15;
                    m.damageResist = 1;
                    m.hitDice = 4;
                    
                    break;
                case "Zombie":
                    m.attackDice = 6;
                    m.armorclass = 8;
                    m.damageResist = 1;
                    m.hitDice = 8;
                    m.numberHitDie = 3;
                    m.xpGiven = 50;
                    break;
                case "Bugbear":
                    m.attackDice = 8;
                    m.numberAttackDice = 2;
                    m.armorclass = 16;
                    m.damageResist = 1;
                    m.hitDice = 8;
                    m.numberHitDie = 5;
                    break;
                case "DireWolf":
                    m.attackDice = 6;
                    m.numberAttackDice = 2;
                    m.armorclass = 14;
                    m.damageResist = 1;
                    m.hitDice = 10;
                    m.numberHitDie = 5;
                    break;
                case "Dryad":
                    m.attackDice = 8;
                    m.numberAttackDice = 1;
                    m.armorclass = 11;
                    m.damageResist = 2;
                    m.hitDice = 8;
                    m.numberHitDie = 5;
                    break;
                case "Ghoul":
                    m.attackDice = 6;
                    m.numberAttackDice = 2;
                    m.armorclass = 12;
                    m.damageResist = 1;
                    m.hitDice = 8;
                    m.numberHitDie = 5;
                    break;
                case "HalfOgre":
                    m.attackDice = 6;
                    m.numberAttackDice = 3;
                    m.armorclass = 18;
                    m.damageResist = 1;
                    m.hitDice = 10;
                    m.numberHitDie = 7;
                    break;
                case "Specter":
                    m.attackDice = 6;
                    m.numberAttackDice = 3;
                    m.armorclass = 12;
                    m.damageResist = 2;
                    m.hitDice = 8;
                    m.numberHitDie = 5;
                    break;
                case "Centaur":
                    m.attackDice = 6;
                    m.numberAttackDice = 3;
                    m.armorclass = 12;
                    m.damageResist = 1;
                    m.hitDice = 10;
                    m.numberHitDie = 6;
                    break;
                case "Gargoyle":
                    m.attackDice = 6;
                    m.numberAttackDice = 2;
                    m.armorclass = 15;
                    m.damageResist = 2;
                    m.hitDice = 8;
                    m.numberHitDie = 8;
                    break;
                case "Griffon":
                    m.attackDice = 6;
                    m.numberAttackDice = 3;
                    m.armorclass = 12;
                    m.damageResist = 1;
                    m.hitDice = 10;
                    m.numberHitDie = 8;
                    break;
                case "SpinedDevil":
                    m.attackDice = 6;
                    m.numberAttackDice = 2;
                    m.armorclass = 13;
                    m.damageResist = 1;
                    m.hitDice = 6;
                    m.numberHitDie = 5;
                    break;
                case "Wererat":
                    m.attackDice = 4;
                    m.numberAttackDice = 2;
                    m.armorclass = 12;
                    m.damageResist = 1;
                    m.hitDice = 8;
                    m.numberHitDie = 6;
                    break;
                case "Basilisk":
                    m.attackDice = 6;
                    m.numberAttackDice = 2;
                    m.armorclass = 15;
                    m.damageResist = 1;
                    m.hitDice = 8;
                    m.numberHitDie = 9;
                    break;
                case "BugbearChief":
                    m.attackDice = 8;
                    m.numberAttackDice = 3;
                    m.armorclass = 17;
                    m.damageResist = 1;
                    m.hitDice = 8;
                    m.numberHitDie = 11;
                    break;
                case "Grell":
                    m.attackDice = 10;
                    m.numberAttackDice = 2;
                    m.armorclass = 12;
                    m.damageResist = 1;
                    m.hitDice = 8;
                    m.numberHitDie = 10;
                    break;
                case "HellHound":
                    m.attackDice = 8;
                    m.numberAttackDice = 2;
                    m.armorclass = 15;
                    m.damageResist = 1;
                    m.hitDice = 8;
                    m.numberHitDie = 8;
                    break;
                case "Manticore":
                    m.attackDice = 8;
                    m.numberAttackDice = 3;
                    m.armorclass = 14;
                    m.damageResist = 1;
                    m.hitDice = 10;
                    m.numberHitDie = 9;
                    break;
                case "Minotaur":
                    m.attackDice = 12;
                    m.numberAttackDice = 2;
                    m.armorclass = 14;
                    m.damageResist = 1;
                    m.hitDice = 10;
                    m.numberHitDie = 10;
                    break;
                case "Mummy":
                    m.attackDice = 6;
                    m.numberAttackDice = 2;
                    m.armorclass = 11;
                    m.damageResist = 2;
                    m.hitDice = 8;
                    m.numberHitDie = 10;
                    break;
                case "Owlbear":
                    m.attackDice = 10;
                    m.numberAttackDice = 3;
                    m.armorclass = 13;
                    m.damageResist = 1;
                    m.hitDice = 10;
                    m.numberHitDie = 8;
                    break;
                case "Yeti":
                    m.attackDice = 6;
                    m.numberAttackDice = 2;
                    m.armorclass = 12;
                    m.damageResist = 1;
                    m.hitDice = 10;
                    m.numberHitDie = 6;
                    break;
                default:
                    m.attackDice = 6;
                    m.armorclass = 12;
                    m.damageResist = 1;
                    m.hitDice = 10;
                    m.xpGiven = 25;
                    break;
            }

            return m;

        }
    }
}

