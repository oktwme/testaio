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
    class Harass : Base
    {
        public static bool HarassUseQ => ChampionMenu["Harass"]["HQ"].GetValue<MenuBool>().Enabled;
        public static bool HarassUseW => ChampionMenu["Harass"]["HW"].GetValue<MenuBool>().Enabled;
        public static int HarassMana => ChampionMenu["Harass"]["HMana"].GetValue<MenuSlider>().Value;
    
        public static void OnLoad()
        {
            if (GameObjects.Player.ManaPercent < HarassMana) return;

            if (HarassUseW && W.IsReady())
            {
                var target = TargetSelector.GetTarget(W.Range, DamageType.Physical);
                var delayRange = Q.Range - GetMoveSpeedByDelay(target.MoveSpeed, W);

                var predW = W.GetPrediction(target, false, -1, new CollisionObjects[] { CollisionObjects.YasuoWall, CollisionObjects.Building });
                var predQ = Q.GetPrediction(target, false, -1, new CollisionObjects[] { CollisionObjects.YasuoWall, CollisionObjects.Minions });

                if (HarassUseQ && Q.IsReady() && Q.Collision && predQ.CastPosition.DistanceToPlayer() <= delayRange)
                {
                    if (predW.Hitchance >= HitChance.High && predQ.Hitchance >= HitChance.High && W.Cast(predW.CastPosition)) Q.Cast(predQ.CastPosition);
                }
                else if (predW.CastPosition.DistanceToPlayer() <= Player.GetRealAutoAttackRange() - GetMoveSpeedByDelay(target.MoveSpeed, W) && predW.Hitchance >= HitChance.High)
                {
                    W.Cast(predW.CastPosition);
                    Orbwalker.Attack(target);
                }
            }

            if (HarassUseQ && Q.IsReady())
            {
                var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
                var pred = Q.GetPrediction(target, false, -1, new CollisionObjects[] { CollisionObjects.YasuoWall, CollisionObjects.Minions });
                if (pred.CastPosition.DistanceToPlayer() <= Q.Range - GetMoveSpeedByDelay(target.MoveSpeed, Q) && pred.Hitchance >= HitChance.High) Q.Cast(pred.CastPosition);
            }

            LastHit.OnLoad();
        }


    }
}
