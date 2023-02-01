using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnsoulSharp;
using EnsoulSharp.SDK;
using EnsoulSharp.SDK.MenuUI;
using RankerAIO.Common;

namespace RankerAIO.Champion.Ezreal
{
    class Ezreal : Base
    {
        public static void Init()
        {
            Q = new Spell(SpellSlot.Q, 1150f);
            Q.SetSkillshot(0.25f, 60f, 2000f, true, SpellType.Line);

            W = new Spell(SpellSlot.W, 1150f);
            W.SetSkillshot(0.25f, 80f, 1700f, false, SpellType.Line);

            E = new Spell(SpellSlot.E, 475f) { Delay = 0.65f };

            R = new Spell(SpellSlot.R, 20000f);
            R.SetSkillshot(1f, 160f, 2000f, false, SpellType.Line);

            Q.DamageType = W.DamageType = E.DamageType = R.DamageType = DamageType.Physical;

            ChampionMenu = new Menu("Ezreal", Korean ? "이즈리얼" : "Ezreal", true).Attach();
            UserMenu.Init();

            Game.OnUpdate += GameOnUpdate;
        }

        private static void GameOnUpdate(EventArgs args)
        {
            if (Orbwalker.CanAttack())
            {
                Automatic.OnLoad();

                switch (Orbwalker.ActiveMode)
                {
                    case OrbwalkerMode.Combo:
                        Combo.OnLoad();
                        break;
                    case OrbwalkerMode.Harass:
                        Harass.OnLoad();
                        break;
                    case OrbwalkerMode.LaneClear:
                        LaneClear.OnLoad();
                        break;
                    case OrbwalkerMode.LastHit:
                        LastHit.OnLoad();
                        break;
                    case OrbwalkerMode.Flee:
                        break;
                }
            }
        }


    }
}
