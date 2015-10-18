using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GTA;
using GTA.Native;
using GTA.Math;

namespace Inferno.InfernoScripts.Parupunte.Scripts
{
    class HumanBomb : ParupunteScript
    {
        private ReduceCounter reduceCounter;

        public HumanBomb(ParupunteCore core) : base(core)
        {
        }

        public override string Name => "人間爆弾";

        public override void OnStart()
        {
            reduceCounter = new ReduceCounter(10000);
            AddProgressBar(reduceCounter);
            StartCoroutine(HumanBombCoroutine());
        }

        HashSet<int> explodedPedHandles = new HashSet<int>();

        private IEnumerable<object> HumanBombCoroutine()
        {
            var player = core.PlayerPed;

            //10秒間死んだ市民がバクハツシサンする
            while(!reduceCounter.IsCompleted)
            {
                var peds = core.CachedPeds.Where(x => x.IsSafeExist()
                                   && !x.IsSameEntity(core.PlayerPed)
                                   && !x.IsRequiredForMission());

                explodedPedHandles.Where(x => Function.Call<bool>(Hash.DOES_ENTITY_EXIST, x));

                foreach (Ped ped in peds)
                {
                    if (explodedPedHandles.Contains(ped.Handle)) { break; }

                    if (ped.IsDead)
                    {
                        var killer = ped.GetKiller();
                        if (killer == null || !killer.Exists()) { killer = ped; }

                        GTA.World.AddOwnedExplosion(ped, ped.Position, GTA.ExplosionType.Rocket, 8.0f, 2.5f);

                        if (ped.IsRequiredForMission())
                        {
                            explodedPedHandles.Add(ped.Handle);
                            ped.IsVisible = false;
                        }
                        else
                        {
                            ped.Delete();
                        }
                    }
                }

                yield return null;
            }

            ParupunteEnd();
        }
    }
}
