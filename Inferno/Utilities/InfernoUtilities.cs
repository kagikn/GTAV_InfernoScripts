﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GTA.Math;

namespace Inferno.Utilities
{
    //便利関数群
    public static class InfernoUtilities
    {
        /// <summary>
        /// ランダムな方向のベクトルを生成する
        /// </summary>
        public static Vector3 CreateRandomVector()
        {
            var random = new Random();
            var x = random.NextDouble() - 0.5;
            var y = random.NextDouble() - 0.5;
            var z = random.NextDouble() - 0.5;
            var randomVector = new Vector3((float)x, (float)y, (float)z);
            randomVector.Normalize();
            return randomVector;
        }
    }
}
