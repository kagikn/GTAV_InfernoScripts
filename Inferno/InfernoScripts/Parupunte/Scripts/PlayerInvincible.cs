using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using GTA;
using GTA.Math;
using GTA.Native;
using Inferno.ChaosMode;
using Inferno.Utilities;


namespace Inferno.InfernoScripts.Parupunte.Scripts
{
    class PlayerInvincible : ParupunteScript
    {
        private ReduceCounter reduceCounter;

        public PlayerInvincible(ParupunteCore core) : base(core)
        {
        }

        public override string Name => "無敵";

        public override void OnStart()
        {
            reduceCounter = new ReduceCounter(30000);
            AddProgressBar(reduceCounter);
            StartCoroutine(InvincibleCoroutine());
        }

        private IEnumerable<object> InvincibleCoroutine()
        {
            var player = core.PlayerPed;

            while (!reduceCounter.IsCompleted)
            {
                //30秒無敵を維持
                while(!reduceCounter.IsCompleted)
                {
                    player.IsInvincible = true;
                    yield return null;
                }
            }
            
            player.IsInvincible = false;

            ParupunteEnd();            
        }
    }
}
