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
    class RankerCommon
    {
        public enum SummonerType
        {
            Heal = 0,
            Mark = 1,
            Dash = 2,
            Ghost = 3,
            Flash = 4,
            Smite = 5,
            Ignite = 6,
            Barrier = 7,
            Exhaust = 8,
            Clarity = 9,
            Cleanse = 10,
            Teleport = 11,
        }

        public static string GetSummonerName(SummonerType type)
        {
            switch (type)
            {
                case SummonerType.Heal:
                    return "SummonerHeal";
                case SummonerType.Mark:
                    return "SummonerMark";
                case SummonerType.Dash:
                    return "SummonerDash";
                case SummonerType.Ghost:
                    return "SummonerGhost";
                case SummonerType.Smite:
                    return "SummonerSmite";
                case SummonerType.Ignite:
                    return "SummonerIgnite";
                case SummonerType.Barrier:
                    return "SummonerBarrier";
                case SummonerType.Exhaust:
                    return "SummonerExhaust";
                case SummonerType.Clarity:
                    return "SummonerClarity";
                case SummonerType.Cleanse:
                    return "SummonerCleanse";
                case SummonerType.Teleport:
                    return "SummonerTeleport";
                default:
                    return string.Empty;
            }
        }

        public static bool IsSummonerReady(AIHeroClient hero, SummonerType type)
        {
            var spell = hero.GetSpellSlot(GetSummonerName(type));
            return spell != SpellSlot.Unknown && spell.IsReady();
        }

        public static bool IsImmunity(AIHeroClient hero)
        {
            return hero.HasBuff("DrMundoPImmunity") ||
                hero.HasBuff("FioraW") ||
                hero.HasBuff("NocturneShroudofDarkness") ||
                hero.HasBuff("malzaharpassiveshield") ||
                hero.HasBuff("MorganaE") ||
                hero.HasBuff("OlafRagnarok") ||
                hero.HasBuff("SivirE") ||
                hero.HasBuff("SionR");
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
                if (IsSummonerReady(item, SummonerType.Flash)) flashRange = 400;

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
                    item.IsTaunted || item.IsCharmed ||
                    item.IsFleeing || item.IsAsleep ||
                    item.IsFeared || item.IsStunned) count.Add(item);
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
