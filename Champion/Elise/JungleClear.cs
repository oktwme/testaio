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
    class JungleClear : Base
    {
        private static bool JungleClearQ => ChampionMenu["JungleClear"]["JCQ"].GetValue<MenuBool>().Enabled;
        private static bool JungleClearW => ChampionMenu["JungleClear"]["JCW"].GetValue<MenuBool>().Enabled;
        private static bool JungleClearE => ChampionMenu["JungleClear"]["JCE"].GetValue<MenuBool>().Enabled;
        private static bool JungleClearR => ChampionMenu["JungleClear"]["JCR"].GetValue<MenuBool>().Enabled;

        private static bool JungleClearQ2 => ChampionMenu["JungleClear"]["JCQ2"].GetValue<MenuBool>().Enabled;
        private static bool JungleClearW2 => ChampionMenu["JungleClear"]["JCW2"].GetValue<MenuBool>().Enabled;
        private static bool JungleClearR2 => ChampionMenu["JungleClear"]["JCR2"].GetValue<MenuBool>().Enabled;

        public static void BasicLogic()
        {
            CastQ();
            CastW();
            CastR();
            CastQ2();
            CastR2();
        }

        public static void CastQ()
        {
            if (JungleClearQ && Q.IsReady() && !Elise.IsSpider())
            {
                var target = Orbwalker.GetTarget() as AIMinionClient;
                if (target.IsJungle()) Q.Cast(target);
            }
        }

        public static void CastW()
        {
            if (JungleClearW && W.IsReady() && !Elise.IsSpider())
            {
                var target = Orbwalker.GetTarget() as AIMinionClient;
                var position = JungleClearQ2 && Q.Level >= 1 && R.IsReady() && Elise.IsCast("Q2") ? target.Position.Rotated(2) : target.Position;
                if (target.IsJungle()) W.Cast(position);
            }
        }

        public static void CastE()
        {
            if (JungleClearE && E.IsReady() && !Elise.IsSpider() && Player.Level < 6 && Player.HealthPercent < 40)
            {
                var target = Orbwalker.GetTarget() as AIMinionClient;
                if (target.IsJungle() && target.MaxHealth > 1000)
                {
                    var pred = E.GetPrediction(target, false, -1, new CollisionObjects[] { CollisionObjects.YasuoWall, CollisionObjects.Minions });
                    if (pred.Hitchance >= HitChance.High) E.Cast(pred.CastPosition);
                }
            }
        }

        public static void CastR()
        {
            if (JungleClearR && R.IsReady() && !Elise.IsSpider())
            {
                Elise.CoolTimeQ = Q.CooldownTime;
                Elise.LastGameTimeQ = Game.Time;
                Elise.CoolTimeW = W.CooldownTime;
                Elise.LastGameTimeW = Game.Time;
                Elise.CoolTimeE = E.CooldownTime;
                Elise.LastGameTimeE = Game.Time;
                if (Elise.IsCoolDown(Q, 1.5f) && Elise.IsCoolDown(W, 1.5f)) R.Cast();
            }
        }

        public static void CastQ2()
        {
            if (JungleClearQ2 && Q2.IsReady() && Elise.IsSpider())
            {
                var target = Orbwalker.GetTarget() as AIMinionClient;
                if (target.IsJungle()) Q2.Cast(target);
            }
        }

        public static void CastW2()
        {
            if (JungleClearW2 && Elise.IsSpider() && W2.IsReady() && Orbwalker.LastTarget.IsJungle()) W2.Cast();
        }

        public static void CastR2()
        {
            if (JungleClearR2 && R.IsReady() && Elise.IsSpider())
            {
                bool CanChange = Elise.IsCoolDown(Q2, 2.0f) && Elise.IsCoolDown(W2, 2.0f) && !Player.HasBuff("EliseSpiderW");
                if (Elise.IsCast("Q") && Elise.IsCast("W") && CanChange)
                {
                    Elise.CoolTimeQ2 = Q2.CooldownTime;
                    Elise.LastGameTimeQ2 = Game.Time;
                    Elise.CoolTimeW2 = W2.CooldownTime;
                    Elise.LastGameTimeW2 = Game.Time;
                    Elise.CoolTimeE2 = E2.CooldownTime;
                    Elise.LastGameTimeE2 = Game.Time;
                    R.Cast();
                }
            }
        }


    }
}
