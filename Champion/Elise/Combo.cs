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
    class Combo : Base
    {
        private static bool ComboQ => ChampionMenu["Combo"]["CQ"].GetValue<MenuBool>().Enabled;
        private static bool ComboW => ChampionMenu["Combo"]["CW"].GetValue<MenuBool>().Enabled;
        private static bool ComboE => ChampionMenu["Combo"]["CE"].GetValue<MenuBool>().Enabled;
        private static bool ComboR => ChampionMenu["Combo"]["CR"].GetValue<MenuBool>().Enabled;

        private static bool ComboQ2 => ChampionMenu["Combo"]["CQ2"].GetValue<MenuBool>().Enabled;
        private static bool ComboW2 => ChampionMenu["Combo"]["CW2"].GetValue<MenuBool>().Enabled;
        private static bool ComboR2 => ChampionMenu["Combo"]["CR2"].GetValue<MenuBool>().Enabled;

        public static void BasicLogic()
        {
            CastQ();
            CastW();
            CastE();
            CastR();
            CastQ2();
            CastR2();
        }

        public static void CastQ()
        {
            if (ComboQ && Q.IsReady() && !Elise.IsSpider()) Q.Cast(TargetSelector.GetTarget(Q.Range, Q.DamageType));
        }

        public static void CastW()
        {
            if (ComboW && W.IsReady() && !Q.IsReady() && !Elise.IsSpider())
            {
                var target = TargetSelector.GetTarget(W.Range, W.DamageType);
                var predW = W.GetPrediction(target, false, -1, new CollisionObjects[] { CollisionObjects.YasuoWall, CollisionObjects.Minions });
                var predW3 = W3.GetPrediction(Player, false, -1, new CollisionObjects[] { CollisionObjects.YasuoWall, CollisionObjects.Minions });

                if (target.DistanceToPlayer() < 275) W.Cast(Player.Position);
                else if (predW.Hitchance >= HitChance.High && predW3.Hitchance >= HitChance.Medium) W.Cast(predW.CastPosition);
                else if (target.DistanceToPlayer() < Q2.Range + GetMoveSpeedByDelay(Player.MoveSpeed, Q2)) W.Cast(predW.CastPosition.Rotated(3));
            }
        }

        public static void CastE()
        {
            if (ComboE && E.IsReady() && !Elise.IsSpider())
            {
                var target = TargetSelector.GetTarget(E.Range, DamageType.Magical);
                var delayRange = E.Range - GetMoveSpeedByDelay(target.MoveSpeed, E);
                var effectiveRange = Q2.Range + Player.MoveSpeed;
                if (effectiveRange > delayRange) effectiveRange = delayRange;

                bool CanE = true;
                var step = target.DistanceToPlayer() / 20;
                for (int i = 0; i < 20; i++)
                {
                    if (step * i < Q.Range || step * i > target.DistanceToPlayer() - (step * 2)) continue;
                    if (Player.Position.Extend(target.Position, step * i).IsWall()) CanE = false;
                }

                if (CanE)
                {
                    var pred = E.GetPrediction(target, false, -1, new CollisionObjects[] { CollisionObjects.YasuoWall, CollisionObjects.Minions });
                    var hitchanceE = IsSummonerReady(target, SummonerType.Flash) ? HitChance.Dash : HitChance.Immobile;
                    if (target.DistanceToPlayer() < effectiveRange)
                    {
                        if (pred.Hitchance == hitchanceE && !IsImmunity(target)) E.Cast(pred.CastPosition);
                        else if (pred.Hitchance >= HitChance.High && !IsImmunity(target)) E.Cast(pred.CastPosition);
                        else if ((pred.Hitchance == HitChance.Collision || IsImmunity(target)) && target.DistanceToPlayer() < Q2.Range) CastRwithoutE();
                    }
                }
            }
        }

        public static void CastR()
        {
            var target = Orbwalker.GetTarget() as AIHeroClient;
            bool fastChange = target.DistanceToPlayer() < 125 + Player.BaseMoveSpeed && Player.Level < 3;

            bool QIR = !Q.IsReady() || Q.Level == 0;
            bool WIR = !W.IsReady() || W.Level == 0;
            bool EIR = !E.IsReady() || E.Level == 0;

            bool CanChange = Elise.IsCoolDown(Q, 1.2f) && Elise.IsCoolDown(W, 1.2f) && Elise.IsCoolDown(E, 1.2f) || fastChange;
            if (ComboR && R.IsReady() && !Elise.IsSpider() && QIR && WIR && EIR && CanChange)
            {
                Elise.CoolTimeQ = Q.CooldownTime;
                Elise.LastGameTimeQ = Game.Time;
                Elise.CoolTimeW = W.CooldownTime;
                Elise.LastGameTimeW = Game.Time;
                Elise.CoolTimeE = E.CooldownTime;
                Elise.LastGameTimeE = Game.Time;
                R.Cast();
            }
        }

        private static void CastRwithoutE()
        {
            bool QIR = !Q.IsReady() || Q.Level == 0;
            bool WIR = !W.IsReady() || W.Level == 0;

            bool CanChange = Elise.IsCoolDown(Q, 1.2f) && Elise.IsCoolDown(W, 1.2f);
            if (ComboR && R.IsReady() && !Elise.IsSpider() && QIR && WIR && CanChange)
            {
                Elise.CoolTimeQ = Q.CooldownTime;
                Elise.LastGameTimeQ = Game.Time;
                Elise.CoolTimeW = W.CooldownTime;
                Elise.LastGameTimeW = Game.Time;
                Elise.CoolTimeE = E.CooldownTime;
                Elise.LastGameTimeE = Game.Time;
                if (Elise.IsCoolDown(Q, 1.2f) && Elise.IsCoolDown(W, 1.2f)) R.Cast();
            }
        }

        public static void CastQ2()
        {
            if (ComboQ2 && Q2.IsReady() && Elise.IsSpider())
            {
                var target = TargetSelector.GetTarget(E.Range, DamageType.Magical);
                Q2.Cast(target);
            }
        }

        public static void CastW2()
        {
            if (ComboW2 && Elise.IsSpider() && W2.IsReady() && !Orbwalker.LastTarget.IsMinion()) W2.Cast();
        }

        public static void CastR2()
        {
            if (ComboR2 && R.IsReady() && Elise.IsSpider())
            {
                bool CanChange = Elise.IsCoolDown(Q2, 1.5f) && Elise.IsCoolDown(W2, 1.5f) && !Player.HasBuff("EliseSpiderW");
                if (Elise.IsCast("Q") && Elise.IsCast("W") && Elise.IsCast("E") && CanChange)
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
