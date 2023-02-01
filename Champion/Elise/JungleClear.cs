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
    class JungleClear : Base
    {
        private static bool JungleClearQ => ChampionMenu["JungleClear"]["JCQ"].GetValue<MenuBool>().Enabled;
        private static bool JungleClearW => ChampionMenu["JungleClear"]["JCW"].GetValue<MenuBool>().Enabled;
        private static bool JungleClearR => ChampionMenu["JungleClear"]["JCR"].GetValue<MenuBool>().Enabled;
        private static bool JungleClearQ2 => ChampionMenu["JungleClear"]["JCQ2"].GetValue<MenuBool>().Enabled;
        private static bool JungleClearR2 => ChampionMenu["JungleClear"]["JCR2"].GetValue<MenuBool>().Enabled;

        public static void OnLoad()
        {
            if (JungleClearQ && Q.IsReady() && !Elise.IsSpider())
            {
                var target = Orbwalker.GetTarget() as AIMinionClient;
                if (target.IsJungle()) Q.Cast(target);
            }

            if (JungleClearW && W.IsReady() && !Elise.IsSpider())
            {
                var target = Orbwalker.GetTarget() as AIMinionClient;
                var position = JungleClearQ2 && R.IsReady() ? target.Position.Rotated(2) : target.Position;
                if (target.IsJungle()) W.Cast(position);
            }

            if (JungleClearR && R.IsReady() && !Elise.IsSpider() && !Q.IsReady() && !W.IsReady()) Elise.CastR();

            if (JungleClearQ2 && Q2.IsReady() && Elise.IsSpider())
            {
                var target = Orbwalker.GetTarget() as AIMinionClient;
                if (target.IsJungle()) Q2.Cast(target);
            }

            if (JungleClearR2 && R.IsReady() && Elise.IsSpider())
            {
                List<bool> IsChangeForm = new List<bool>();
                IsChangeForm.Add(Game.Time - Elise.LastGameTimeQ > Elise.CoolTimeQ ? true : false);
                IsChangeForm.Add(Game.Time - Elise.LastGameTimeW > Elise.CoolTimeW ? true : false);
                if (IsChangeForm.Where(x => x == false).Count() == 0) R.Cast();
            }
        }


    }
}
