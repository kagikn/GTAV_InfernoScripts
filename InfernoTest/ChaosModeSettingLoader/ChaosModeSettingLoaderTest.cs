﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Inferno;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Inferno.ChaosMode;

namespace InfernoTest
{
    /// <summary>
    /// ChaosModeSettingLoaderTest の概要の説明
    /// </summary>
    [TestClass]
    public class ChaosModeSettingLoaderTest
    {
        [TestMethod]
        public void 全て正常値が設定されたJosonからChaosSettingが生成できる()
        {
            var testLoader =
                new TestLoader(
                    "{\"AttackPlayerCorrectionProbabillity\":3,\"DefaultMissionCharacterTreatment\":2,\"Interval\":50,\"IsAttackPlayerCorrectionEnabled\":true,\"IsChangeMissionCharacterWeapon\":false,\"IsStupidShooting\":false,\"Radius\":100,\"ShootAccuracy\":60,\"WeaponList\":[\"RPG\",\"BAT\"]}");

            var result = testLoader.LoadSettingFile("");

            Assert.AreEqual(100,result.Radius);
            Assert.AreEqual(3,result.AttackPlayerCorrectionProbabillity);
            Assert.AreEqual(MissionCharacterTreatmentType.ExcludeAllMissionCharacter,result.DefaultMissionCharacterTreatment);
            Assert.AreEqual(50,result.Interval);
            Assert.AreEqual(60,result.ShootAccuracy);
            Assert.IsTrue(result.IsAttackPlayerCorrectionEnabled);
            Assert.IsFalse(result.IsStupidShooting);
            Assert.IsFalse(result.IsChangeMissionCharacterWeapon);
            CollectionAssert.AreEqual(new Weapon[] {Weapon.BAT, Weapon.RPG}, result.WeaponList);
        }

        [TestMethod]
        public void 一部パラメータが抜けていてもデフォルト値で上書きされたChaosSettingが生成できる()
        {
            //AttackPlayerCorrectionProbabillityとDefaultMissionCharacterTreatmentが未設定
            var testLoader =
                new TestLoader("{\"Interval\":50,\"IsAttackPlayerCorrectionEnabled\":true,\"IsChangeMissionCharacterWeapon\":false,\"IsStupidShooting\":false,\"Radius\":100,\"ShootAccuracy\":60,\"WeaponList\":[\"RPG\",\"BAT\"]}");

            var result = testLoader.LoadSettingFile("");

            //設定されていない要素はデフォルト値
            Assert.AreEqual(100, result.AttackPlayerCorrectionProbabillity);
            Assert.AreEqual(MissionCharacterTreatmentType.ExcludeUniqueCharacter, result.DefaultMissionCharacterTreatment);

            //それ以外はjsonの通り
            Assert.AreEqual(100, result.Radius);
            Assert.AreEqual(50, result.Interval);
            Assert.AreEqual(60, result.ShootAccuracy);
            Assert.IsTrue(result.IsAttackPlayerCorrectionEnabled);
            Assert.IsFalse(result.IsStupidShooting);
            Assert.IsFalse(result.IsChangeMissionCharacterWeapon);
            CollectionAssert.AreEqual(new Weapon[] { Weapon.BAT, Weapon.RPG }, result.WeaponList);
        }

        [TestMethod]
        public void 不正なjsonが渡された場合はデフォルト値の設定ファイルになる()
        {
            var testLoader = new TestLoader("{AAA,BBB}"); //jsonの文法違反
            var result = testLoader.LoadSettingFile("");
            Assert.AreEqual(1000, result.Radius);
            Assert.AreEqual(100, result.AttackPlayerCorrectionProbabillity);
            Assert.AreEqual(MissionCharacterTreatmentType.ExcludeUniqueCharacter, result.DefaultMissionCharacterTreatment);
            Assert.AreEqual(500, result.Interval);
            Assert.AreEqual(100, result.ShootAccuracy);
            Assert.IsFalse(result.IsAttackPlayerCorrectionEnabled);
            Assert.IsTrue(result.IsStupidShooting);
            Assert.IsTrue(result.IsChangeMissionCharacterWeapon);
            var allWeapons = ((Weapon[])Enum.GetValues(typeof(Weapon)));
            CollectionAssert.AreEqual(allWeapons, result.WeaponList);
        }

        [TestMethod]
        public void jsonの型が一致しない場合はデフォルト値の設定ファイルになる()
        {
            var testLoader =
                new TestLoader(
                    "{\"AttackPlayerCorrectionProbabillity\":true,\"DefaultMissionCharacterTreatment\":\"hoge\",\"Interval\":50,\"IsAttackPlayerCorrectionEnabled\":true,\"IsChangeMissionCharacterWeapon\":false,\"IsStupidShooting\":false,\"Radius\":100,\"ShootAccuracy\":60,\"WeaponList\":[\"RPG\",\"BAT\"]}");

            var result = testLoader.LoadSettingFile("");
            Assert.AreEqual(1000, result.Radius);
            Assert.AreEqual(100, result.AttackPlayerCorrectionProbabillity);
            Assert.AreEqual(MissionCharacterTreatmentType.ExcludeUniqueCharacter,
                result.DefaultMissionCharacterTreatment);
            Assert.AreEqual(500, result.Interval);
            Assert.AreEqual(100, result.ShootAccuracy);
            Assert.IsFalse(result.IsAttackPlayerCorrectionEnabled);
            Assert.IsTrue(result.IsStupidShooting);
            Assert.IsTrue(result.IsChangeMissionCharacterWeapon);
            var allWeapons = ((Weapon[]) Enum.GetValues(typeof (Weapon)));
            CollectionAssert.AreEqual(allWeapons, result.WeaponList);
        }

        [TestMethod]
        public void 空文字列だった場合はデフォルト値のChaosSettingが生成される()
        {
            var testLoader = new TestLoader("");
            var result = testLoader.LoadSettingFile("");
            Assert.AreEqual(1000, result.Radius);
            Assert.AreEqual(100, result.AttackPlayerCorrectionProbabillity);
            Assert.AreEqual(MissionCharacterTreatmentType.ExcludeUniqueCharacter, result.DefaultMissionCharacterTreatment);
            Assert.AreEqual(500, result.Interval);
            Assert.AreEqual(100, result.ShootAccuracy);
            Assert.IsFalse(result.IsAttackPlayerCorrectionEnabled);
            Assert.IsTrue(result.IsStupidShooting);
            Assert.IsTrue(result.IsChangeMissionCharacterWeapon);
            var allWeapons = ((Weapon[])Enum.GetValues(typeof(Weapon)));
            CollectionAssert.AreEqual(allWeapons, result.WeaponList);
        }

        /// <summary>
        /// テスト用ローダー
        /// </summary>
        private class TestLoader : ChaosModeSettingLoader
        {
            private readonly string _readJson;

            public TestLoader(string readJson)
            {
                _readJson = readJson;
            }

            protected override string ReadFile(string filePath)
            {
                return _readJson;
            }
        }
    }
}

