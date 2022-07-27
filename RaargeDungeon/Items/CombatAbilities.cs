using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaargeDungeon.Creatures;

namespace RaargeDungeon.Items
{
    public class CombatAbilities : ScrollBase
    {
        public int abilityDamageDice { get; set; }
        public int abilityNumberDamageDice { get; set; }
        public int abilityToHitModifier { get; set; }
        public string abilityCombatType { get; set; }
        public int abilityNumberAttacks { get; set; }

        public static Player GetCombatAbilities(Player p, string combatAbility)
        {
            CombatAbilities combatAbil = new CombatAbilities();
            List<CombatAbilities> combAbilList = new List<CombatAbilities>();

            combAbilList = GetCombatAbilitiesList(p);

            foreach(var ability in combAbilList)
            {
                if(combatAbility == ability.Name)
                {
                    p.combatAbilities.Add(ability);
                }
            }

            return p;
        }

        private static List<CombatAbilities> GetCombatAbilitiesList(Player p)
        {
            List<CombatAbilities> list = new List<CombatAbilities>();

            CombatAbilities ExtraAttackI = new CombatAbilities();
            ExtraAttackI.Name = "Extra Attack I";
            ExtraAttackI.ShortName = "e1";
            ExtraAttackI.Description = "One extra standard melee attack";
            ExtraAttackI.RequiredLevel = 5;
            ExtraAttackI.SpellCost = 0;
            ExtraAttackI.CurrentSpellCost = 0;
            ExtraAttackI.MinCost = 0;
            ExtraAttackI.abilityDamageDice = p.attackDie;
            ExtraAttackI.abilityNumberDamageDice = p.numberAttackDie;
            ExtraAttackI.abilityNumberAttacks = 1;
            ExtraAttackI.abilityToHitModifier = BaseCreature.GetModifier(p.strength + (int)(p.weaponMastery / 2m)) + p.proficiencyBonus;
            ExtraAttackI.abilityCombatType = "melee";
            list.Add(ExtraAttackI);

            CombatAbilities ExtraAttackII = new CombatAbilities();
            ExtraAttackII.Name = "Extra Attack II";
            ExtraAttackII.ShortName = "e2";
            ExtraAttackII.Description = "Two extra standard melee attacks";
            ExtraAttackII.RequiredLevel = 11;
            ExtraAttackII.SpellCost = 0;
            ExtraAttackII.CurrentSpellCost = 0;
            ExtraAttackII.MinCost = 0;
            ExtraAttackII.abilityDamageDice = p.attackDie;
            ExtraAttackII.abilityNumberDamageDice = p.numberAttackDie;
            ExtraAttackII.abilityNumberAttacks = 2;
            ExtraAttackII.abilityToHitModifier = BaseCreature.GetModifier(p.strength + (int)(p.weaponMastery / 2m)) + p.proficiencyBonus;
            ExtraAttackII.abilityCombatType = "melee";
            list.Add(ExtraAttackII);

            CombatAbilities ExtraAttackIII = new CombatAbilities();
            ExtraAttackIII.Name = "Extra Attack III";
            ExtraAttackIII.ShortName = "e3";
            ExtraAttackIII.Description = "Three extra standard melee attacks";
            ExtraAttackIII.RequiredLevel = 20;
            ExtraAttackIII.SpellCost = 0;
            ExtraAttackIII.CurrentSpellCost = 0;
            ExtraAttackIII.MinCost = 0;
            ExtraAttackIII.abilityDamageDice = p.attackDie;
            ExtraAttackIII.abilityNumberDamageDice = p.numberAttackDie;
            ExtraAttackIII.abilityNumberAttacks = 3;
            ExtraAttackIII.abilityToHitModifier = BaseCreature.GetModifier(p.strength + (int)(p.weaponMastery / 2m)) + p.proficiencyBonus;
            ExtraAttackIII.abilityCombatType = "melee";
            list.Add(ExtraAttackII);

            return list;
        }
    }
}
