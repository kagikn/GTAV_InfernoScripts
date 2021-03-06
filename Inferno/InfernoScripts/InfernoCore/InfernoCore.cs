﻿using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows.Forms;
using GTA;
using GTA.Native;


namespace Inferno
{
    /// <summary>
    /// インフェルノスクリプト中で統一して使う機能や処理を押し込めたもの
    /// </summary>
    public sealed class InfernoCore : Script
    {
        private DebugLogger _debugLogger;

        public static InfernoCore Instance { get; private set; }

        private static readonly Subject<Unit> OnTickSubject = new Subject<Unit>();
        private static readonly Subject<KeyEventArgs> OnKeyDownSubject = new Subject<KeyEventArgs>();

        private readonly BehaviorSubject<Ped[]> _pedsNearPlayer = new BehaviorSubject<Ped[]>(default(Ped[]));
        /// <summary>
        /// 周辺市民
        /// </summary>
        public IObservable<Ped[]> PedsNearPlayer => _pedsNearPlayer.AsObservable();

        private readonly BehaviorSubject<Vehicle[]> _vehiclesNearPlayer = new BehaviorSubject<Vehicle[]>(default(Vehicle[]));
        /// <summary>
        /// 周辺車両
        /// </summary>
        public IObservable<Vehicle[]> VehicleNearPlayer => _vehiclesNearPlayer.AsObservable();

        private BehaviorSubject<Ped> playerPed = new BehaviorSubject<Ped>(default(Ped));

        public IObservable<Ped> PlayerPed => playerPed.AsObservable(); 

        /// <summary>
        /// 25ms周期のTick
        /// </summary>
        public static IObservable<Unit> OnTickAsObservable => OnTickSubject.AsObservable();

        /// <summary>
        /// キー入力
        /// </summary>
        public static IObservable<KeyEventArgs> OnKeyDownAsObservable => OnKeyDownSubject.AsObservable();

        public InfernoCore()
        {
            Instance = this;

            _debugLogger = new DebugLogger(@"InfernoScript.log");

            //100ms周期でイベントを飛ばす
            Interval = 100;
            Observable.FromEventPattern<EventHandler, EventArgs>(h => h.Invoke, h => Tick += h, h => Tick -= h)
                .Select(_ => Unit.Default)
                .Multicast(OnTickSubject)
                .Connect();

            //キー入力
            Observable.FromEventPattern<KeyEventHandler, KeyEventArgs>(h => h.Invoke, h => KeyDown += h,
                h => KeyDown -= h)
                .Select(e => e.EventArgs)
                .Multicast(OnKeyDownSubject)
                .Connect();


            //市民と車両の更新
            OnTickAsObservable
                .Subscribe(_ => UpdatePedsAndVehiclesList());
        }

        /// <summary>
        /// 市民と車両のキャッシュ
        /// </summary>
        private void UpdatePedsAndVehiclesList()
        {
            try
            {
                var player = Game.Player;
                var ped = player?.Character;
                if (!ped.IsSafeExist()) return;
                playerPed.OnNext(ped);
                _pedsNearPlayer.OnNext(World.GetNearbyPeds(ped, 500));
                _vehiclesNearPlayer.OnNext(World.GetNearbyVehicles(ped, 500));
            }
            catch (Exception e)
            {
                LogWrite(e.StackTrace);
            }
        }


        public void LogWrite(string message)
        {
            _debugLogger.Log(message);
        }
    }
}
