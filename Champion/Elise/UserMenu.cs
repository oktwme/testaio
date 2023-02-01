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
    class UserMenu : Base
    {
        public static void Init()
        {
            ChampionMenu.SetLogo(EnsoulSharp.SDK.Rendering.SpriteRender.CreateLogo(Properties.Resources.Elise));
            Korean = ChampionMenu.Add(new MenuBool("Korean", "Korean")).Enabled;

            var LastHit = ChampionMenu.Add(new Menu("LastHit", Korean ? "CS 막타" : "LastHit"));
            {
                LastHit.Add(new MenuSeparator("LHHS", Korean ? "인간폼" : "HUMAN FORM"));
                LastHit.Add(new MenuBool("LHQ", Korean ? "Q 사용" : "Use Q"));
                LastHit.Add(new MenuSeparator("LHSS", Korean ? "거미폼" : "SPIDER FORM"));
                LastHit.Add(new MenuBool("LHQ2", Korean ? "Q 사용" : "Use Q"));
                LastHit.Add(new MenuSlider("LHMana", Korean ? "마나가 X% 이상일 때만 사용" : "Don't LastHit if Mana <= X%", 30, 0, 100));
            }

            var LaneClear = ChampionMenu.Add(new Menu("LaneClear", Korean ? "라인 클리어" : "LaneClear"));
            {
                LaneClear.Add(new MenuSeparator("LCHS", Korean ? "인간폼" : "HUMAN FORM"));
                LaneClear.Add(new MenuBool("LCQ", Korean ? "Q 사용" : "Use Q"));
                LaneClear.Add(new MenuBool("LCW", Korean ? "W 사용" : "Use W"));
                LaneClear.Add(new MenuSeparator("LCSS", Korean ? "거미폼" : "SPIDER FORM"));
                LaneClear.Add(new MenuBool("LCQ2", Korean ? "Q 사용" : "Use Q"));
                LaneClear.Add(new MenuSlider("LCMana", Korean ? "마나가 X% 이상일 때만 사용" : "Don't LaneClear if Mana <= X%", 10, 0, 100));
            }

            var JungleClear = ChampionMenu.Add(new Menu("JungleClear", Korean ? "정글 클리어" : "JungleClear"));
            {
                JungleClear.Add(new MenuSeparator("JCHS", Korean ? "인간폼" : "HUMAN FORM"));
                JungleClear.Add(new MenuBool("JCQ", Korean ? "Q 사용" : "Use Q"));
                JungleClear.Add(new MenuBool("JCW", Korean ? "W 사용" : "Use W"));
                JungleClear.Add(new MenuBool("JCE", Korean ? "E 사용" : "Use E"));
                JungleClear.Add(new MenuBool("JCR", Korean ? "R 사용" : "Use W"));
                JungleClear.Add(new MenuSeparator("JCSS", Korean ? "거미폼" : "SPIDER FORM"));
                JungleClear.Add(new MenuBool("JCQ2", Korean ? "Q 사용" : "Use Q"));
                JungleClear.Add(new MenuBool("JCW2", Korean ? "W 사용" : "Use W"));
                JungleClear.Add(new MenuBool("JCR2", Korean ? "R 사용" : "Use R"));
            }

            //var Harass = ChampionMenu.Add(new Menu("Harass", Korean ? "견제" : "Harass"));
            //{
            //    Harass.Add(new MenuBool("HQ", Korean ? "Q 사용" : "Use Q"));
            //    Harass.Add(new MenuBool("HW", Korean ? "W 사용" : "Use W"));
            //    Harass.Add(new MenuSlider("HMana", Korean ? "마나가 X% 이상일 때만 사용" : "Don't Harass if Mana <= X%", 30, 0, 100));
            //}

            var combo = ChampionMenu.Add(new Menu("Combo", Korean ? "콤보" : "Combo"));
            {
                combo.Add(new MenuSeparator("CHS", Korean ? "인간폼" : "HUMAN FORM"));
                combo.Add(new MenuBool("CQ", Korean ? "Q 사용" : "Use Q"));
                combo.Add(new MenuBool("CW", Korean ? "W 사용" : "Use W"));
                combo.Add(new MenuBool("CE", Korean ? "E 사용" : "Use E"));
                combo.Add(new MenuBool("CR", Korean ? "R 사용" : "Use R"));
                combo.Add(new MenuSeparator("CSS", Korean ? "거미폼" : "SPIDER FORM"));
                combo.Add(new MenuBool("CQ2", Korean ? "Q 사용" : "Use Q"));
                combo.Add(new MenuBool("CW2", Korean ? "W 사용" : "Use W"));
                combo.Add(new MenuBool("CR2", Korean ? "R 사용" : "Use R"));
            }
        }
    }
}
