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
    class Elise : Base
    {
        public static void Init()
        {
            Q = new Spell(SpellSlot.Q, 575f) { Delay = 0.25f };
            W = new Spell(SpellSlot.W, 950f);
            E = new Spell(SpellSlot.E, 1100f);

            Q2 = new Spell(SpellSlot.Q, 475f) { Delay = 0.25f };
            W2 = new Spell(SpellSlot.W, 0f);
            E2 = new Spell(SpellSlot.E, 700f) { Delay = 0 };

            W3 = new Spell(SpellSlot.W, 950f);

            R = new Spell(SpellSlot.R, 0f);

            W.SetSkillshot(0.125f, 75f, 1000f, true, SpellType.Line);
            W3.SetSkillshot(0.125f, 137.5f, 1000f, true, SpellType.Circle);
            E.SetSkillshot(0.25f, 55f, 1600f, true, SpellType.Line);

            Q.DamageType = W.DamageType = E.DamageType = DamageType.Magical;
            Q2.DamageType = W2.DamageType = DamageType.Magical;

            ChampionMenu = new Menu("Elise", Korean ? "엘리스" : "Elise", true).Attach();
            UserMenu.Init();

            Game.OnUpdate += GameOnUpdate;
            Orbwalker.OnBeforeAttack += Orbwalker_OnBeforeAttack;
            Orbwalker.OnAfterAttack += Orbwalker_OnAfterAttack;
        }

        public static float CoolTimeQ = 0, LastGameTimeQ = 0;
        public static float CoolTimeW = 0, LastGameTimeW = 0;
        public static float CoolTimeE = 0, LastGameTimeE = 0;

        public static float CoolTimeQ2 = 0, LastGameTimeQ2 = 0;
        public static float CoolTimeW2 = 0, LastGameTimeW2 = 0;
        public static float CoolTimeE2 = 0, LastGameTimeE2 = 0;

        /// <summary>
        /// 스킬을 사용할 수 있는 상태인지 확인
        /// </summary>
        /// <param name="slot"></param>
        /// <returns>사용할 수 있다면 True</returns>
        public static bool IsCast(string slot)
        {
            switch (slot)
            {
                case "Q":
                    return Game.Time - LastGameTimeQ >= CoolTimeQ;
                case "W":
                    return Game.Time - LastGameTimeW >= CoolTimeW;
                case "E":
                    return Game.Time - LastGameTimeE >= CoolTimeE;
                case "Q2":
                    return Game.Time - LastGameTimeQ2 >= CoolTimeQ2;
                case "W2":
                    return Game.Time - LastGameTimeW2 >= CoolTimeW2;
                case "E2":
                    return Game.Time - LastGameTimeE2 >= CoolTimeE2;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 입력한 쿨타임 보다 많이 남았는지 확인
        /// </summary>
        /// <param name="spell"></param>
        /// <param name="time"></param>
        /// <returns>입력한 쿨타임 보다 많이 남았다면 True</returns>
        public static bool IsCoolDown(Spell spell, float time)
        {
            return (spell.Level >= 1 && spell.CooldownTime > time) || spell.Level == 0;
        }

        /// <summary>
        /// 거미폼인지 확인
        /// </summary>
        /// <returns>거미폼이라면 True</returns>
        public static bool IsSpider()
        {
            return Player.GetSpell(SpellSlot.R).Name.ToLower().Contains("spider");
        }
        
        private static void Orbwalker_OnBeforeAttack(object sender, BeforeAttackEventArgs e)
        {
            JungleClear.CastE();
        }

        private static void Orbwalker_OnAfterAttack(object sender, AfterAttackEventArgs e)
        {
            Combo.CastW2();
            JungleClear.CastW2();
        }

        private static void GameOnUpdate(EventArgs args)
        {
            if(Orbwalker.CanAttack())
            {
                switch (Orbwalker.ActiveMode)
                {
                    case OrbwalkerMode.Combo:
                        Combo.BasicLogic();
                        break;
                    case OrbwalkerMode.Harass:
                        for (int i = 0; i < Player.Buffs.Length; i++) Console.WriteLine(i + " : " + Player.Buffs[i].Name);
                        break;
                    case OrbwalkerMode.LaneClear:
                        JungleClear.BasicLogic();
                        LaneClear.OnLoad();
                        break;
                    case OrbwalkerMode.LastHit:
                        LastHit.OnLoad();
                        break;
                    case OrbwalkerMode.Flee:
                        break;
                }
            }
        }

    }
}
