﻿using System.Linq;

namespace Inferno.ChaosMode.WeaponProvider
{
    /// <summary>
    /// 有効な武器一覧を管理する
    /// </summary>
    public class ChaosModeWeapons
    {
        public Weapon[] ShootWeapons { get; }
        public Weapon[] ClosedWeapons { get; }
        public Weapon[] ProjectileWeapons { get; }
        public Weapon[] ExcludeClosedWeapons { get; }
        public Weapon[] DriveByWeapons { get; }
        public Weapon[] AllWeapons { get; }

        public ChaosModeWeapons()
        {
            //射撃系の武器
            ShootWeapons = new[]
            {
                Weapon.ADVANCEDRIFLE,
                Weapon.AIRSTRIKE_ROCKET,
                Weapon.APPISTOL,
                Weapon.ASSAULTRIFLE,
                Weapon.ASSAULTSHOTGUN,
                Weapon.ASSAULTSMG,
                Weapon.BULLPUPSHOTGUN,
                Weapon.BULLPURIFLE,
                Weapon.CARBINERIFLE,
                Weapon.COMBATMG,
                Weapon.COMBATPISTOL,
                Weapon.COUGAR,
                Weapon.EXHAUSTION,
                Weapon.GRENADELAUNCHER,
                Weapon.GRENADELAUNCHER_SMOKE,
                Weapon.HEAVYSNIPER,
                Weapon.HEAVYPISTOL,
                Weapon.HEAVYSHOTGUN,
                Weapon.FIREEXTINGUISHER,
                Weapon.FIREWORK,
                Weapon.MG,
                Weapon.MICROSMG,
                Weapon.MINIGUN,
                Weapon.MUSCKET,
                Weapon.MARKSMANRIFLE,
                Weapon.PISTOL,
                Weapon.PISTOL50,
                Weapon.PUMPSHOTGUN,
                Weapon.RPG,
                Weapon.SAWNOFFSHOTGUN,
                Weapon.SMG,
                Weapon.SNIPERRIFLE,
                Weapon.STINGER,
                Weapon.STUNGUN,
                Weapon.PETROLCAN,
                Weapon.RAILGUN,
                Weapon.FLAREGUN,
                Weapon.MARKSMANPISTOL,
                Weapon.MACHINEPISTOL,
                Weapon.GUSENBERG,
                Weapon.REVOLVER,
                Weapon.COMPACTRIFLE,
                Weapon.DOUBLEBARRELSHOTGUN,
                Weapon.MINISMG,
                Weapon.AUTOSHOTGUN,
                Weapon.COMPACTLAUNCHER,
            };

            //近距離系
            ClosedWeapons = new[]
            {
                Weapon.BARBED_WIRE,
                Weapon.BAT,
                Weapon.CROWBAR,
                Weapon.DROWNING,
                Weapon.HAMMER,
                Weapon.GOLFCLUB,
                Weapon.KNIFE,
                Weapon.NIGHTSTICK,
            };

            //投げる系
            ProjectileWeapons = new[]
            {
                Weapon.BALL,
                Weapon.BZGAS,
                Weapon.GRENADE,
                Weapon.MOLOTOV,
                Weapon.STICKYBOMB,
                Weapon.FLARE,
                Weapon.SMOKEGRENADE,
                Weapon.PROXIMITYMINE,
                Weapon.PIPEBOMB
            };

            //ドライブバイ
            DriveByWeapons = new[]
            {
                Weapon.PISTOL,
                Weapon.APPISTOL,
                Weapon.COMBATPISTOL,
                Weapon.HEAVYPISTOL,
                Weapon.PISTOL50,
                Weapon.FLAREGUN,
                Weapon.REVOLVER,
                Weapon.MICROSMG,
                Weapon.MACHINEPISTOL,
                Weapon.COMPACTRIFLE,
                Weapon.SAWNOFFSHOTGUN,
                Weapon.DOUBLEBARRELSHOTGUN,
                Weapon.STUNGUN,
                Weapon.MINISMG,
                Weapon.AUTOSHOTGUN,
                Weapon.COMPACTLAUNCHER,
            };

            AllWeapons = ShootWeapons.Concat(ClosedWeapons).Concat(ProjectileWeapons).ToArray();
            ExcludeClosedWeapons = ShootWeapons.Concat(ProjectileWeapons).ToArray();
        }
    }
}
