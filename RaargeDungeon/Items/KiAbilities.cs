using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaargeDungeon.Creatures;

namespace RaargeDungeon.Items
{
    public class KiAbilities : ScrollBase
    {
        public int abilityDamageDice { get; set; }
        public int abilityNumberDamageDice { get; set;}
        public int abilityToHitModifier { get; set; }
        public string abilityChiType { get; set; }
        public int abilityNumberAttacks { get; set; }


        public static Player GetKiAbilities(Player p, string kiAbility)
        {
            KiAbilities kiAbil = new KiAbilities();
            List<KiAbilities> kiAbilList = new List<KiAbilities>();

            kiAbilList = GetKiAbilitiesList(p);

            foreach(var ability in kiAbilList)
            {
                if(kiAbility == ability.Name)
                {
                    p.kiAbilities.Add(ability);
                }
            }

            return p;
        }

        public static List<KiAbilities> GetKiAbilitiesList(Player p)
        {
            List<KiAbilities> kiAbilList = new List<KiAbilities>();

            #region Flurry of Blows
            KiAbilities FlurryOfBlows = new KiAbilities();
            FlurryOfBlows.Name = "Flurry of Blows";
            FlurryOfBlows.ShortName = "fb";
            FlurryOfBlows.Description = "Unleash a flurry of attacks after an initial attack";
            FlurryOfBlows.RequiredLevel = 2;
            FlurryOfBlows.SpellCost = 1;
            FlurryOfBlows.MinCost = 1;
            FlurryOfBlows.CurrentSpellCost = 1;
            FlurryOfBlows.abilityDamageDice = p.attackDie;
            FlurryOfBlows.abilityNumberDamageDice = p.numberAttackDie;
            FlurryOfBlows.abilityNumberAttacks = 2;
            FlurryOfBlows.abilityToHitModifier = BaseCreature.GetModifier(p.strength + (int)(p.weaponMastery / 2m)) + p.proficiencyBonus;
            FlurryOfBlows.abilityChiType = "melee";
            kiAbilList.Add(FlurryOfBlows);

            return kiAbilList;
            
            #endregion
        }
    }
}
