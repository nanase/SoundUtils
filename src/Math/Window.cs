﻿/* SoundUtils

LICENSE - The MIT License (MIT)

Copyright (c) 2017 Tomona Nanase

Permission is hereby granted, free of charge, to any person obtaining a copy of
this software and associated documentation files (the "Software"), to deal in
the Software without restriction, including without limitation the rights to
use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System;

namespace SoundUtils
{
    /// <summary>
    /// 各種の窓関数を畳み込むためのメソッド群を提供します。
    /// </summary>
    public static class Window
    {
        #region -- Public Static Methods --
        /// <summary>
        /// 配列にハン窓を畳み込みます。
        /// </summary>
        /// <param name="array">畳み込まれる配列。</param>
        public static void Hanning(double[] array)
        {
            CheckArrayIsNotNull(array);

            var length = array.Length;
            var factor = 2.0 * Math.PI / length;
            var k = (length & 1) == 0 ? 0.0 : 0.5;

            for (var i = 0; i < length; i++, k++)
                array[i] *= 0.5 - 0.5 * Math.Cos(factor * k);
        }

        /// <summary>
        /// 配列にハミング窓を畳み込みます。
        /// </summary>
        /// <param name="array">畳み込まれる配列。</param>
        public static void Hamming(double[] array)
        {
            CheckArrayIsNotNull(array);

            var length = array.Length;
            var factor = 2.0 * Math.PI / length;
            var k = (length & 1) == 0 ? 0.0 : 0.5;

            for (var i = 0; i < length; i++, k++)
                array[i] *= 0.54 - 0.46 * Math.Cos(factor * k);
        }

        /// <summary>
        /// 配列にバートレット窓を畳み込みます。
        /// </summary>
        /// <param name="array">畳み込まれる配列。</param>
        public static void Bartlett(double[] array)
        {
            CheckArrayIsNotNull(array);

            var length = array.Length;
            var k = (length & 1) == 0 ? 0.0 : 0.5;
            var n = 1.0 / length;

            for (var i = 0; i < length; i++, k++)
                array[i] *= 1.0 - 2.0 * Math.Abs(k * n - 0.5);
        }

        /// <summary>
        /// 配列にナットール窓を畳み込みます。
        /// </summary>
        /// <param name="array">畳み込まれる配列。</param>
        public static void Nuttall(double[] array)
        {
            CheckArrayIsNotNull(array);

            var length = array.Length;
            var factor = 2.0 * Math.PI / length;
            var k = (length & 1) == 0 ? 0.0 : 0.5;

            for (var i = 0; i < length; i++, k++)
                array[i] *= 0.355768 -
                            0.487396 * Math.Cos(factor * k) +
                            0.144232 * Math.Cos(2.0 * factor * k) -
                            0.012604 * Math.Cos(3.0 * factor * k);
        }

        /// <summary>
        /// 配列にブラックマン窓を畳み込みます。
        /// </summary>
        /// <param name="array">畳み込まれる配列。</param>
        public static void Blackman(double[] array)
        {
            CheckArrayIsNotNull(array);

            var length = array.Length;
            var factor = 2.0 * Math.PI / length;
            var k = (length & 1) == 0 ? 0.0 : 0.5;

            for (var i = 0; i < length; i++, k++)
                array[i] *= 0.42 - 0.5 * Math.Cos(factor * k) + 0.08 * Math.Cos(2.0 * factor * k);
        }

        /// <summary>
        /// 配列にブラックマンハリス窓を畳み込みます。
        /// </summary>
        /// <param name="array">畳み込まれる配列。</param>
        public static void BlackmanHarris(double[] array)
        {
            CheckArrayIsNotNull(array);

            var length = array.Length;
            var factor = 2.0 * Math.PI / length;
            var k = (length & 1) == 0 ? 0.0 : 0.5;

            for (var i = 0; i < length; i++, k++)
                array[i] *= 0.35875 -
                            0.48829 * Math.Cos(factor * k) +
                            0.14128 * Math.Cos(2.0 * factor * k) -
                            0.01168 * Math.Cos(3.0 * factor * k);
        }

        /// <summary>
        /// 配列にブラックマンナットール窓を畳み込みます。
        /// </summary>
        /// <param name="array">畳み込まれる配列。</param>
        public static void BlackmanNuttall(double[] array)
        {
            CheckArrayIsNotNull(array);

            var length = array.Length;
            var factor = 2.0 * Math.PI / length;
            var k = (length & 1) == 0 ? 0.0 : 0.5;

            for (var i = 0; i < length; i++, k++)
                array[i] *= 0.3635819 -
                            0.4891775 * Math.Cos(factor * k) +
                            0.1365995 * Math.Cos(2.0 * factor * k) -
                            0.0106411 * Math.Cos(3.0 * factor * k);
        }

        /// <summary>
        /// 配列にフラットトップ窓を畳み込みます。
        /// </summary>
        /// <param name="array">畳み込まれる配列。</param>
        public static void FlatTop(double[] array)
        {
            CheckArrayIsNotNull(array);

            var length = array.Length;
            var factor = 2.0 * Math.PI / length;
            var k = (length & 1) == 0 ? 0.0 : 0.5;

            for (var i = 0; i < length; i++, k++)
                array[i] *= 1 -
                            1.93 * Math.Cos(factor * k) +
                            1.29 * Math.Cos(2.0 * factor * k) -
                            0.388 * Math.Cos(3.0 * factor * k) +
                            0.032 * Math.Cos(4.0 * factor * k);
        }

        /// <summary>
        /// 配列にウェルチ窓を畳み込みます。
        /// </summary>
        /// <param name="array">畳み込まれる配列。</param>
        public static void Welch(double[] array)
        {
            CheckArrayIsNotNull(array);

            var length = array.Length;
            var factor = 1.0 / length;
            var k = (length & 1) == 0 ? 0.0 : 0.5;

            for (var i = 0; i < length; i++, k++)
                array[i] *= 4.0 * (factor * k) * (1.0 - factor * k);
        }

        /// <summary>
        /// 配列にカイザー窓を畳み込みます。
        /// </summary>
        /// <param name="array">畳み込まれる配列。</param>
        /// <param name="alpha">カイザー窓の alpha 係数。</param>
        public static void Kaiser(double[] array, double alpha)
        {
            CheckArrayIsNotNull(array);

            var length = array.Length;
            var n = 2.0 / length;
            var k = (length & 1) == 0 ? 0.0 : 0.5;

            if (Math.Abs(alpha) < double.Epsilon)
            {
                Array.Clear(array, 0, length);
                return;
            }

            alpha *= Math.PI;

            var alphaTmp = 1.0 / SoundMath.Bessel0(alpha);

            for (var i = 0; i < length; i++, k++)
                array[i] *= SoundMath.Bessel0(alpha * Math.Sqrt(1.0 - Math.Pow(k * n - 1.0, 2.0))) * alphaTmp;
        }
        #endregion

        #region -- Private Static Methods --

        private static void CheckArrayIsNotNull<T>(T[] array)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
        }

        #endregion
    }
}
