using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using GTA;
using GTA.Math;
using GTA.Native;

namespace Inferno
{
    /// <summary>
    /// ふわふわジャンプ
    /// </summary
    class LowGravity : InfernoScript
    {
        
        protected override int TickInterval
        {
            get { return 80; }
        }

        protected override void Setup()
        {
            var player = this.GetPlayer();

            OnTickAsObservable
                    .Where(_ => this.GetPlayer().IsSafeExist() && !player.IsInVehicle()
                                && this.IsGamePadPressed(GameKey.Sprint) && this.IsGamePadPressed(GameKey.Jump))
                    .Subscribe(_ => MakeLowGravity());
        }

        void MakeLowGravity()
        {
            var player = this.GetPlayer(); 
            player.ApplyForce(new Vector3(0.0f, 0.0f, 0.9f)); 
        }
    }
}
