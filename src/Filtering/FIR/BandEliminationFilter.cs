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

namespace SoundUtils.Filtering.FIR
{
    /// <summary>
    /// 周波数帯域を遮断させるバンドイリミネーションフィルタです。
    /// </summary>
    public class BandEliminationFilter : BandFilter
    {
        #region -- Protected Methods --
        /// <summary>
        /// 指定された配列に size だけの長さでインパルス応答を生成します。
        /// </summary>
        /// <param name="array">インパルス応答が生成される配列。</param>
        /// <param name="size">生成される長さ。</param>
        protected override void GenerateValues(double[] array, int size)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (size < 0 || size > array.Length)
                throw new ArgumentOutOfRangeException(nameof(size));

            var offset = size / 2;
            var feL2 = 2.0 * (FrequencyLow / SamplingRate);
            var feH2 = 2.0 * (FrequencyHigh / SamplingRate);
            var feLPi2 = Math.PI * feL2;
            var feHPi2 = Math.PI * feH2;

            for (int i = 0, j = -offset; i < size && j <= offset; i++, j++)
                array[i] = SoundMath.Sinc(Math.PI * j) -
                           feH2 * SoundMath.Sinc(feHPi2 * j) +
                           feL2 * SoundMath.Sinc(feLPi2 * j);
        }
        #endregion
    }
}
