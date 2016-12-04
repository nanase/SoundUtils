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
    /// 周波数帯域を通過させるバンドパスフィルタです。
    /// </summary>
    public class BandPassFilter : BandFilter
    {
        #region -- Protected Methods --
        /// <summary>
        /// 指定された配列に size だけの長さでインパルス応答を生成します。
        /// </summary>
        /// <param name="array">インパルス応答が生成される配列。</param>
        /// <param name="size">生成される長さ。</param>
        protected override void GenerateValues(double[] array, int size)
        {
            int offset = size / 2;
            double feL_2 = 2.0 * (FrequencyLow / SamplingRate);
            double feH_2 = 2.0 * (FrequencyHigh / SamplingRate);
            double feL_PI2 = Math.PI * feL_2;
            double feH_PI2 = Math.PI * feH_2;

            for (int i = 0, j = -offset; i < size && j <= offset; i++, j++)
                array[i] = feH_2 * SoundMath.Sinc(feH_PI2 * j) - feL_2 * SoundMath.Sinc(feL_PI2 * j);
        }
        #endregion
    }
}
