using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RaargeDungeon.Creatures;

namespace RaargeDungeon.Items
{
    public class SpellScroll : ScrollBase
    {
        public int spellDamageDice { get; set; }
        public string type { get; set; }
        public int spellDamageModifier { get; set; }
        public int spellNumberDamageDice { get; set; }
        public int critMultiplier { get; set; }
        public int dcCheck { get; set; }
        public int damageDicePerSpellLevel { get; set; }


        public static Player GetSpellScroll(Player p, string scrollName)
        {
            SpellScroll scroll = new SpellScroll();
            List<SpellScroll> spellList = new List<SpellScroll>();

            spellList = GetSpellsList();

            foreach(var spell in spellList)
            {
                if(scrollName == spell.Name)
                {
                    p.spells.Add(spell);
                }
            }

            return p;
        }

        public static List<SpellScroll> GetSpellsList()
        {
            List<SpellScroll> list = new List<SpellScroll>();

            #region Inflict Wounds
            SpellScroll InflictWounds = new SpellScroll();
            InflictWounds.Name = "Inflict Wounds";
            InflictWounds.ShortName = "iw";
            InflictWounds.Description = "Clerical spell that opens wounds on your target";
            InflictWounds.RequiredLevel = 1;
            InflictWounds.SpellCost = 14;
            InflictWounds.CurrentSpellCost = 14;
            InflictWounds.MinCost = 8;
            InflictWounds.FlavorText = "wounds begin to form and open on your enemy.";
            InflictWounds.spellDamageDice = 10;
            InflictWounds.type = "Necromancy";
            InflictWounds.spellDamageModifier = 0;
            InflictWounds.spellNumberDamageDice = 3;
            InflictWounds.critMultiplier = 2;
            list.Add(InflictWounds);
            #endregion

            #region Magic Missile
            SpellScroll MagicMissile = new SpellScroll();
            MagicMissile.Name = "Magic Missile";
            MagicMissile.ShortName = "mm";
            MagicMissile.Description = "Arcane Missiles that do not miss";
            MagicMissile.RequiredLevel = 1;
            MagicMissile.SpellCost = 12;
            MagicMissile.CurrentSpellCost = 12;
            MagicMissile.MinCost = 6;
            MagicMissile.FlavorText = "bolts of arcane energy spring forth and hurl towards your enemy.";
            MagicMissile.spellDamageDice = 4;
            MagicMissile.type = "Arcane";
            MagicMissile.spellDamageModifier = 1;
            MagicMissile.spellNumberDamageDice = 3;
            MagicMissile.critMultiplier = 3;
            list.Add(MagicMissile);
            #endregion



            return list;


        }
    }
}
