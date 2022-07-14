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
               

        public enum monsterRace { Orc, Goblin, Ogre, Madman, Kobold, Wolf, Imp, Spider, Skeleton, Sprite, Zombie }
        public monsterRace race = monsterRace.Sprite;

        public static string GetMonsterName()
        {
            string name = "";

            string[] raceList = Enum.GetValues(typeof(monsterRace)).Cast<string>().ToArray();

            name = raceList[Randomizer.GetRandomNumber(0, raceList.Length - 1)];

            return name;
        }
        
        public static Monster GetCombatStats(Monster m)
        {
            switch (m.name.ToString())
            {
                case "Orc":
                    m.attackDice = 6;
                    m.armorclass = 13;
                    m.damageResist = 1;
                    m.hitDice = 8;
                    m.numberHitDie = 2;
                    break;
                case "Goblin":
                    m.attackDice = 6;
                    m.armorclass = 15;
                    m.damageResist = 1;
                    m.hitDice = 6;
                    m.numberHitDie = 2;
                    break;
                case "Ogre":
                    m.attackDice = 8;
                    m.numberAttackDice = 2;
                    m.armorclass = 11;
                    m.damageResist = 1;
                    m.hitDice = 10;
                    m.numberHitDie = 7;
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
                    break;
                default:
                    m.attackDice = 6;
                    m.armorclass = 12;
                    m.damageResist = 1;
                    m.hitDice = 10;
                    break;
            }

            return m;

        }
    }
}

