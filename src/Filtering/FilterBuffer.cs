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
    /// データ型 T の配列に対する固定バッファを提供します。
    /// </summary>
    /// <typeparam name="T">バッファとして確保されるデータ型。</typeparam>
    public class FilterBuffer<T>
    {
        #region -- Private Fields --
        private readonly int length;
        private readonly T[] data;
        private readonly Action<T[]> action;
        private int index;
        #endregion

        #region -- Public Properties --
        /// <summary>
        /// バッファに対する生の配列を取得します。
        /// </summary>
        public T[] Data { get { return this.data; } }

        /// <summary>
        /// バッファの長さを取得します。
        /// </summary>
        public int Length { get { return this.length; } }
        #endregion

        #region -- Constructors --
        /// <summary>
        /// バッファの長さと実行される処理を指定して新しい FilterBuffer クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="length">バッファの長さ。</param>
        /// <param name="action">バッファが length に達した時に実行される処理。</param>
        public FilterBuffer(int length, Action<T[]> action)
        {
            this.length = length;
            this.data = new T[length];
            this.action = action;
        }
        #endregion

        #region -- Public Methods --
        /// <summary>
        /// バッファにデータを格納します。
        /// </summary>
        /// <param name="input">格納されるデータ型 T の配列。</param>
        public void Push(T[] input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            int input_index = 0;

            while (input_index < input.Length)
            {
                int copy_length = Math.Min(this.length - this.index, input.Length - input_index);

                Array.Copy(input, input_index, this.data, this.index, copy_length);

                this.index += copy_length;
                input_index += copy_length;

                if (this.index + 1 >= this.length)
                {
                    this.action(this.data);
                    this.index = 0;
                }
            }
        }

        /// <summary>
        /// バッファのデータ格納を終了し、足りない領域をクリアして指定された処理を実行します。
        /// </summary>
        public void Close()
        {
            if (this.index > 0)
            {
                Array.Clear(this.data, this.index, this.length - this.index);
                this.action(this.data);

                this.index = 0;
            }
        }
        #endregion
    }
}
