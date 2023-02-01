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
    class UserMenu : Base
    {
        public static void Init()
        {
            ChampionMenu.SetLogo(EnsoulSharp.SDK.Rendering.SpriteRender.CreateLogo(Properties.Resources.Ezreal));
            Korean = ChampionMenu.Add(new MenuBool("Korean", "Korean")).Enabled;

            var LastHit = ChampionMenu.Add(new Menu("LastHit", Korean ? "CS 막타" : "LastHit"));
            {
                LastHit.Add(new MenuBool("LHQ", Korean ? "Q 사용" : "Use Q"));
                LastHit.Add(new MenuSlider("LHMana", Korean ? "마나가 X% 이상일 때만 사용" : "Don't LastHit if Mana <= X%", 30, 0, 100));
            }

            var LaneClear = ChampionMenu.Add(new Menu("LaneClear", Korean ? "라인 클리어" : "LaneClear"));
            {
                LaneClear.Add(new MenuBool("LCQ", Korean ? "Q 사용" : "Use Q"));
                LaneClear.Add(new MenuBool("LCW", Korean ? "W 사용" : "Use W"));
                LaneClear.Add(new MenuSlider("LCMana", Korean ? "마나가 X% 이상일 때만 사용" : "Don't LaneClear if Mana <= X%", 10, 0, 100));
            }

            var JungleClear = ChampionMenu.Add(new Menu("JungleClear", Korean ? "정글 클리어" : "JungleClear"));
            {
                JungleClear.Add(new MenuBool("JCQ", Korean ? "Q 사용" : "Use Q"));
                JungleClear.Add(new MenuBool("JCW", Korean ? "W 사용" : "Use W"));
                JungleClear.Add(new MenuBool("JCR", Korean ? "오브젝트 궁 스틸" : "Object Steal Use R"));
            }

            var Harass = ChampionMenu.Add(new Menu("Harass", Korean ? "견제" : "Harass"));
            {
                Harass.Add(new MenuBool("HQ", Korean ? "Q 사용" : "Use Q"));
                Harass.Add(new MenuBool("HW", Korean ? "W 사용" : "Use W"));
                Harass.Add(new MenuSlider("HMana", Korean ? "마나가 X% 이상일 때만 사용" : "Don't Harass if Mana <= X%", 30, 0, 100));
            }

            var combo = ChampionMenu.Add(new Menu("Combo", Korean ? "콤보" : "Combo"));
            {
                combo.Add(new MenuBool("CQ", Korean ? "Q 사용" : "Use Q"));
                combo.Add(new MenuBool("CW", Korean ? "W 사용" : "Use W"));
                combo.Add(new MenuBool("CR", Korean ? "R 사용" : "Use R"));
            }

            var KS = ChampionMenu.Add(new Menu("KS", Korean ? "킬 스틸" : "Kill Steal"));
            {
                KS.Add(new MenuBool("KSQ", Korean ? "Q 사용" : "Use Q"));
                KS.Add(new MenuBool("KSR", Korean ? "R 사용" : "Use R"));
            }
        }
    }
}
