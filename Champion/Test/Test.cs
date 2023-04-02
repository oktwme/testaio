using EnsoulSharp;
using EnsoulSharp.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XerathExploit
{
    class Program
    {
        private static AIHeroClient Player;
        private static Spell Q, W, E, R;

        static void Main(string[] args)
        {
            GameEvent.OnGameLoad += OnLoad;
        }

        private static void OnLoad()
        {
            Player = ObjectManager.Player;
            Q = new Spell(SpellSlot.Q, 1600f);
            W = new Spell(SpellSlot.W, 1000f);
            E = new Spell(SpellSlot.E, 1050f);
            R = new Spell(SpellSlot.R, 5600f);

            Q.SetSkillshot(0.6f, 100f, 1000f, false, SpellType.Line);
            W.SetSkillshot(0.25f, 250f, float.MaxValue, false, SpellType.Circle);
            E.SetSkillshot(0.25f, 60f, 1400f, true, SpellType.Line);
            R.SetSkillshot(0.7f, 100f, float.MaxValue, false, SpellType.Circle);

            EnsoulSharp.SDK.Events.Tick.OnTick += OnTick;
        }

        private static void OnTick(EventArgs args)
        {
            if (Player.IsDead) return;
            if (Orbwalker.ActiveMode == OrbwalkerMode.None) return;

            var target = TargetSelector.GetTarget(Q.Range);
            if (target == null) return;

            var qPrediction = Q.GetPrediction(target);
            if (qPrediction.Hitchance >= HitChance.High)
            {
                Q.Cast(qPrediction.CastPosition);
            }

            var wPrediction = W.GetPrediction(target);
            if (wPrediction.Hitchance >= HitChance.High)
            {
                W.Cast(wPrediction.CastPosition);
            }

            var ePrediction = E.GetPrediction(target);
            if (ePrediction.Hitchance >= HitChance.High)
            {
                E.Cast(ePrediction.CastPosition);
            }

            var rPrediction = R.GetPrediction(target);
            if (rPrediction.Hitchance >= HitChance.High)
            {
                R.Cast(rPrediction.CastPosition);
            }
        }
    }
}
