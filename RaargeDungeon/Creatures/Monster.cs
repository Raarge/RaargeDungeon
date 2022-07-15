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
               

        public enum monsterRace { Orc, Goblin, Ogre, Madman, Kobold, Wolf, Imp, Spider, Skeleton, Sprite, Zombie }
        public monsterRace race = monsterRace.Sprite;

        public static string GetMonsterName()
        {
            string name = "";

            string[] mRaces = Enum.GetNames(typeof(monsterRace));
            
            int indexer = Randomizer.GetRandomNumber(mRaces.Length - 1,0);

            name = mRaces[indexer];

            return name;
        }
        
        public static Monster SpawnMonster(Monster mstr, bool random, int plrLevel)
        {
            mstr.strength = Randomizer.GetPlayerStats();
            mstr.constitution = Randomizer.GetPlayerStats();
            mstr.dexterity = Randomizer.GetPlayerStats();
            mstr.intelligence = Randomizer.GetPlayerStats();
            mstr.wisdom = Randomizer.GetPlayerStats();
            mstr.charisma = Randomizer.GetPlayerStats();
            mstr.level = Randomizer.GetRandomNumber(plrLevel + 2);

            if (random)
            {
                //Console.WriteLine("Made it to Combat");
                mstr.name = Monster.GetMonsterName();
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
                case "Goblin":
                    m.attackDice = 6;
                    m.armorclass = 15;
                    m.damageResist = 1;
                    m.hitDice = 6;
                    m.numberHitDie = 2;
                    break;
                case "Ogre":
                    m.attackDice = 12;
                    m.numberAttackDice = 1;
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

