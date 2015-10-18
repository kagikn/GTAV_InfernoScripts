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
    class MolotovParadise : ParupunteScript
    {
        private ReduceCounter reduceCounter;

        public MolotovParadise(ParupunteCore core) : base(core)
        {
        }

        public override string Name => "火の海";

        public override void OnStart()
        {
            reduceCounter = new ReduceCounter(2000);
            AddProgressBar(reduceCounter);
            StartCoroutine(MolotovParadiseCoroutine());
        }

        private IEnumerable<object> MolotovParadiseCoroutine()
        {
            var player = Game.Player.Character;
            //だいたい0.1秒ごとに2秒間火炎瓶の爆発が発生し続ける
            while(!reduceCounter.IsCompleted)
            {
                if (!player.IsSafeExist()) { yield return null; }

                var pos = player.Position.AroundRandom2D(50f);
                Function.Call(Hash._ADD_SPECFX_EXPLOSION, pos.X, pos.Y, pos.Z, -1, core.GetGTAObjectHashKey("EXP_VFXTAG_MOLOTOV"), 3f, false, false, 0f); //ADD_EXPLOSIONでは爆発ダメージが発生してしまう

                yield return null;
            }

            ParupunteEnd();
        } 
    }
}
