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
    class Combo : Base
    {
        private static bool ComboUseQ => ChampionMenu["Combo"]["CQ"].GetValue<MenuBool>().Enabled;
        private static bool ComboUseW => ChampionMenu["Combo"]["CW"].GetValue<MenuBool>().Enabled;
        private static bool ComboUseR => ChampionMenu["Combo"]["CR"].GetValue<MenuBool>().Enabled;
    
        public static void OnLoad()
        {
            if (ComboUseW && W.IsReady())
            {
                var target = TargetSelector.GetTarget(W.Range, DamageType.Physical);
                var delayRange = Q.Range - GetMoveSpeedByDelay(target.MoveSpeed, W);

                var predW = W.GetPrediction(target, false, -1, new CollisionObjects[] { CollisionObjects.YasuoWall, CollisionObjects.Building });
                var predQ = Q.GetPrediction(target, false, -1, new CollisionObjects[] { CollisionObjects.YasuoWall, CollisionObjects.Minions });

                if (ComboUseQ && Q.IsReady() && Q.Collision && predQ.CastPosition.DistanceToPlayer() <= delayRange)
                {
                    if (predW.Hitchance >= HitChance.High && predQ.Hitchance >= HitChance.High && W.Cast(predW.CastPosition)) Q.Cast(predQ.CastPosition);
                }
                else if (predW.CastPosition.DistanceToPlayer() <= Player.GetRealAutoAttackRange() - GetMoveSpeedByDelay(target.MoveSpeed, W) && predW.Hitchance >= HitChance.High)
                {
                    W.Cast(predW.CastPosition);
                    Orbwalker.Attack(target);
                }
            }

            if (ComboUseQ && Q.IsReady())
            {
                var target = TargetSelector.GetTarget(Q.Range, DamageType.Physical);
                var pred = Q.GetPrediction(target, false, -1, new CollisionObjects[] { CollisionObjects.YasuoWall, CollisionObjects.Minions });
                if (pred.CastPosition.DistanceToPlayer() <= Q.Range - GetMoveSpeedByDelay(target.MoveSpeed, Q) && pred.Hitchance >= HitChance.High) Q.Cast(pred.CastPosition);
            }

            if (ComboUseR && R.IsReady())
            {
                var DontMoveEnemys = GetCCEnemys();
                var target = DontMoveEnemys.Count >= 3 ? DontMoveEnemys.OrderBy(x => x.Health).FirstOrDefault() : null;

                if(target != null)
                {
                    var pred = R.GetPrediction(target, false, -1, new CollisionObjects[] { CollisionObjects.YasuoWall });
                    if (pred.Hitchance >= HitChance.High && pred.AoeTargetsHitCount >= 3) R.Cast(pred.CastPosition);
                }
                else if (IsSafe())
                {
                    var LowHpEnemys = GameObjects.EnemyHeroes
                        .OrderBy(x => x.Health)
                        .Where(x => R.GetHealthPrediction(x) < R.GetDamage(x));

                    var pred = R.GetPrediction(LowHpEnemys.FirstOrDefault(), false, -1, new CollisionObjects[] { CollisionObjects.YasuoWall });
                    if (LowHpEnemys.Count() >= 2 && pred.Hitchance < HitChance.High && pred.AoeTargetsHitCount >= 2)
                    {
                        var enemys = pred.AoeTargetsHit;
                        if (enemys[0].Position.Distance(enemys[1]) <= R.Width) R.Cast(pred.CastPosition);
                    }
                }
            }
        }


    }
}
