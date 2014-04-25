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
using SoundUtils.Filtering.FIR;

namespace SoundUtils.Filtering
{
    /// <summary>
    /// 波形をローパスフィルタを用いてオーバーサンプリングするための機能を提供します。
    /// </summary>
    public class OverSampling
    {
        #region -- Private Fields --
        private readonly double samplingRate;
        private readonly bool stereo;
        private readonly int magnification, filterSize;

        private readonly SoundFilter filter;
        #endregion

        #region -- Constructors --
        /// <summary>
        /// パラメータを指定して新しい OverSampling クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="samplingRate">サンプリング周波数。これはオーバーサンプリング後の周波数となります。</param>
        /// <param name="magnification">サンプリング倍率。</param>
        /// <param name="stereo">ステレオである場合は true、モノラルである場合は false。</param>
        /// <param name="filterSize">フィルタサイズ。</param>
        public OverSampling(double samplingRate, int magnification, bool stereo, int filterSize)
        {
            if (samplingRate <= 0.0)
                throw new ArgumentOutOfRangeException("samplingRate");

            if (magnification <= 0)
                throw new ArgumentOutOfRangeException("magnification");

            if (filterSize <= 0)
                throw new ArgumentOutOfRangeException("filterSize");

            if (filterSize % 2 != 0)
                throw new ArgumentException();

            this.samplingRate = samplingRate;
            this.magnification = magnification;
            this.stereo = stereo;
            this.filterSize = filterSize;

            this.filter = new SoundFilter(stereo, filterSize);

            var filterGenerator = new LowPassFilter()
            {
                SamplingRate = samplingRate * magnification,
                CutoffFrequency = samplingRate / 2 -
                                  FiniteImpulseResponse.GetDelta(samplingRate * magnification, filterSize)
            };
            double[] impulse = filterGenerator.Generate(filterSize / 2);

            Window.Blackman(impulse);
            filter.SetFilter(impulse);
        }
        #endregion

        #region -- Public Methods --
        /// <summary>
        /// 指定された波形にオーバーサンプリングを実行します。
        /// </summary>
        /// <param name="buffer">オーバーサンプリングが適用される配列。</param>
        /// <returns>オーバーサンプリングされたデータ数。</returns>
        public int Apply(double[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");

            if (this.magnification == 1)
                return this.filterSize;

            filter.Filtering(buffer);

            for (int i = 0, j = 0, inc = this.magnification * (this.stereo ? 2 : 1); i < filterSize; i += inc)
            {
                buffer[j++] = buffer[i];
                buffer[j++] = buffer[i + 1];
            }

            return this.filterSize / this.magnification;
        }
        #endregion
    }
}
