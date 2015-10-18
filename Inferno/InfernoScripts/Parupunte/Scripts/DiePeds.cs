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
    class DiePeds : ParupunteScript
    {
        public DiePeds(ParupunteCore core) : base(core)
        {
        }

        public override string Name => "市民全員即死";

        public override void OnStart()
        {
            var PlayerAroundDistance = 100f;
            var peds = core.CachedPeds.Where(x => x.IsSafeExist()
                                               && !x.IsSameEntity(core.PlayerPed)
                                               && !x.IsRequiredForMission()
                                               && (x.Position - core.PlayerPed.Position).Length() <= PlayerAroundDistance);

            foreach(Ped ped in peds)
            {
                ped.Kill();
            }

            ParupunteEnd();
        }
    }
}
