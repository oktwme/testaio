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
    class LaneClear : Base
    {
        private static bool LaneClearQ => ChampionMenu["LaneClear"]["LCQ"].GetValue<MenuBool>().Enabled;
        private static bool LaneClearW => ChampionMenu["LaneClear"]["LCW"].GetValue<MenuBool>().Enabled;
        private static bool LaneClearQ2 => ChampionMenu["LaneClear"]["LCQ2"].GetValue<MenuBool>().Enabled;
        private static int LaneClearMana => ChampionMenu["LaneClear"]["LCMana"].GetValue<MenuSlider>().Value;

        public static void OnLoad()
        {
            if (Player.ManaPercent < LaneClearMana) return;

            if (!Elise.IsSpider())
            {
                if (LaneClearQ && Q.IsReady())
                {
                    var target = GameObjects.EnemyMinions
                        .OrderBy(x => x.Health)
                        .Where(x => x.IsValidTarget(Q.Range) && Q.GetHealthPrediction(x) <= Q.GetDamage(x))
                        .FirstOrDefault();

                    Q.Cast(target);
                }

                if (LaneClearW && W.IsReady())
                {
                    var minions = GameObjects.EnemyMinions
                        .Where(x => x.IsValidTarget(W.Range + Player.MoveSpeed))
                        .ToList();

                    if(minions.Count() == 1)
                    {
                        var target = minions.FirstOrDefault();
                        if(W.GetHealthPrediction(target) <= W.GetDamage(target)) W.Cast(target.Position);
                    }
                    else
                    {
                        var minHitCount = 4;
                        if (minions.Count() <= 3) minHitCount = 2;

                        var predFW = W3.GetLineFarmLocation(minions);
                        if (predFW.MinionsHit >= minHitCount) W.Cast(predFW.Position);

                        if (W.IsReady())
                        {
                            minHitCount = 3;
                            if (predFW.MinionsHit >= minHitCount) W.Cast(predFW.Position);
                        }
                    }
                }
            }
            else
            {
                if (LaneClearQ2 && Q2.IsReady())
                {
                    var target = GameObjects.EnemyMinions
                        .OrderBy(x => x.Health)
                        .Where(x => x.IsValidTarget(Q2.Range) && Q2.GetHealthPrediction(x) <= Q2.GetDamage(x))
                        .FirstOrDefault();

                    Q2.Cast(target);
                }
            }
        }


    }
}
