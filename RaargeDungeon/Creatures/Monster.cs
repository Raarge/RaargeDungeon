using System;
using System.Collections.Generic;
using RaargeDungeon.Items;
using RaargeDungeon.Helpers;
using RaargeDungeon.Builder;
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
        public int numberAttackDice { get; set; } = 1;
        public int attackDiceModifier { get; set; }
        public int numberAttacks { get; set;} = 1;
        public int armorclass { get; set; }
        public int hitDice { get; set; }
        public int numberHitDie { get; set; } = 1;
        public int hitDiceModifier { get; set; }
        public int toHitBonus { get; set; } = 2;
        public bool IsAlive { get; set; }
        public bool IsStunned { get; set; } = false;
        public bool isUndead { get; set; } = false;   
        public string race { get; set; }
        public int lastDamageTaken { get; set; }

        
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
                mstr.race = mstr.name;
                mstr.baseHealth = Randomizer.GetMonsterHealth(mstr.hitDice, mstr.hitDice, mstr.hitDiceModifier);
                mstr.health = mstr.baseHealth;

            }
            else
            {


                mstr.name = "Evil Human Rogue";
                mstr.xpGiven = 25;
                mstr.attackDice = 6;
                mstr.attackDiceModifier = 0;
                mstr.numberAttackDice = 1;
                mstr.toHitBonus = 2;
                mstr.damageResist = 1;
                mstr.hitDice = 4;
                mstr.numberHitDie = 1;
                mstr.hitDiceModifier = 1;
                mstr.numberAttacks = 1;
                mstr.isUndead = false;
                mstr.baseHealth = Randomizer.GetMonsterHealth(mstr.hitDice, mstr.numberHitDie, mstr.hitDiceModifier);
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

            mstrList = MonsterBuilder.LoadMonsters(mstrList);

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

        public static int GetMonsterSavingThrowModifier(Monster m, string typeSave)
        {
            int statScore = 0;
            int saveModifier = 0;

            if (typeSave == "strength")
                statScore = m.strength;
            else if (typeSave == "constitution")
                statScore = m.constitution;
            else if (typeSave == "dexterity")
                statScore = m.dexterity;
            else if (typeSave == "intelligence")
                statScore = m.intelligence;
            else if (typeSave == "wisdom")
                statScore = m.wisdom;
            else if (typeSave == "charisma")
                statScore = m.charisma;


            saveModifier = GetModifier(statScore);

            return saveModifier;
        }
    }
}

