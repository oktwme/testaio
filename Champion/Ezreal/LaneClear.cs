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
    class LaneClear : Base
    {
        private static bool LaneClearUseQ => ChampionMenu["LaneClear"]["LCQ"].GetValue<MenuBool>().Enabled;
        private static bool LaneClearUseW => ChampionMenu["LaneClear"]["LCW"].GetValue<MenuBool>().Enabled;
        private static int LaneClearMana => ChampionMenu["LaneClear"]["LCMana"].GetValue<MenuSlider>().Value;

        private static bool JungleClearUseQ => ChampionMenu["JungleClear"]["JCQ"].GetValue<MenuBool>().Enabled;
        private static bool JungleClearUseW => ChampionMenu["JungleClear"]["JCW"].GetValue<MenuBool>().Enabled;

        public static void OnLoad()
        {
            if (GameObjects.Player.ManaPercent < LaneClearMana) return;

            if (LaneClearUseQ && Q.IsReady())
            {
                var minions = GameObjects.EnemyMinions
                        .OrderBy(x => x.Health)
                        .Where(x => x.IsValidTarget(Q.Range - Player.MoveSpeed) && !x.IsDead);

                var minion = minions.Find(x => Q.GetHealthPrediction(x) < Q.GetDamage(x) ||
                (Q.GetHealthPrediction(x) > (Q.GetDamage(x) + Player.GetAutoAttackDamage(x)) / Player.AttackSpeed() && !x.IsUnderAllyTurret()));

                var pred = Q.GetPrediction(minion, false, -1, new CollisionObjects[] { CollisionObjects.YasuoWall, CollisionObjects.Minions });
                if (pred.Hitchance >= HitChance.High) Q.Cast(pred.CastPosition);
            }

            if (LaneClearUseW && W.IsReady())
            {
                if(GameObjects.EnemyHeroes.OrderBy(x => x.DistanceToPlayer()).Where(x => x.IsValidTarget(x.GetRealAutoAttackRange() + 400)).Count() == 0)
                {
                    var target = Orbwalker.GetTarget() as AITurretClient;
                    var player = GameObjects.Player;
                    if (player.Position.Distance(target) <= player.GetRealAutoAttackRange() + (player.MoveSpeed * W.Delay))
                    {
                        W.Cast(target.Position);
                        Orbwalker.Attack(target);
                    }
                }
            }

            if (JungleClearUseW && W.IsReady())
            {
                var target = Orbwalker.GetTarget() as AIMinionClient;
                if (target.IsJungle() && (target.IsBaron() || target.IsDragon()))
                {
                    if (JungleClearUseQ && Q.IsReady())
                    {
                        var predW = W.GetPrediction(target, false, -1, new CollisionObjects[] { CollisionObjects.YasuoWall, CollisionObjects.Building });
                        var predQ = Q.GetPrediction(target, false, -1, new CollisionObjects[] { CollisionObjects.YasuoWall });
                        if (predW.Hitchance >= HitChance.High && predQ.Hitchance >= HitChance.High && W.Cast(predW.CastPosition)) Q.Cast(predQ.CastPosition);
                    }
                    else
                    {
                        var pred = W.GetPrediction(target, false, -1, new CollisionObjects[] { CollisionObjects.YasuoWall, CollisionObjects.Building });
                        if (pred.Hitchance >= HitChance.High) W.Cast(pred.CastPosition);
                    }
                }
            }

            if (JungleClearUseQ && Q.IsReady())
            {
                var jungles = GameObjects.Jungle
                    .OrderBy(x => x.Health)
                    .Where(x => x.IsValidTarget(Q.Range - GetMoveSpeedByDelay(x.MoveSpeed, Q)) && (x.IsDragon() || x.IsBaron()) && Q.GetHealthPrediction(x) < Q.GetDamage(x));

                if (jungles.Count() > 0)
                {
                    var pred = Q.GetPrediction(jungles.FirstOrDefault(), false, -1, new CollisionObjects[] { CollisionObjects.YasuoWall });
                    if (pred.Hitchance >= HitChance.High) Q.Cast(pred.CastPosition);
                }
                else
                {
                    var target = Orbwalker.GetTarget() as AIMinionClient;
                    if (target.IsJungle())
                    {
                        var predQ = Q.GetPrediction(target, false, -1, new CollisionObjects[] { CollisionObjects.YasuoWall });
                        if (predQ.Hitchance >= HitChance.High) Q.Cast(predQ.CastPosition);
                    }
                }
            }
        }

    }
}
