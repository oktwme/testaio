using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnsoulSharp;
using EnsoulSharp.SDK;
using EnsoulSharp.SDK.MenuUI;
using RankerAIO.Common;

namespace RankerAIO.Champion.Elise
{
    class Elise : Base
    {
        public static void Init()
        {
            Q = new Spell(SpellSlot.Q, 575f) { Delay = 0.25f };
            W = new Spell(SpellSlot.W, 950f);
            E = new Spell(SpellSlot.E, 1100f);

            Q2 = new Spell(SpellSlot.Q, 475f) { Delay = 0.25f };
            W2 = new Spell(SpellSlot.W, 0f);
            E2 = new Spell(SpellSlot.E, 700f) { Delay = 0 };

            W3 = new Spell(SpellSlot.W, 950f);

            R = new Spell(SpellSlot.R, 0f);

            W.SetSkillshot(0.125f, 75f, 1000f, true, SpellType.Line);
            W3.SetSkillshot(0.125f, 150f, 0f, true, SpellType.Circle);
            E.SetSkillshot(0.25f, 55f, 1600f, true, SpellType.Line);

            Q.DamageType = W.DamageType = E.DamageType = DamageType.Magical;
            Q2.DamageType = W2.DamageType = DamageType.Magical;

            ChampionMenu = new Menu("Elise", Korean ? "엘리스" : "Elise", true).Attach();
            UserMenu.Init();

            Game.OnUpdate += GameOnUpdate;
            Orbwalker.OnBeforeAttack += Orbwalker_OnBeforeAttack;
            Orbwalker.OnAfterAttack += Orbwalker_OnAfterAttack;
        }

        public static float CoolTimeQ = 0, LastGameTimeQ = 0;
        public static float CoolTimeW = 0, LastGameTimeW = 0;
        public static float CoolTimeE = 0, LastGameTimeE = 0;

        public static bool IsSpider()
        {
            return Player.GetSpell(SpellSlot.R).Name.ToLower().Contains("spider");
        }

        public static void CastR()
        {
            CoolTimeQ = Q.CooldownTime;
            LastGameTimeQ = Game.Time;
            CoolTimeW = W.CooldownTime;
            LastGameTimeW = Game.Time;
            CoolTimeE = E.CooldownTime;
            LastGameTimeE = Game.Time;
            R.Cast();
        }

        private static bool ComboW2 => ChampionMenu["Combo"]["CW2"].GetValue<MenuBool>().Enabled;
        private static bool JungleClearW2 => ChampionMenu["JungleClear"]["JCW2"].GetValue<MenuBool>().Enabled;
        private static void Orbwalker_OnAfterAttack(object sender, AfterAttackEventArgs e)
        {
            if (ComboW2 && IsSpider() && W2.IsReady() && !Orbwalker.LastTarget.IsMinion()) W2.Cast();
            if (JungleClearW2 && IsSpider() && W2.IsReady() && Orbwalker.LastTarget.IsJungle()) W2.Cast();
        }

        private static bool JungleClearE => ChampionMenu["JungleClear"]["JCE"].GetValue<MenuBool>().Enabled;
        private static void Orbwalker_OnBeforeAttack(object sender, BeforeAttackEventArgs e)
        {
            if (JungleClearE && E.IsReady() && !IsSpider() && Player.Level < 6 && Player.HealthPercent < 40)
            {
                var target = Orbwalker.GetTarget() as AIMinionClient;
                if (target.IsJungle() && target.MaxHealth > 1000)
                {
                    var pred = E.GetPrediction(target, false, -1, new CollisionObjects[] { CollisionObjects.YasuoWall, CollisionObjects.Minions });
                    if (pred.Hitchance >= HitChance.High) E.Cast(pred.CastPosition);
                }
            }
        }

        private static void GameOnUpdate(EventArgs args)
        {
            if(Orbwalker.CanAttack())
            {
                switch (Orbwalker.ActiveMode)
                {
                    case OrbwalkerMode.Combo:
                        Combo.OnLoad();
                        break;
                    case OrbwalkerMode.Harass:
                        break;
                    case OrbwalkerMode.LaneClear:
                        JungleClear.OnLoad();
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
