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
    class Base
    {
        public static bool Korean { get; set; }

        public static Spell Q, W, E, R;
        public static Spell Q2, W2, E2, R2;
        public static Spell Q3, W3, E3, R3;

        public static Menu ChampionMenu;

        public static AIHeroClient Player = GameObjects.Player;

        public static bool IsSummonerReady(AIHeroClient hero, string name)
        {
            return (hero.GetSpell(SpellSlot.Summoner1).Name.ToLower().Contains(name) && hero.GetSpell(SpellSlot.Summoner1).IsReady()) ||
                   (hero.GetSpell(SpellSlot.Summoner2).Name.ToLower().Contains(name) && hero.GetSpell(SpellSlot.Summoner2).IsReady()) ?
                   true : false;
        }

        public static bool IsSafe()
        {
            List<bool> IsSafe = new List<bool>();

            foreach (var item in GameObjects.EnemyHeroes.OrderBy(x => x.DistanceToPlayer()))
            {
                List<float> ranges = new List<float>();
                ranges.Add(item.AttackRange);
                ranges.Add(item.GetSpell(SpellSlot.Q).SData.CastRange);
                ranges.Add(item.GetSpell(SpellSlot.W).SData.CastRange);
                ranges.Add(item.GetSpell(SpellSlot.E).SData.CastRange);

                float flashRange = 0;
                if (IsSummonerReady(item, "flash")) flashRange = 400;

                if (item.DistanceToPlayer() > (ranges.Max() + item.MoveSpeed + flashRange))
                {
                    IsSafe.Add(true);
                }
                else IsSafe.Add(false);
            }

            return IsSafe.Where(x => x == false).Count() == 0 ? true : false;
        }

        public static List<AIHeroClient> GetCCEnemys()
        {
            List<AIHeroClient> count = new List<AIHeroClient>();
            foreach (var item in GameObjects.EnemyHeroes.OrderBy(x => x.DistanceToPlayer()))
            {
                if (item.DistanceToPlayer() < GameObjects.Player.GetRealAutoAttackRange(item)) return new List<AIHeroClient>();

                if (item.IsGrounded || item.IsSuppressed ||
                    item.IsTaunted  || item.IsCharmed    ||
                    item.IsFleeing  || item.IsAsleep     ||
                    item.IsFeared   || item.IsStunned     ) count.Add(item);
            }

            return count;
        }

        public static List<AIHeroClient> GetObjectInRangeEnemys(AIMinionClient obj, float range = 650)
        {
            return GameObjects.EnemyHeroes.Where(x => x.Distance(obj) < range).ToList();
        }

        public static float GetObjectTotalDamage(List<AIHeroClient> enemys, AIMinionClient target)
        {
            float total = 0;
            foreach (var item in enemys)
            {
                total += (float)item.GetAutoAttackDamage(target);
                if (item.GetSpell(SpellSlot.Q).IsReady()) total += (float)item.GetSpellDamage(target, SpellSlot.Q);
                if (item.GetSpell(SpellSlot.W).IsReady()) total += (float)item.GetSpellDamage(target, SpellSlot.W);
                if (item.GetSpell(SpellSlot.E).IsReady()) total += (float)item.GetSpellDamage(target, SpellSlot.E);
            }

            return total;
        }

        public static float GetMoveSpeedByDelay(float moveSpeed, Spell spell)
        {
            return (moveSpeed / spell.Speed + 1) * (moveSpeed * spell.Delay);
        }

    }
}
