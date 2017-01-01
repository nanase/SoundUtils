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

namespace SoundUtils.Filtering.IIR
{
    /// <summary>
    /// コムフィルタを生成するクラスです。
    /// </summary>
    public class CombFilter : InfiniteImpulseResponse
    {
        #region -- Public Properties --
        /// <summary>
        /// 遅延器の数を取得または設定します。
        /// </summary>
        public double Delay { get; set; }

        /// <summary>
        /// 増幅度を取得または設定します。
        /// </summary>
        public double Amplifier { get; set; }
        #endregion

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

            var progress = Delay;

            for (int i = 0, j = 0; i < size; j++)
            {
                var value = Math.Ceiling(progress);
                var alpha = 1.0 + progress - value;
                var beta = 1.0 - alpha;
                var amp = Math.Pow(Amplifier, j);

                i = (int)value - 1;
                progress += Delay;

                if (i < size)
                {
                    array[i] += alpha * amp;

                    if (i + 1 < size)
                        array[i + 1] += beta * amp;
                    else break;                    
                }
                else break;
            }
        }
        #endregion
    }
}
