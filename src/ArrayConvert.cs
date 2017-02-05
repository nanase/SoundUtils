/* SoundUtils

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
    /// 配列に対する変換処理メソッド群を提供します。
    /// </summary>
    public static class ArrayConvert
    {
        #region -- Public Static Methods --
        #region ToDouble

        /// <summary>
        /// 配列を <see cref="double"/> 型に変換します。
        /// </summary>
        /// <param name="source">変換元の <see cref="float"/> 型の配列。</param>
        /// <param name="destination">変換先の <see cref="float"/> 型の配列。</param>
        public static void ToDouble(this float[] source, double[] destination)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (destination == null)
                throw new ArgumentNullException(nameof(destination));

            if (source.Length != destination.Length)
                throw new ArgumentOutOfRangeException(nameof(destination));

            for (int i = 0, length = source.Length; i < length; i++)
                destination[i] = source[i];
        }

        #endregion

        #region ToSingle

        /// <summary>
        /// 配列を <see cref="float"/> 型に変換します。
        /// </summary>
        /// <param name="source">変換元の <see cref="double"/> 型の配列。</param>
        /// <param name="destination">変換先の <see cref="float"/> 型の配列。</param>
        public static void ToSingle(this double[] source, float[] destination)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (destination == null)
                throw new ArgumentNullException(nameof(destination));

            if (source.Length != destination.Length)
                throw new ArgumentOutOfRangeException(nameof(destination));

            for (int i = 0, length = source.Length; i < length; i++)
                destination[i] = (float)source[i];
        }

        #endregion

        #region ToByte

        /// <summary>
        /// <see cref="short"/> 型の配列の値を <see cref="byte"/> 型に変換します。
        /// </summary>
        /// <param name="source">変換される <see cref="short"/> 型配列。</param>
        /// <param name="destination">変換された値が格納される <see cref="byte"/> 型配列。</param>
        /// <param name="reverse">バイトオーダを判定するかどうかの真偽値。</param>
        public static void ToByte(this short[] source, byte[] destination, bool reverse = false)
        {
            ToByte(source, 0, source.Length, destination, reverse);
        }

        /// <summary>
        /// <see cref="short"/> 型の配列の値を <see cref="byte"/> 型に変換します。
        /// </summary>
        /// <param name="source">変換される <see cref="short"/> 型配列。</param>
        /// <param name="offset">読み取りが開始されるインデックスのオフセット。</param>
        /// <param name="count">読み取られる要素数。</param>
        /// <param name="destination">変換された値が格納される <see cref="byte"/> 型配列。</param>
        /// <param name="reverse">バイトオーダを判定するかどうかの真偽値。</param>
        public static unsafe void ToByte(
            this short[] source,
            int offset,
            int count,
            byte[] destination,
            bool reverse = false)
        {
            short temp;
            byte* b0 = (byte*)&temp, b1 = b0 + 1;

            for (int i = offset, j = 0, length = offset + count; i < length; i++)
            {
                temp = source[i];

                if (reverse)
                {
                    destination[j++] = *b0;
                    destination[j++] = *b1;
                }
                else
                {
                    destination[j++] = *b1;
                    destination[j++] = *b0;
                }
            }
        }

        #endregion

        #region RegulateAsInt8

        public static void RegulateAsInt8(this short[] source, byte[] destination)
        {
            RegulateAsInt8(source, 0, source.Length, destination);
        }

        public static void RegulateAsInt8(this short[] source, int offset, int count, byte[] destination)
        {
            for (int i = offset, j = 0, length = offset + count; i < length; i++, j++)
                destination[j] = (byte)Math.Round((source[i] / 65536.0 + 0.5) * 255);
        }

        public static void RegulateAsInt8(this float[] source, byte[] destination)
        {
            RegulateAsInt8(source, 0, source.Length, destination);
        }

        public static void RegulateAsInt8(this float[] source, int offset, int count, byte[] destination)
        {
            for (int i = offset, j = 0, length = offset + count; i < length; i++, j++)
            {
                var tempFloat = source[i];

                if (float.IsNaN(tempFloat) || float.IsInfinity(tempFloat))
                    destination[j] = 127;
                else if (tempFloat > 1.0f)
                    destination[j] = 255;
                else if (tempFloat < -1.0f)
                    destination[j] = 0;
                else
                    destination[j] = (byte)Math.Round((tempFloat + 1.0f) * 127.5f);
            }
        }

        public static void RegulateAsInt8(this double[] source, byte[] destination)
        {
            RegulateAsInt8(source, 0, source.Length, destination);
        }

        public static void RegulateAsInt8(this double[] source, int offset, int count, byte[] destination)
        {
            for (int i = offset, j = 0, length = offset + count; i < length; i++, j++)
            {
                var tempDouble = source[i];

                if (double.IsNaN(tempDouble) || double.IsInfinity(tempDouble))
                    destination[j] = 127;
                else if (tempDouble > 1.0)
                    destination[j] = 255;
                else if (tempDouble < -1.0)
                    destination[j] = 0;
                else
                    destination[j] = (byte)Math.Round((tempDouble + 1.0) * 127.5);
            }
        }

        #endregion

        #region RegulateAsInt16

        public static void RegulateAsInt16(this float[] source, byte[] destination, bool reverse = false)
        {
            RegulateAsInt16(source, 0, source.Length, destination, reverse);
        }

        public static unsafe void RegulateAsInt16(
            this float[] source,
            int offset,
            int count,
            byte[] destination,
            bool reverse = false)
        {
            short temp;
            byte* b0 = (byte*)&temp, b1 = b0 + 1;

            for (int i = offset, j = 0, length = offset + count; i < length; i++)
            {
                var tempFloat = source[i];

                if (float.IsNaN(tempFloat) || float.IsInfinity(tempFloat))
                {
                    j += 2;
                    continue;
                }

                if (tempFloat > 1.0f)
                    temp = short.MaxValue;
                else if (tempFloat < -1.0f)
                    temp = short.MinValue;
                else
                    temp = (short)(tempFloat * 32767.5f);

                if (reverse)
                {
                    destination[j++] = *b0;
                    destination[j++] = *b1;
                }
                else
                {
                    destination[j++] = *b1;
                    destination[j++] = *b0;
                }
            }
        }

        public static void RegulateAsInt16(this double[] source, byte[] destination, bool reverse = false)
        {
            RegulateAsInt16(source, 0, source.Length, destination, reverse);
        }

        public static unsafe void RegulateAsInt16(
            this double[] source,
            int offset,
            int count,
            byte[] destination,
            bool reverse = false)
        {
            short temp;
            byte* b0 = (byte*)&temp, b1 = b0 + 1;

            for (int i = offset, j = 0, length = offset + count; i < length; i++)
            {
                var tempDouble = source[i];

                if (double.IsNaN(tempDouble) || double.IsInfinity(tempDouble))
                {
                    j += 2;
                    continue;
                }

                if (tempDouble > 1.0)
                    temp = short.MaxValue;
                else if (tempDouble < -1.0)
                    temp = short.MinValue;
                else
                    temp = (short)(tempDouble * 32767.5);

                if (reverse)
                {
                    destination[j++] = *b0;
                    destination[j++] = *b1;
                }
                else
                {
                    destination[j++] = *b1;
                    destination[j++] = *b0;
                }
            }
        }

        #endregion
        #endregion
    }
}
