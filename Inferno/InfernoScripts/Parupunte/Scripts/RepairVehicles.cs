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
    class RepairVehicles : ParupunteScript
    {
        public RepairVehicles(ParupunteCore core) : base(core)
        {
        }

        public override string Name => "車両修復";

        public override void OnStart()
        {
            var PlayerAroundDistance = 100f;
            var vehicles = core.CachedVehicles.Where(x => x.IsSafeExist()
                                                       && (x.Position - core.PlayerPed.Position).Length() <= PlayerAroundDistance);

            foreach(Vehicle vehicle in vehicles)
            {
                vehicle.Repair();
            }

            ParupunteEnd();
        }
    }
}
