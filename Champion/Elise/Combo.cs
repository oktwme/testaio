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
            if(!Elise.IsSpider())
            {
                if(ComboQ && Q.IsReady()) Q.Cast(TargetSelector.GetTarget(Q.Range, Q.DamageType));
            
                if(ComboW && W.IsReady() && !Q.IsReady())
                {
                    var target = TargetSelector.GetTarget(W.Range, W.DamageType);
                    var predW = W.GetPrediction(target, false, -1, new CollisionObjects[] { CollisionObjects.Minions });
                    var predW3 = W3.GetPrediction(Player, false, -1, new CollisionObjects[] { CollisionObjects.Minions });
                    if (predW.Hitchance >= HitChance.High && predW3.Hitchance >= HitChance.Medium) W.Cast(target.Position);
                    else if (predW.Hitchance >= HitChance.High) W.Cast(Player.Position);
                }
            
                if(ComboE && E.IsReady())
                {
                    var target = TargetSelector.GetTarget(E.Range, DamageType.Magical);
                    var delayRange = E.Range - GetMoveSpeedByDelay(target.MoveSpeed, E);

                    bool CanE = true;
                    var step = target.DistanceToPlayer() / 20;
                    for (int i = 0; i < 20; i++)
                    {
                        if (step * i < Q.Range || step * i > target.DistanceToPlayer() - (step * 2)) continue;
                        if (Player.Position.Extend(target.Position, step * i).IsWall()) CanE = false;
                    }

                    if(CanE)
                    {
                        var pred = E.GetPrediction(target, false, -1, new CollisionObjects[] { CollisionObjects.YasuoWall, CollisionObjects.Minions });
                        var hitchanceE = IsSummonerReady(target, "flash") ? HitChance.Dash : HitChance.Immobile;
                        if (target.DistanceToPlayer() < delayRange && (pred.Hitchance == hitchanceE || pred.Hitchance >= HitChance.High)) E.Cast(pred.CastPosition);
                        //if (target.DistanceToPlayer() < delayRange && pred.Hitchance >= HitChance.High) E.Cast(pred.CastPosition);
                    }
                }
            
                if (ComboR && R.IsReady() && !Q.IsReady() && !E.IsReady()) Elise.CastR();
            }
            else
            {
                if (ComboQ2 && Q2.IsReady())
                {
                    var target = TargetSelector.GetTarget(E.Range, DamageType.Magical);
                    Q2.Cast(target);
                }

                if (ComboR2 && R.IsReady())
                {
                    List<bool> IsChangeForm = new List<bool>();
                    IsChangeForm.Add(Game.Time - Elise.LastGameTimeQ > Elise.CoolTimeQ ? true : false);
                    IsChangeForm.Add(Game.Time - Elise.LastGameTimeW > Elise.CoolTimeW ? true : false);
                    IsChangeForm.Add(Game.Time - Elise.LastGameTimeE > Elise.CoolTimeE ? true : false);
                    if (IsChangeForm.Where(x => x == false).Count() == 0) R.Cast();
                }
            }
        }


    }
}
