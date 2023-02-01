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
    class LastHit : Base
    {
        private static bool LastHitQ => ChampionMenu["LastHit"]["LHQ"].GetValue<MenuBool>().Enabled;
        private static bool LastHitQ2 => ChampionMenu["LastHit"]["LHQ2"].GetValue<MenuBool>().Enabled;
        private static int LastHitMana => ChampionMenu["LastHit"]["LHMana"].GetValue<MenuSlider>().Value;

        public static void OnLoad()
        {
            if (Player.ManaPercent < LastHitMana) return;

            if (!Elise.IsSpider())
            {
                if(LastHitQ && Q.IsReady())
                {
                    var target = GameObjects.EnemyMinions
                        .OrderBy(x => x.Health)
                        .Where(x => x.IsValidTarget(Q.Range) && Q.GetHealthPrediction(x) <= Q.GetDamage(x))
                        .FirstOrDefault();

                    Q.Cast(target);
                }
            }
            else
            {
                if(LastHitQ2 && Q2.IsReady())
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
