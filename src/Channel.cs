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
    /// インターリーブされた配列に対する操作を提供します。
    /// </summary>
    public static class Channel
    {
        #region -- Public Static Methods --
        public static void Split<T>(T[] source, T[] lch, T[] rch)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (lch == null)
                throw new ArgumentNullException("lch");

            if (rch == null)
                throw new ArgumentNullException("rch");

            if (source.Length % 2 == 1)
                throw new ArgumentException();

            if (source.Length / 2 > lch.Length)
                throw new ArgumentException();

            if (source.Length / 2 > rch.Length)
                throw new ArgumentException();

            for (int i = 0, j = 0; i < source.Length / 2; i++)
            {
                lch[i] = source[j++];
                rch[i] = source[j++];
            }
        }

        public static void Join<T>(T[] lch, T[] rch, T[] dest)
        {
            if (lch == null)
                throw new ArgumentNullException("lch");

            if (rch == null)
                throw new ArgumentNullException("rch");

            if (dest == null)
                throw new ArgumentNullException("dest");

            if (lch.Length * 2 > dest.Length)
                throw new ArgumentException();

            if (rch.Length * 2 > dest.Length)
                throw new ArgumentException();

            if (lch.Length + rch.Length > dest.Length)
                throw new ArgumentException();

            for (int i = 0, j = 0; i < dest.Length; j++)
            {
                dest[i++] = lch[j];
                dest[i++] = rch[j];
            }
        }

        #region Interleave
        public static void Interleave<T>(T[] src, T[] dest, int count)
        {
            Interleave(src, 0, dest, 0, count);
        }

        public static void Interleave<T>(T[] src, int srcOffset, T[] dest, int dest_offset, int count)
        {
            if (src == null)
                throw new ArgumentNullException("src");

            if (dest == null)
                throw new ArgumentNullException("dest");

            if (srcOffset < 0 || srcOffset >= src.Length)
                throw new ArgumentOutOfRangeException("srcOffset");

            if (dest_offset < 0 || dest_offset >= dest.Length)
                throw new ArgumentOutOfRangeException("dest_offset");

            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            if ((src.Length - srcOffset) < count / 2 ||
                (dest.Length - dest_offset) < count)
                throw new ArgumentOutOfRangeException("count");

            for (int i = 0, j = srcOffset, k = dest_offset; i < count; i++, j++, k++)
            {
                dest[k] = src[j];
                dest[++k] = default(T);
            }
        }

        public static void Interleave<T>(T[] srcR, T[] srcI, T[] dest, int count)
        {
            Interleave(srcR, 0, srcI, 0, dest, 0, count);
        }

        public static void Interleave<T>(T[] srcR, int srcROffset, T[] srcI, int srcIOffset, T[] dest, int dest_offset, int count)
        {
            if (srcR == null)
                throw new ArgumentNullException("srcR");

            if (srcI == null)
                throw new ArgumentNullException("srcI");

            if (dest == null)
                throw new ArgumentNullException("dest");

            if (srcROffset < 0 || srcROffset >= srcR.Length)
                throw new ArgumentOutOfRangeException("srcROffset");

            if (srcIOffset < 0 || srcIOffset >= srcI.Length)
                throw new ArgumentOutOfRangeException("srcIOffset");

            if (dest_offset < 0 || dest_offset >= dest.Length)
                throw new ArgumentOutOfRangeException("dest_offset");

            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            if ((srcR.Length - srcROffset) < count / 2 ||
                (srcI.Length - srcIOffset) < count / 2 ||
                (dest.Length - dest_offset) < count)
                throw new ArgumentOutOfRangeException("count");

            for (int i = 0, j = srcROffset, k = srcIOffset, l = dest_offset; i < count; i++, j++, k++)
            {
                dest[l++] = srcR[j];
                dest[l++] = srcI[k];
            }
        }
        #endregion

        #region Deinterleave
        public static void Deinterleave<T>(T[] src, T[] dest, int count)
        {
            Deinterleave(src, 0, dest, 0, count);
        }

        public static void Deinterleave<T>(T[] src, int srcOffset, T[] dest, int dest_offset, int count)
        {
            if (src == null)
                throw new ArgumentNullException("src");

            if (dest == null)
                throw new ArgumentNullException("dest");

            if (srcOffset < 0 || srcOffset >= src.Length)
                throw new ArgumentOutOfRangeException("srcOffset");

            if (dest_offset < 0 || dest_offset >= dest.Length)
                throw new ArgumentOutOfRangeException("dest_offset");

            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            if ((src.Length - srcOffset) < count ||
                (dest.Length - dest_offset) * 2 < count)
                throw new ArgumentOutOfRangeException("count");

            for (int i = 0, j = srcOffset, k = dest_offset; i < count; i++, j += 2, k++)
                dest[k] = src[j];
        }

        public static void Deinterleave<T>(T[] src, T[] destR, T[] destI, int count)
        {
            Deinterleave(src, 0, destR, 0, destI, 0, count);
        }

        public static void Deinterleave<T>(T[] src, int srcOffset, T[] destR, int destR_offset, T[] destI, int destI_offset, int count)
        {
            if (src == null)
                throw new ArgumentNullException("src");

            if (destR == null)
                throw new ArgumentNullException("destR");

            if (destI == null)
                throw new ArgumentNullException("destI");

            if (srcOffset < 0 || srcOffset >= src.Length)
                throw new ArgumentOutOfRangeException("srcOffset");

            if (destR_offset < 0 || destR_offset >= destR.Length)
                throw new ArgumentOutOfRangeException("destR_offset");

            if (destI_offset < 0 || destI_offset >= destI.Length)
                throw new ArgumentOutOfRangeException("destI_offset");

            if (count < 0)
                throw new ArgumentOutOfRangeException("count");

            if ((src.Length - srcOffset) < count * 2 ||
                (destR.Length - destR_offset) < count ||
                (destI.Length - destI_offset) < count)
                throw new ArgumentOutOfRangeException("count");

            for (int i = 0, j = srcOffset, k = destR_offset, l = destI_offset; i < count; i++, k++, l++)
            {
                destR[k] = src[j++];
                destI[l] = src[j++];
            }
        }
        #endregion
        #endregion
    }
}
