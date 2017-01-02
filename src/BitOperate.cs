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
    /// 値型に対してビットまたはバイト単位での操作を提供します。
    /// </summary>
    public static unsafe class BitOperate
    {
        #region -- Public Static Methods --
        /// <summary>
        /// 実行中のマシンのバイト順がリトルエンディアンであるかを表す真偽値を表します。この変数は読み取り専用です。
        /// </summary>
        public readonly static bool IsLittleEndian = BitConverter.IsLittleEndian;

        /// <summary>
        /// 実行中のマシンのバイト順がビッグエンディアンであるかを表す真偽値を表します。この変数は読み取り専用です。
        /// </summary>
        public readonly static bool IsBigEndian = !BitConverter.IsLittleEndian;

        /// <summary>
        /// 指定された数値をリトルエンディアンに並べ替えます。
        /// </summary>
        /// <returns>リトルエンディアンで表された変換後の <see cref="long"/> 型の値。</returns>
        /// <param name="value">変換前の <see cref="long"/> 型の値。</param>
        public static long ToLittleEndian(this long value)
        {
            if (IsBigEndian)
                return value.ReverseBytes();

            return value;
        }

        /// <summary>
        /// 指定された数値をリトルエンディアンに並べ替えます。
        /// </summary>
        /// <returns>リトルエンディアンで表された変換後の <see cref="int"/> 型の値。</returns>
        /// <param name="value">変換前の <see cref="int"/> 型の値。</param>
        public static int ToLittleEndian(this int value)
        {
            if (IsBigEndian)
                return value.ReverseBytes();

            return value;
        }

        /// <summary>
        /// 指定された数値をリトルエンディアンに並べ替えます。
        /// </summary>
        /// <returns>リトルエンディアンで表された変換後の <see cref="short"/> 型の値。</returns>
        /// <param name="value">変換前の <see cref="short"/> 型の値。</param>
        public static short ToLittleEndian(this short value)
        {
            if (IsBigEndian)
                return value.ReverseBytes();

            return value;
        }

        /// <summary>
        /// 指定された数値をビッグエンディアンに並べ替えます。
        /// </summary>
        /// <returns>ビッグエンディアンで表された変換後の <see cref="long"/> 型の値。</returns>
        /// <param name="value">変換前の <see cref="long"/> 型の値。</param>
        public static long ToBigEndian(this long value)
        {
            if (IsLittleEndian)
                return value.ReverseBytes();

            return value;
        }

        /// <summary>
        /// 指定された数値をビッグエンディアンに並べ替えます。
        /// </summary>
        /// <returns>ビッグエンディアンで表された変換後の <see cref="int"/> 型の値。</returns>
        /// <param name="value">変換前の <see cref="int"/> 型の値。</param>
        public static int ToBigEndian(this int value)
        {
            if (IsLittleEndian)
                return value.ReverseBytes();

            return value;
        }

        /// <summary>
        /// 指定された数値をビッグエンディアンに並べ替えます。
        /// </summary>
        /// <returns>ビッグエンディアンで表された変換後の <see cref="short"/> 型の値。</returns>
        /// <param name="value">変換前の <see cref="short"/> 型の値。</param>
        public static short ToBigEndian(this short value)
        {
            if (IsLittleEndian)
                return value.ReverseBytes();

            return value;
        }

        /// <summary>
        /// 指定された値のバイトオーダを逆転させます。
        /// </summary>
        /// <param name="value">逆転されるデータ値。</param>
        /// <returns>逆転されたデータ値。</returns>
        public static long ReverseBytes(this long value)
        {
            byte* x = stackalloc byte[8];
            var bp = (byte*)&value;
            x[0] = bp[7];
            x[1] = bp[6];
            x[2] = bp[5];
            x[3] = bp[4];
            x[4] = bp[3];
            x[5] = bp[2];
            x[6] = bp[1];
            x[7] = bp[0];

            return *(long*)x;
        }

        /// <summary>
        /// 指定された値のバイトオーダを逆転させます。
        /// </summary>
        /// <param name="value">逆転されるデータ値。</param>
        /// <returns>逆転されたデータ値。</returns>
        public static int ReverseBytes(this int value)
        {
            byte* x = stackalloc byte[4];
            var bp = (byte*)&value;
            x[0] = bp[3];
            x[1] = bp[2];
            x[2] = bp[1];
            x[3] = bp[0];

            return *(int*)x;
        }

        /// <summary>
        /// 指定された値のバイトオーダを逆転させます。
        /// </summary>
        /// <param name="value">逆転されるデータ値。</param>
        /// <returns>逆転されたデータ値。</returns>
        public static short ReverseBytes(this short value)
        {
            byte* b0 = (byte*)&value, b1 = b0 + 1;
            return (short)(*b0 * 256 + *b1);
        }
        #endregion
    }
}
