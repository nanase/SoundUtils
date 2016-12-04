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
    /// 指定された周波数帯域に作用するインパルス応答を抽象化します。
    /// </summary>
    public abstract class BandFilter : FiniteImpulseResponse
    {
        #region -- Public Properties --
        /// <summary>
        /// 周波数帯域の低い値を取得または設定します。
        /// </summary>
        public double FrequencyLow { get; set; }

        /// <summary>
        /// 周波数帯域の高い値を取得または設定します。
        /// </summary>
        public double FrequencyHigh { get; set; }
        #endregion

        #region -- Public Methods --
        /// <summary>
        /// 中心周波数と帯域幅を指定して周波数帯域を設定します。
        /// </summary>
        /// <param name="centerFrequecy">中心周波数。</param>
        /// <param name="bandwidth">帯域幅。</param>
        public void SetFrequency(double centerFrequecy, double bandwidth)
        {
            FrequencyLow = centerFrequecy - bandwidth / 2.0;
            FrequencyHigh = centerFrequecy + bandwidth / 2.0;
        }
        #endregion
    }
}
