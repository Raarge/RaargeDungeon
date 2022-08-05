using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaargeDungeon.Creatures;
using RaargeDungeon.Items;
using RaargeDungeon.Helpers;

namespace RaargeDungeon.Combat
{
    public class MagicalCombat
    {
        public static Combatants DoMagicalCombat (Monster m, Player p, SpellScroll spell)
        {
            Combatants combatants = new Combatants ();
            bool isSavePossible = spell.savingThrowAllowed;
            bool didCreatureSave = false;
            int creatureSaveMod;
            int creatureSaveThrow;
            bool halfOnSave = spell.halfDamageOnSave;
            string typeOfSave;
            int spellDC;
            int spellDamage = 0;

            for (int i = 1; i <= spell.numberAttacks; i++)
            {
                //check for save
                if (isSavePossible)
                {
                    // get the type of throw
                    typeOfSave = spell.typeSaveRequired;
                    // get monster saving throw
                    creatureSaveMod = Monster.GetMonsterSavingThrowModifier(m, typeOfSave);
                    // make roll for creature
                    creatureSaveThrow = Randomizer.GetRandomDieRoll(20, 1, creatureSaveMod);
                    // get the dc
                    spellDC = Player.GetSpellDifficultyModifier(p, typeOfSave, 0);
                    // check if saved
                    if (creatureSaveThrow >= spellDC)
                    {
                        didCreatureSave = true;
                    }
                    else
                        didCreatureSave = false;
                    
                }
                
                //calculate spell damage
                if (!isSavePossible || (isSavePossible && didCreatureSave && halfOnSave))
                {
                    if (p.currentClass == Player.PlayerClass.Wizard)
                    spellDamage = Randomizer.GetRandomDieRoll(spell.spellDamageDice, spell.spellNumberDamageDice, Player.GetModifier(p.intelligence)) + spell.spellDamageModifier; 
                    else if(p.currentClass == Player.PlayerClass.Cleric)
                    spellDamage = Randomizer.GetRandomDieRoll(spell.spellDamageDice, spell.spellNumberDamageDice, Player.GetModifier(p.wisdom)) + spell.spellDamageModifier;

                    if (didCreatureSave && halfOnSave)
                        spellDamage /= 2;
                }

                UIHelpers.Print($"You gesture casting {spell.Name}, ");

                if (!isSavePossible || !didCreatureSave)
                {
                    UIHelpers.Print(spell.FlavorText);
                    Console.WriteLine($"You deal {spellDamage} {spell.type} damage to the {m.name}.");
                }
                else if (didCreatureSave && halfOnSave)
                {
                    UIHelpers.Print(spell.FlavorText);
                    Console.WriteLine($"You deal {spellDamage} {spell.type} damage to the {m.name} who partially gets out of the way.");
                }
                else if (didCreatureSave && !halfOnSave)
                {
                    UIHelpers.Print($"However, a {m.name} manages to get out of the way of your spell.");
                }

                if (m.health < spellDamage)
                    spellDamage = m.health;

                m.health -= spellDamage;
                if (m.health <= 0)
                    m.IsAlive = false;

                spellDamage = 0;
            }

            combatants.player = p;
            combatants.monster = m;

            return combatants;
        }
        
    }
}
