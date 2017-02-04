﻿/* SoundUtils

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
        /// <param name="src">変換元の <see cref="float"/> 型の配列。</param>
        /// <param name="dst">変換先の <see cref="float"/> 型の配列。</param>
        public static void ToDouble(this float[] src, double[] dst)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst == null)
                throw new ArgumentNullException(nameof(dst));

            if (src.Length != dst.Length)
                throw new ArgumentOutOfRangeException(nameof(dst));

            for (int i = 0, l = src.Length; i < l; i++)
                dst[i] = src[i];
        }
        #endregion

        #region ToSingle
        /// <summary>
        /// 配列を <see cref="float"/> 型に変換します。
        /// </summary>
        /// <param name="src">変換元の <see cref="double"/> 型の配列。</param>
        /// <param name="dst">変換先の <see cref="float"/> 型の配列。</param>
        public static void ToSingle(this double[] src, float[] dst)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dst == null)
                throw new ArgumentNullException(nameof(dst));

            if (src.Length != dst.Length)
                throw new ArgumentOutOfRangeException(nameof(dst));

            for (int i = 0, l = src.Length; i < l; i++)
                dst[i] = (float)src[i];
        }
        #endregion

        #region ToByte
        /// <summary>
        /// <see cref="short"/> 型の配列の値を <see cref="byte"/> 型に変換します。
        /// </summary>
        /// <param name="src">変換される <see cref="short"/> 型配列。</param>
        /// <param name="dst">変換された値が格納される <see cref="byte"/> 型配列。</param>
        /// <param name="reverse">バイトオーダを判定するかどうかの真偽値。</param>
        public static void ToByte(this short[] src, byte[] dst, bool reverse = false)
        {
            ToByte(src, 0, src.Length, dst, reverse);
        }

        /// <summary>
        /// <see cref="short"/> 型の配列の値を <see cref="byte"/> 型に変換します。
        /// </summary>
        /// <param name="src">変換される <see cref="short"/> 型配列。</param>
        /// <param name="offset">読み取りが開始されるインデックスのオフセット。</param>
        /// <param name="count">読み取られる要素数。</param>
        /// <param name="dst">変換された値が格納される <see cref="byte"/> 型配列。</param>
        /// <param name="reverse">バイトオーダを判定するかどうかの真偽値。</param>
        public static unsafe void ToByte(this short[] src, int offset, int count, byte[] dst, bool reverse = false)
        {
            short tmp;
            byte* b0 = (byte*)&tmp, b1 = b0 + 1;

            for (int i = offset, j = 0, length = offset + count; i < length; i++)
            {
                tmp = src[i];

                if (reverse)
                {
                    dst[j++] = *b0;
                    dst[j++] = *b1;
                }
                else
                {
                    dst[j++] = *b1;
                    dst[j++] = *b0;
                }
            }
        }
        #endregion

        #region RegulateAsInt8
        public static void RegulateAsInt8(this short[] src, byte[] dst)
        {
            RegulateAsInt8(src, 0, src.Length, dst);
        }

        public static void RegulateAsInt8(this short[] src, int offset, int count, byte[] dst)
        {
            for (int i = offset, j = 0, length = offset + count; i < length; i++, j++)
                dst[j] = (byte)Math.Round((src[i] / 65536.0 + 0.5) * 255);
        }

        public static void RegulateAsInt8(this float[] src, byte[] dst)
        {
            RegulateAsInt8(src, 0, src.Length, dst);
        }

        public static void RegulateAsInt8(this float[] src, int offset, int count, byte[] dst)
        {
            for (int i = offset, j = 0, length = offset + count; i < length; i++, j++)
            {
                var dtmp = src[i];

                if (float.IsNaN(dtmp) || float.IsInfinity(dtmp))
                    dst[j] = 127;
                else if (dtmp > 1.0f)
                    dst[j] = 255;
                else if (dtmp < -1.0f)
                    dst[j] = 0;
                else
                    dst[j] = (byte)Math.Round((dtmp + 1.0f) * 127.5f);
            }
        }

        public static void RegulateAsInt8(this double[] src, byte[] dst)
        {
            RegulateAsInt8(src, 0, src.Length, dst);
        }

        public static void RegulateAsInt8(this double[] src, int offset, int count, byte[] dst)
        {
            for (int i = offset, j = 0, length = offset + count; i < length; i++, j++)
            {
                var dtmp = src[i];

                if (double.IsNaN(dtmp) || double.IsInfinity(dtmp))
                    dst[j] = 127;
                else if (dtmp > 1.0)
                    dst[j] = 255;
                else if (dtmp < -1.0)
                    dst[j] = 0;
                else
                    dst[j] = (byte)Math.Round((dtmp + 1.0) * 127.5);
            }
        }
        #endregion

        #region RegulateAsInt16
        public static void RegulateAsInt16(this float[] src, byte[] dst, bool reverse = false)
        {
            RegulateAsInt16(src, 0, src.Length, dst, reverse);
        }

        public static unsafe void RegulateAsInt16(this float[] src, int offset, int count, byte[] dst, bool reverse = false)
        {
            short tmp;
            byte* b0 = (byte*)&tmp, b1 = b0 + 1;

            for (int i = offset, j = 0, length = offset + count; i < length; i++)
            {
                var dtmp = src[i];

                if (float.IsNaN(dtmp) || float.IsInfinity(dtmp))
                {
                    j += 2;
                    continue;
                }

                if (dtmp > 1.0f)
                    tmp = short.MaxValue;
                else if (dtmp < -1.0f)
                    tmp = short.MinValue;
                else
                    tmp = (short)(dtmp * 32767.5f);

                if (reverse)
                {
                    dst[j++] = *b0;
                    dst[j++] = *b1;
                }
                else
                {
                    dst[j++] = *b1;
                    dst[j++] = *b0;
                }
            }
        }

        public static void RegulateAsInt16(this double[] src, byte[] dst, bool reverse = false)
        {
            RegulateAsInt16(src, 0, src.Length, dst, reverse);
        }

        public static unsafe void RegulateAsInt16(this double[] src, int offset, int count, byte[] dst, bool reverse = false)
        {
            short tmp;
            byte* b0 = (byte*)&tmp, b1 = b0 + 1;

            for (int i = offset, j = 0, length = offset + count; i < length; i++)
            {
                var dtmp = src[i];

                if (double.IsNaN(dtmp) || double.IsInfinity(dtmp))
                {
                    j += 2;
                    continue;
                }

                if (dtmp > 1.0)
                    tmp = short.MaxValue;
                else if (dtmp < -1.0)
                    tmp = short.MinValue;
                else
                    tmp = (short)(dtmp * 32767.5);

                if (reverse)
                {
                    dst[j++] = *b0;
                    dst[j++] = *b1;
                }
                else
                {
                    dst[j++] = *b1;
                    dst[j++] = *b0;
                }
            }
        }
        #endregion
        #endregion
    }
}
