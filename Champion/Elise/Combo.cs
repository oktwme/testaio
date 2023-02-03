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
        private static bool ComboR2 => ChampionMenu["Combo"]["CR2"].GetValue<MenuBool>().Enabled;

        public static void OnLoad()
        {
            if (ComboQ && Q.IsReady() && !Elise.IsSpider()) Q.Cast(TargetSelector.GetTarget(Q.Range, Q.DamageType));

            if (ComboW && W.IsReady() && !Q.IsReady() && !Elise.IsSpider())
            {
                var target = TargetSelector.GetTarget(W.Range, W.DamageType);
                var predW = W.GetPrediction(target, false, -1, new CollisionObjects[] { CollisionObjects.YasuoWall, CollisionObjects.Minions });
                var predW3 = W3.GetPrediction(Player, false, -1, new CollisionObjects[] { CollisionObjects.YasuoWall, CollisionObjects.Minions });

                if (target.DistanceToPlayer() < 275) W.Cast(Player.Position);
                else if(predW.Hitchance >= HitChance.High && predW3.Hitchance >= HitChance.Medium) W.Cast(predW.CastPosition);
                else if (target.DistanceToPlayer() < Q2.Range + GetMoveSpeedByDelay(Player.MoveSpeed, Q2)) W.Cast(predW.CastPosition.Rotated(3));
            }

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
                        else if ((pred.Hitchance == HitChance.Collision || IsImmunity(target)) && target.DistanceToPlayer() < Q2.Range) CastHumanR();
                    }
                }
            }

            if (ComboR && R.IsReady() && !Elise.IsSpider() && !Q.IsReady() && !W.IsReady() && !E.IsReady())
            {
                Elise.CoolTimeQ = Q.CooldownTime;
                Elise.LastGameTimeQ = Game.Time;
                Elise.CoolTimeW = W.CooldownTime;
                Elise.LastGameTimeW = Game.Time;
                Elise.CoolTimeE = E.CooldownTime;
                Elise.LastGameTimeE = Game.Time;
                if (Q.CooldownTime > 1.2f && W.CooldownTime > 1.2f && E.CooldownTime > 1.2f) R.Cast();
            }

            if (ComboQ2 && Q2.IsReady() && Elise.IsSpider())
            {
                var target = TargetSelector.GetTarget(E.Range, DamageType.Magical);
                Q2.Cast(target);
            }

            if (ComboR2 && R.IsReady() && Elise.IsSpider())
            {
                List<bool> IsChangeForm = new List<bool>();
                IsChangeForm.Add(Game.Time - Elise.LastGameTimeQ > Elise.CoolTimeQ ? true : false);
                IsChangeForm.Add(Game.Time - Elise.LastGameTimeW > Elise.CoolTimeW ? true : false);
                IsChangeForm.Add(Game.Time - Elise.LastGameTimeE > Elise.CoolTimeE ? true : false);
                bool CanChange = Q2.CooldownTime > 1.5f && W2.CooldownTime > 1.5f;
                if (IsChangeForm.Where(x => x == false).Count() == 0 && CanChange) R.Cast();
            }
        }

        private static void CastHumanR()
        {
            if (ComboR && R.IsReady() && !Elise.IsSpider() && !Q.IsReady() && !W.IsReady())
            {
                Elise.CoolTimeQ = Q.CooldownTime;
                Elise.LastGameTimeQ = Game.Time;
                Elise.CoolTimeW = W.CooldownTime;
                Elise.LastGameTimeW = Game.Time;
                Elise.CoolTimeE = E.CooldownTime;
                Elise.LastGameTimeE = Game.Time;
                if (Q.CooldownTime > 1.2f && W.CooldownTime > 1.2f) R.Cast();
            }
        }


    }
}
