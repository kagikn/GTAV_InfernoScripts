﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA;
using GTA.Math;
using GTA.Native;
using Inferno.ChaosMode;

namespace Inferno
{
    /// <summary>
    /// 市民を生成してパラシュート降下させる
    /// </summary>
    internal class SpawnParachuteCitizenArmy : InfernoScript
    {

        protected override void Setup()
        {
            CreateInputKeywordAsObservable("carmy")
                .Subscribe(_ =>
                {
                    IsActive = !IsActive;
                    DrawText("SpawnParachuteCitizenArmy:" + IsActive, 3.0f);
                });

            OnAllOnCommandObservable.Subscribe(_ => IsActive = true);

            CreateTickAsObservable(5000)
                .Where(_ => IsActive)
                .Subscribe(_ => CreateParachutePed());
        }

        private void CreateParachutePed()
        {
            if(!playerPed.IsSafeExist())return;
            var playerPosition = playerPed.Position;

            var velocity = playerPed.Velocity;
            //プレイヤが移動中ならその進行先に生成する
            var ped =
                NativeFunctions.CreateRandomPed(playerPosition + 3*velocity + new Vector3(0, 0, 50).AroundRandom2D(50));
            
            if(!ped.IsSafeExist()) return;

            ped.MarkAsNoLongerNeeded();
            ped.Task.ClearAllImmediately();
            
            //プレイヤ周囲15mを目標に降下
            var targetPosition = playerPosition.AroundRandom2D(15);
            ped.ParachuteTo(targetPosition);

            //着地までカオス化させない
            StartCoroutine(PedOnGroundedCheck(ped));
        }

        /// <summary>
        /// 市民が着地するまで監視する
        /// </summary>
        /// <param name="ped"></param>
        /// <returns></returns>
        IEnumerable<Object>  PedOnGroundedCheck(Ped ped)
        {
            //市民無敵化
            ped.IsInvincible = true;
            ped.SetNotChaosPed(true);
            for (var i = 0; i < 10; i++)
            {
                yield return WaitForSeconds(1);

                //市民が消えていたり死んでたら監視終了
                if(!ped.IsSafeExist()) yield break;
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
            }
        }

    }
}
