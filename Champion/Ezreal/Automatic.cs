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
    class Automatic : Base
    {
        private static bool KSUseQ => ChampionMenu["KS"]["KSQ"].GetValue<MenuBool>().Enabled;
        private static bool KSUseR => ChampionMenu["KS"]["KSR"].GetValue<MenuBool>().Enabled;
        private static bool JungleClearUseR => ChampionMenu["JungleClear"]["JCR"].GetValue<MenuBool>().Enabled;

        public static void OnLoad()
        {
            if (KSUseQ && Q.IsReady())
            {
                var target = GameObjects.EnemyHeroes
                    .OrderBy(x => x.Health)
                    .Where(x => x.IsValidTarget(Q.Range - GetMoveSpeedByDelay(x.MoveSpeed, Q)) && Q.GetHealthPrediction(x) < Q.GetDamage(x))
                    .FirstOrDefault();

                var pred = Q.GetPrediction(target, false, -1, new CollisionObjects[] { CollisionObjects.YasuoWall, CollisionObjects.Minions });
                if (pred.Hitchance >= HitChance.High) Q.Cast(pred.CastPosition);
            }

            if (KSUseR && R.IsReady() && IsSafe())
            {
                var target = GameObjects.EnemyHeroes
                    .OrderBy(x => x.Health)
                    .Where(x => x.Health < R.GetDamage(x) && !x.IsInvulnerable && !x.IsDead)
                    .FirstOrDefault();

                var pred = R.GetPrediction(target, false, -1, new CollisionObjects[] { CollisionObjects.YasuoWall });
                if (pred.Hitchance >= HitChance.Immobile) R.Cast(pred.CastPosition);
            }

            if (JungleClearUseR && R.IsReady() && IsSafe())
            {
                AIMinionClient baron = null, dragon = null;
                foreach (var item in GameObjects.Jungle)
                {
                    if (item.IsBaron() && item.DistanceToPlayer() > 955) baron = item;
                    if (item.IsDragon() && item.DistanceToPlayer() > 955) dragon = item;
                }

                if (baron != null)
                {
                    var enemys = GetObjectInRangeEnemys(baron);
                    var damage = GetObjectTotalDamage(enemys, baron);
                    var speed = baron.DistanceToPlayer() / R.Speed + R.Delay;

                    if (baron.Health < (R.GetDamage(baron) * speed) + damage && !baron.IsDead && baron.IsEnemy && enemys.Count() > 0)
                    {
                        var pred = R.GetPrediction(baron, false, -1, new CollisionObjects[] { CollisionObjects.YasuoWall });
                        if (pred.Hitchance >= HitChance.High) R.Cast(pred.CastPosition);
                    }
                }
                else if (dragon != null)
                {
                    var enemys = GetObjectInRangeEnemys(dragon);
                    var damage = GetObjectTotalDamage(enemys, dragon);
                    var speed = dragon.DistanceToPlayer() / R.Speed + R.Delay;

                    if (dragon.Health < (R.GetDamage(dragon) * speed) + damage && !dragon.IsDead && enemys.Count() > 0)
                    {
                        var pred = R.GetPrediction(dragon, false, -1, new CollisionObjects[] { CollisionObjects.YasuoWall });
                        if (pred.Hitchance >= HitChance.High) R.Cast(pred.CastPosition);
                    }
                }
            }
        }


    }
}
