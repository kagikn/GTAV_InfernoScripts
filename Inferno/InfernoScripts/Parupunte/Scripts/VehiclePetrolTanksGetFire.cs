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
    class VehiclePetrolTanksGetFire : ParupunteScript
    {
        public VehiclePetrolTanksGetFire(ParupunteCore core) : base(core)
        {
        }

        public override string Name => "周辺車両一斉発火";

        public override void OnStart()
        {
            var PlayerAroundDistance = 100f;
            var vehicles = core.CachedVehicles.Where(x => x.IsSafeExist()
                                                       && (x.Position - core.PlayerPed.Position).Length() <= PlayerAroundDistance);

            foreach(Vehicle vehicle in vehicles)
            {
                vehicle.PetrolTankHealth = -1;
            }

            ParupunteEnd();
        }
    }
}
