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
    class LastHit : Base
    {
        private static bool LastHitUseQ => ChampionMenu["LastHit"]["LHQ"].GetValue<MenuBool>().Enabled;
        private static int LastHitMana => ChampionMenu["LastHit"]["LHMana"].GetValue<MenuSlider>().Value;

        public static void OnLoad()
        {
            if (GameObjects.Player.ManaPercent < LastHitMana) return;

            if (LastHitUseQ && Q.IsReady())
            {
                foreach (var item in GameObjects.EnemyMinions.OrderByDescending(x => x.MaxHealth))
                {
                    if (!item.IsValidTarget(Q.Range)) continue;

                    if (Q.GetHealthPrediction(item) < Q.GetDamage(item) &&
                        (item.DistanceToPlayer() > GameObjects.Player.GetRealAutoAttackRange() ||
                        !GameObjects.Player.CanAttack || item.IsUnderAllyTurret()))
                    {
                        var pred = Q.GetPrediction(item, false, -1, new CollisionObjects[] { CollisionObjects.YasuoWall, CollisionObjects.Minions });
                        if (pred.Hitchance >= HitChance.High && Q.Cast(pred.CastPosition)) break;
                    }
                }
            }
        }


    }
}
