/* SoundUtils

LICENSE - The MIT License (MIT)

Copyright (c) 2014 Tomona Nanase

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
    /// 信号処理に用いられる汎用的な数学関数を提供します。
    /// </summary>
    public static class SoundMath
    {
        #region -- Public Static Methods --
        /// <summary>
        /// Sinc 関数の値を取得します。
        /// </summary>
        /// <param name="x">x の値。</param>
        /// <returns>Sinc(x) の結果。</returns>
        public static double Sinc(double x)
        {
            return x == 0.0 ? 1.0 : Math.Sin(x) / x;
        }

        /// <summary>
        /// 第1種0次変形ベッセル関数の値を取得します。
        /// </summary>
        /// <param name="x">x の値。</param>
        /// <param name="iterate">近似計算の反復回数。</param>
        /// <returns>計算結果。</returns>
        public static double Bessel0(double x, int iterate = 100)
        {
            if (x < 0.0)
                throw new ArgumentOutOfRangeException(nameof(x));

            if (iterate < 0)
                throw new ArgumentOutOfRangeException(nameof(iterate));

            var y = 0.0;

            for (var i = 0; i < iterate; i++)
            {
                var f = InvertedFactorial(i);

                y += (f * f) * Math.Pow(x / 2.0, 2.0 * i);
            }

            return y;
        }

        /// <summary>
        /// n の階乗を逆数で計算します。
        /// </summary>
        /// <param name="n">階乗の整数パラメータ。</param>
        /// <returns>n の階乗の逆数の値。</returns>
        public static double InvertedFactorial(int n)
        {
            var y = 1.0;

            while (n > 1)
            {
                y *= 1.0 / n;
                n--;
            }

            return y;
        }
        #endregion
    }
}
