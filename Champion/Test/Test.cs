using EnsoulSharp;
using EnsoulSharp.SDK;
using EnsoulSharp.SDK.Utility;

namespace MyPlugin
{
    public class Plugin
    {
        public Plugin()
        {
            GameEvent.OnGameLoad += OnLoad;
        }

        private void OnLoad()
        {
            Chat.Print("MyPlugin loaded!");

            GameEvent.OnUpdate += OnUpdate;
        }

        private void OnUpdate(EventArgs args)
        {
            var player = ObjectManager.Player;

            if (player.ChampionName == "Ezreal")
            {
                var target = TargetSelector.GetTarget(player.Spellbook.GetSpell(SpellSlot.Q).SData.CastRange);

                if (target != null)
                {
                    var prediction = Prediction.GetPrediction(player.Spellbook.GetSpell(SpellSlot.Q), target);

                    if (prediction.Hitchance >= HitChance.High)
                    {
                        var packet = Packet.C2S.Cast.Create(player.Spellbook.GetSpell(SpellSlot.Q).Slot, prediction.CastPosition);
                        packet.Send();
                    }
                }
            }
        }
    }
}
