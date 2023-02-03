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
            Orbwalker.OnBeforeAttack += Orbwalker_OnBeforeAttack;
            Orbwalker.OnAfterAttack += Orbwalker_OnAfterAttack;
        }

        private static bool LastHitUseQ => ChampionMenu["LastHit"]["LHQ"].GetValue<MenuBool>().Enabled;
        private static int LastHitMana => ChampionMenu["LastHit"]["LHMana"].GetValue<MenuSlider>().Value;
        private static void Orbwalker_OnBeforeAttack(object sender, BeforeAttackEventArgs e)
        {
            if (GameObjects.Player.ManaPercent < LastHitMana) return;

            if (Orbwalker.ActiveMode == OrbwalkerMode.LastHit && LastHitUseQ && Q.IsReady())
            {
                var target = Orbwalker.GetTarget() as AIMinionClient;

                if(target.IsMinion() && Q.GetHealthPrediction(target) < Q.GetDamage(target))
                {
                    var pred = Q.GetPrediction(target, false, -1, new CollisionObjects[] { CollisionObjects.YasuoWall, CollisionObjects.Minions });
                    if (pred.Hitchance >= HitChance.High) Q.Cast(pred.CastPosition);
                }
            }
        }

        private static void Orbwalker_OnAfterAttack(object sender, AfterAttackEventArgs e)
        {
            if (GameObjects.Player.ManaPercent < LastHitMana) return;

            if (Orbwalker.ActiveMode == OrbwalkerMode.LastHit && LastHitUseQ && Q.IsReady())
            {
                var target = GameObjects.EnemyMinions
                    .OrderBy(x => x.Health)
                    .Where(x => x.IsValidTarget(Q.Range) && x.DistanceToPlayer() > Player.GetRealAutoAttackRange() && Q.GetHealthPrediction(x) < Q.GetDamage(x))
                    .FirstOrDefault();

                var pred = Q.GetPrediction(target, false, -1, new CollisionObjects[] { CollisionObjects.YasuoWall, CollisionObjects.Minions });
                if (pred.Hitchance >= HitChance.High) Q.Cast(pred.CastPosition);
            }
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
                        break;
                    case OrbwalkerMode.Flee:
                        break;
                }
            }
        }


    }
}
