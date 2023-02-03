using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnsoulSharp;
using EnsoulSharp.SDK;
using EnsoulSharp.SDK.MenuUI;
using EnsoulSharp.SDK.Rendering;
using EnsoulSharp.SDK.Rendering.Caches;
using Newtonsoft.Json;
using SharpDX;

namespace RankerAIO.Common
{
    class Base : RankerCommon
    {
        public static bool Korean { get; set; }

        public static Spell Q, W, E, R;
        public static Spell Q2, W2, E2, R2;
        public static Spell Q3, W3, E3, R3;

        public static Menu ChampionMenu;

        public static AIHeroClient Player = GameObjects.Player;
    }
}
