using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnsoulSharp;
using EnsoulSharp.SDK;
using EnsoulSharp.SDK.MenuUI;
using RankerAIO.Common;

namespace RankerAIO
{
    class Program : Base
    {
        static void Main(string[] args)
        {
            GameEvent.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad()
        {
            switch (GameObjects.Player.CharacterName)
            {
                case "Ezreal":
                    Champion.Ezreal.Ezreal.Init();
                    break;
                case "Elise":
                    Champion.Elise.Elise.Init();
                    break;
                default:
                    //Game.Print("Not Support : {0}", GameObjects.Player.CharacterName);
                    Champion.Test.Test.Init();
                    break;
            }
        }

    }
}
