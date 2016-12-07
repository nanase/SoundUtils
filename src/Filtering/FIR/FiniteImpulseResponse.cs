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

namespace SoundUtils.Filtering.FIR
{
    /// <summary>
    /// 有限インパルス応答を生成するクラスを抽象化します。
    /// </summary>
    public abstract class FiniteImpulseResponse : ImpulseResponse
    {
        #region -- Public Methods --
        /// <summary>
        /// 指定されたデルタ値からインパルス応答を生成します。
        /// </summary>
        /// <param name="delta">デルタ値。</param>
        /// <returns>インパルス応答が格納された配列。</returns>
        public double[] Generate(double delta)
        {
            return base.Generate(GetFilterSize(SamplingRate, delta));
        }

        /// <summary>
        /// 指定されたデルタ値からインパルス応答を生成します。
        /// </summary>
        /// <param name="array">インパルス応答が生成される配列。</param>
        /// <param name="delta">デルタ値。</param>
        public void Generate(double[] array, double delta)
        {
            base.Generate(array, GetFilterSize(SamplingRate, delta));
        }
        #endregion

        #region -- Public Static Methods --
        /// <summary>
        /// サンプリング周波数と遅延器の数からデルタ値を計算します。
        /// </summary>
        /// <param name="samplingRate">サンプリング周波数。</param>
        /// <param name="delayer">遅延器の数。</param>
        /// <returns>デルタ値。</returns>
        public static double GetDelta(double samplingRate, int delayer)
        {
            // 奇数で返す
            if ((delayer & 1) == 0)
                delayer--;

            return (3.1 / (delayer - 0.5)) * samplingRate;
        }

        /// <summary>
        /// サンプリング周波数とデルタ値からフィルタサイズ (遅延器の数) を計算します。
        /// </summary>
        /// <param name="samplingRate">サンプリング周波数。</param>
        /// <param name="delta">デルタ値。</param>
        /// <returns>フィルタサイズ。</returns>
        public static int GetFilterSize(double samplingRate, double delta)
        {
            delta /= samplingRate;
            var delayer = (int)(3.1 / delta + 0.5) - 1;

            // 偶数で返す
            if ((delayer & 1) == 1)
                delayer++;

            return delayer;
        }
        #endregion
    }
}
