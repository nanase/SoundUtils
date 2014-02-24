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
    /// 配列に対する変換処理メソッド群を提供します。
    /// </summary>
    public static class ArrayConvert
    {
        #region -- Public Static Methods --
        #region CastToDouble
        /// <summary>
        /// 配列を System.Double 型に変換します。
        /// </summary>
        /// <param name="src">変換元の配列。</param>
        /// <param name="dst">変換先の配列。</param>
        public static void CastToDouble(float[] src, double[] dst)
        {
            if (src == null)
                throw new ArgumentNullException("src");

            if (dst == null)
                throw new ArgumentNullException("dst");

            if (src.Length != dst.Length)
                throw new ArgumentOutOfRangeException("dst");

            for (int i = 0, l = src.Length; i < l; i++)
                dst[i] = (double)src[i];
        }
        #endregion

        #region CastToSingle
        /// <summary>
        /// 配列を System.Single 型に変換します。
        /// </summary>
        /// <param name="src">変換元の配列。</param>
        /// <param name="dst">変換先の配列。</param>
        public static void CastToSingle(double[] src, float[] dst)
        {
            if (src == null)
                throw new ArgumentNullException("src");

            if (dst == null)
                throw new ArgumentNullException("dst");

            if (src.Length != dst.Length)
                throw new ArgumentOutOfRangeException("dst");

            for (int i = 0, l = src.Length; i < l; i++)
                dst[i] = (float)src[i];
        }
        #endregion

        #region ToByte
        unsafe public static void ToByte(short[] src, int offset, int count, byte[] dst, bool reverse = false)
        {
            short tmp;
            byte* b0 = (byte*)&tmp, b1 = b0 + 1;

            if (reverse)
                for (int i = offset, j = 0, length = offset + count; i < length; i++)
                {
                    tmp = src[i];
                    dst[j++] = *b0;
                    dst[j++] = *b1;
                }
            else
                for (int i = offset, j = 0, length = offset + count; i < length; i++)
                {
                    tmp = src[i];
                    dst[j++] = *b1;
                    dst[j++] = *b0;
                }
        }
        #endregion
        #endregion
    }
}
