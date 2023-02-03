using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnsoulSharp;
using EnsoulSharp.SDK;
using EnsoulSharp.SDK.MenuUI;
using RankerAIO.Common;

namespace RankerAIO.Champion.Test
{
    class Test : Base
    {
        public static void Init()
        {
            ChampionMenu = new Menu("Test", "Test", true).Attach();
            Korean = ChampionMenu.Add(new MenuBool("Korean", "Korean")).Enabled;

            Game.OnUpdate += Game_OnUpdate;
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if(Orbwalker.ActiveMode == OrbwalkerMode.Harass)
            {
                for (int i = 0; i < Player.Buffs.Length; i++) Console.WriteLine(i + " : " + Player.Buffs[i].Name);

                //Console.WriteLine(Player.HasBuff("SivirE"));
            }
        }
    }
}
