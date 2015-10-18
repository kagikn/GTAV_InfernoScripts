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
using Inferno.ChaosMode;

namespace Inferno.InfernoScripts.Parupunte.Scripts
{ 
    class ParachuteCitizenParadise : ParupunteScript
    {
        private ReduceCounter reduceCounter;

        public ParachuteCitizenParadise(ParupunteCore core) : base(core)
        { 
        }

        public override string Name => "パラシュート市民パラダイス";

        public override void OnStart()
        {
            HashSet<object> coroutineIds = new HashSet<object>();
            reduceCounter = new ReduceCounter(4000);
            AddProgressBar(reduceCounter);
            StartCoroutine(ParachuteCitizenParadiseCoroutine());
        }

        private IEnumerable<object> ParachuteCitizenParadiseCoroutine()
        {
            var player = core.PlayerPed;
      
            //4秒間パラシュート市民を0.1秒間隔で生成
            while(!reduceCounter.IsCompleted)
            {
                CreateParachutePed();
                yield return null;
            }

            foreach(object id in coroutineIds)
            {
                if(id != null) { yield return null; }
            }

            ParupunteEnd();
        }

        private void CreateParachutePed()
        {
            if (!core.PlayerPed.IsSafeExist()) return;
            var playerPosition = core.PlayerPed.Position;

            var velocity = core.PlayerPed.Velocity;
            //プレイヤが移動中ならその進行先に生成する
            var ped = GTA.World.CreateRandomPed(playerPosition + 3 * velocity + new Vector3(0, 0, 60).AroundRandom2D(90));

            if (!ped.IsSafeExist()) return;

            ped.Task.ClearAllImmediately();
            ped.MarkAsNoLongerNeeded();

            //プレイヤ周囲15mを目標に降下
            var targetPosition = playerPosition.AroundRandom2D(35);
            ped.ParachuteTo(targetPosition);

            //着地までカオス化させない
            var id = StartCoroutine(PedOnGroundedCheck(ped));
            coroutineIds.Add(id);
        }

        /// <summary>
        /// 市民が着地するまで監視する
        /// </summary>
        /// <param name="ped"></param>
        /// <returns></returns>
        IEnumerable<Object> PedOnGroundedCheck(Ped ped)
        {
            //市民無敵化
            ped.IsInvincible = true;
            ped.SetNotChaosPed(true);
            for (var i = 0; i < 10; i++)
            {
                yield return WaitForSeconds(1);

                //市民が消えていたり死んでたら監視終了
                if (!ped.IsSafeExist()) yield break;
                if (ped.IsDead) yield break;

                //着地していたら監視終了
                if (!ped.IsInAir)
                {
                    break;
                }

            }

            //監視終了後１秒待ってからカオス化許可する(アニメーションがおかしくなるのを避けるため)
            foreach (var s in WaitForSeconds(1.0f))
            {
                yield return s;
            }

            if (ped.IsSafeExist())
            {
                ped.SetNotChaosPed(false);
                ped.IsInvincible = false;
                Function.Call(Hash.SET_PED_TO_RAGDOLL_WITH_FALL, ped, 0, 0, 0, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f);
            }
        }
    }
}
