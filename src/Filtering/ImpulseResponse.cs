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

namespace SoundUtils.Filtering
{
    /// <summary>
    /// インパルスに対する応答を生成する抽象クラスを定義します。
    /// </summary>
    public abstract class ImpulseResponse
    {
        #region -- Public Properties --
        /// <summary>
        /// サンプリング周波数。
        /// </summary>
        public double SamplingRate { get; set; }
        #endregion

        #region -- Public Methods --
        /// <summary>
        /// 指定された長さの配列からインパルス応答を生成します。
        /// </summary>
        /// <param name="length">インパルス応答の長さ。</param>
        /// <returns>インパルス応答が格納された配列。</returns>
        public double[] Generate(int length)
        {
            double[] array = new double[length];

            this.GenerateValues(array, length);

            return array;
        }

        /// <summary>
        /// 指定された配列にインパルス応答を生成します。
        /// </summary>
        /// <param name="array">インパルス応答が生成される配列。</param>
        public void Generate(double[] array)
        {
            int length = array.Length;

            this.GenerateValues(array, length);
        }

        /// <summary>
        /// 指定された配列にインパルス応答を生成します。
        /// </summary>
        /// <param name="array">インパルス応答が生成される配列。</param>
        /// <param name="length">生成される長さ。</param>
        public void Generate(double[] array, int length)
        {
            if (array.Length < length)
                throw new ArgumentOutOfRangeException("length");

            int arrayLength = array.Length;

            this.GenerateValues(array, Math.Min(arrayLength, length));
        }
        #endregion

        #region -- Protected Methods --
        /// <summary>
        /// 指定された配列に size だけの長さでインパルス応答を生成します。
        /// </summary>
        /// <param name="array">インパルス応答が生成される配列。</param>
        /// <param name="size">生成される長さ。</param>
        protected abstract void GenerateValues(double[] array, int size);
        #endregion
    }
}
