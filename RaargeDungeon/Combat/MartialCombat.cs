using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaargeDungeon.Helpers;
using RaargeDungeon.Creatures;
using System.Collections;

namespace RaargeDungeon.Combat
{
    public class MartialCombat
    {
        public static Combatants DoAttack(Player p, Monster m, string typeAttack, string action)
        {
            Combatants combatants = new Combatants();
            HitChecks mstrHit = new HitChecks();
            HitChecks plyrHit = new HitChecks();
            string plryAtkType = typeAttack;
            string mstrAtkType = GetMonsterAttackType(m);

            //determine initiative
            p.currentInitiative = GetInitiativeRoll(p.dexterity);
            m.currentInitiative = GetInitiativeRoll(m.dexterity);

            //determine combat order
            if (p.currentInitiative >= m.currentInitiative)
            {
                p.currentCombatOrder = 1;
                m.currentCombatOrder = 2;
            }
            else
            {
                m.currentCombatOrder = 1;
                p.currentCombatOrder = 2;
            }

            // extract below here into another method
            //****************************************

            // Check Option Taken


            //check hits first 
            plyrHit = PlyrHitTry(p, m, plyrHit, plryAtkType);
            mstrHit = MstrHitTry(p, m, mstrHit, mstrAtkType);

            bool monsterCrit = false;
            bool playerCrit = false;
            int plyrDamage = 0;
            int mstrDamage = 0;


            //rewrite line below ******************************************************
            //damage = Randomizer.GetRandomDieRoll(mstr.wisdom, mstr.numberAttackDice);

            if (plyrHit.AttackHits)
            {
                if (plyrHit.ToHitRoll == 20)
                    playerCrit = true;

                if (plryAtkType == "melee")
                {
                    if (p.currentClass == Player.PlayerClass.Monk || p.currentClass == Player.PlayerClass.Rogue)
                        plyrDamage = Randomizer.GetRandomDieRoll(p.attackDie, p.numberAttackDie, BaseCreature.GetModifier(p.dexterity)) + (p.weaponValue / 2);
                    else
                        plyrDamage = Randomizer.GetRandomDieRoll(p.attackDie, p.numberAttackDie, BaseCreature.GetModifier(p.strength)) + (p.weaponValue / 2);
                }
                else
                {
                    // add spell damage here 


                }
                if (playerCrit)
                    plyrDamage *= 2;

                if (plyrDamage < 0)
                    plyrDamage = 0;
            }
            if (mstrHit.AttackHits)
            {
                MonsterHits(m, mstrHit, mstrAtkType, ref monsterCrit, ref mstrDamage);
            }

            // add spell text in here
            if (plyrHit.AttackHits)
            {
                string leader = TextHelpers.GetAttackStart(plyrDamage);
                string style = TextHelpers.GetWeaponAttackStyle();

                UIHelpers.Print($"{leader}, {style} at {m.name}, who attacks in return.");
                if (playerCrit)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("You land a critical!");
                    Console.ResetColor();
                }
                Console.WriteLine($"You deal {plyrDamage} damage to {m.name}.");

            }
            else
                Console.WriteLine($"You swing your {TextHelpers.GetWeapon()} at a {m.name} but miss.");

            // add spell text in here
            if (mstrHit.AttackHits)
            {
                UIHelpers.Print($"A {m.name} growls and swings landing a blow to you.");
                TextHelpers.GetMonsterHitLine(m.name, mstrDamage, monsterCrit, action.ToLower());
            }
            else
                Console.WriteLine($"A {m.name} swings wildly at you but misses.");

            m.health -= plyrDamage;

            m.health -= plyrDamage;
            if (m.health <= 0)
            {

                m.IsAlive = false;
            }

            combatants.player = p;
            combatants.monster = m;

            return combatants;
        }

        public static void MonsterHits(Monster m, HitChecks mstrHit, string mstrAtkType, ref bool monsterCrit, ref int mstrDamage)
        {

            if (mstrHit.ToHitRoll == 20)
                monsterCrit = true;

            if (mstrAtkType == "melee")
            {
                mstrDamage = Randomizer.GetRandomDieRoll(m.attackDice, m.numberAttackDice, BaseCreature.GetModifier(m.strength));
            }
            else
            {
                // add spell damage here

            }

            if (monsterCrit)
                mstrDamage *= 2;

            if (mstrDamage < 0)
                mstrDamage = 0;

        }

        public static HitChecks MstrHitTry(Player p, Monster m, HitChecks mstrHit, string mstrAtkType)
        {
            if (mstrAtkType == "melee" && m.currentCombatOrder != 0)
            {
                if (mstrAtkType == "melee")
                    mstrHit = CheckHit(p.armorclass, m.strength);
                else if (mstrAtkType == "spell")
                    mstrHit = CheckMagicHit(m.spellDcCheck, m.intelligence);
            }

            return mstrHit;
        }

        public static HitChecks PlyrHitTry(Player p, Monster m, HitChecks plyrHit, string plryAtkType)
        {
            if (plryAtkType == "melee" && p.currentInitiative != 0)
            {
                if (plryAtkType == "melee")
                {
                    plyrHit = CheckHit(m.armorclass, p.strength);
                }
                else if (plryAtkType == "spell")
                {
                    plyrHit = CheckMagicHit(p.spellDcCheck, m.intelligence);
                }
            }

            return plyrHit;
        }

        public static string GetMonsterAttackType(Monster m)
        {
            string type = "";

            if (m.spells.Count != 0)
                type = "melee";
            else
            {
                int randChk = Randomizer.GetRandomNumber(4);
                if (randChk == 3)
                    type = "spell";
                else
                    type = "melee";
            }

            return type;
        }

        public static HitChecks CheckHit(int opossingAC, int Str)
        {
            HitChecks hitCheck = new HitChecks();
            bool didHit;
            int atkRoll = Randomizer.GetRandomDieRoll(20) + BaseCreature.GetModifier(Str);

            if (atkRoll > opossingAC)
                didHit = true;
            else
                didHit = false;

            hitCheck.AttackHits = didHit;
            hitCheck.ToHitRoll = atkRoll;

            return hitCheck;
        }

        public static HitChecks CheckMagicHit(int dcCheck, int saveStat)
        {
            HitChecks hitCheck = new HitChecks();
            bool didHit;
            int spellSaveRoll = Randomizer.GetRandomDieRoll(20) + BaseCreature.GetModifier(saveStat);

            if (spellSaveRoll > dcCheck)
                didHit = true;
            else
                didHit = false;

            hitCheck.AttackHits = didHit;
            hitCheck.ToHitRoll = spellSaveRoll;

            return hitCheck;
        }

        public static int GetInitiativeRoll(int dexterity)
        {
            int roll = 0;
            int mod = BaseCreature.GetModifier(dexterity);
            roll = Randomizer.GetRandomDieRoll(20) + mod;

            return roll;
        }
    }
}
