using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using Inferno.ChaosMode;
using Inferno.Utilities;


namespace Inferno.InfernoScripts.Parupunte.Scripts
{
    class HealthAndArmorHealing : ParupunteScript
    {
        public HealthAndArmorHealing(ParupunteCore core) : base(core)
        {
        }

        public override string Name => "全回復";

        public override void OnStart()
        {
            var player = core.PlayerPed;

            var maxHealth = Game.Player.Character.MaxHealth;
            var maxArmor = Game.Player.GetPlayerMaxArmor();

            player.Health = maxHealth;
            player.Armor = maxArmor;

            ParupunteEnd();
        }
    }
}
