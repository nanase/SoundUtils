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
        /// <summary>
        /// 配列を 2 つのチャネルに分割します。
        /// </summary>
        /// <typeparam name="T">配列のデータ型。</typeparam>
        /// <param name="source">分割される T 型の配列。</param>
        /// <param name="lch">L チャネルに分割される T 型配列。</param>
        /// <param name="rch">R チャネルに分割される T 型配列。</param>
        public static void Split<T>(T[] source, T[] lch, T[] rch)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            Deinterleave(source, lch, rch, source.Length / 2);
        }

        /// <summary>
        /// 2 つのチャネルを 1 つの配列に合成します。
        /// </summary>
        /// <typeparam name="T">配列のデータ型。</typeparam>
        /// <param name="lch">合成元の L チャネルの T 型配列。</param>
        /// <param name="rch">合成元の R チャネルの T 型配列。</param>
        /// <param name="dest">合成先の T 型配列。</param>
        public static void Join<T>(T[] lch, T[] rch, T[] dest)
        {
            if (dest == null)
                throw new ArgumentNullException(nameof(dest));

            Interleave(lch, rch, dest, dest.Length / 2);
        }

        #region Interleave
        /// <summary>
        /// 単一の配列と既定の値をインターリーブします。
        /// </summary>
        /// <typeparam name="T">配列のデータ型。</typeparam>
        /// <param name="src">インターリーブされる T 型配列。</param>
        /// <param name="dest">インターリーブされた結果が格納されるの T 型配列</param>
        /// <param name="count">インターリーブされる配列から読み取られるデータ数。</param>
        public static void Interleave<T>(T[] src, T[] dest, int count)
        {
            Interleave(src, 0, dest, 0, count);
        }

        /// <summary>
        /// 単一の配列と既定の値をインターリーブします。
        /// </summary>
        /// <typeparam name="T">配列のデータ型。</typeparam>
        /// <param name="src">インターリーブされる T 型配列。</param>
        /// <param name="srcOffset">読み取りが開始されるインデックスのオフセット。</param>
        /// <param name="dest">インターリーブされた結果が格納される T 型配列</param>
        /// <param name="destOffset">書き込みが開始されるインデックスのオフセット。</param>
        /// <param name="count">インターリーブされる配列から読み取られるデータ数。</param>
        public static void Interleave<T>(T[] src, int srcOffset, T[] dest, int destOffset, int count)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dest == null)
                throw new ArgumentNullException(nameof(dest));

            if (srcOffset < 0 || srcOffset >= src.Length)
                throw new ArgumentOutOfRangeException(nameof(srcOffset));

            if (destOffset < 0 || destOffset >= dest.Length)
                throw new ArgumentOutOfRangeException(nameof(destOffset));

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (src.Length - srcOffset < count / 2 ||
                dest.Length - destOffset < count)
                throw new ArgumentOutOfRangeException(nameof(count));

            for (int i = 0, j = srcOffset, k = destOffset; i < count; i++, j++, k++)
            {
                dest[k] = src[j];
                dest[++k] = default(T);
            }
        }

        /// <summary>
        /// 2つの配列をインターリーブします。
        /// </summary>
        /// <typeparam name="T">配列のデータ型。</typeparam>
        /// <param name="srcR">1つ目の T 型配列。</param>
        /// <param name="srcI">2つ目の T 型配列。</param>
        /// <param name="dest">インタリーブされた結果が格納される T 型配列。</param>
        /// <param name="count">インターリーブされる配列から読み取られるデータ数。</param>
        public static void Interleave<T>(T[] srcR, T[] srcI, T[] dest, int count)
        {
            Interleave(srcR, 0, srcI, 0, dest, 0, count);
        }

        /// <summary>
        /// 2つの配列をインターリーブします。
        /// </summary>
        /// <typeparam name="T">配列のデータ型。</typeparam>
        /// <param name="srcR">1つ目の T 型配列。</param>
        /// <param name="srcROffset">1つ目の T 型配列の読み取りが開始されるインデックスのオフセット。</param>
        /// <param name="srcI">2つ目の T 型配列。</param>
        /// <param name="srcIOffset">2つ目の T 型配列の読み取りが開始されるインデックスのオフセット。</param>
        /// <param name="dest">インタリーブされた結果が格納される T 型配列。</param>
        /// <param name="destOffset">結果の T 型配列の格納が開始されるインデックスのオフセット。</param>
        /// <param name="count">インターリーブされる配列から読み取られるデータ数。</param>
        public static void Interleave<T>(T[] srcR, int srcROffset, T[] srcI, int srcIOffset, T[] dest, int destOffset, int count)
        {
            if (srcR == null)
                throw new ArgumentNullException(nameof(srcR));

            if (srcI == null)
                throw new ArgumentNullException(nameof(srcI));

            if (dest == null)
                throw new ArgumentNullException(nameof(dest));

            if (srcROffset < 0 || srcROffset >= srcR.Length)
                throw new ArgumentOutOfRangeException(nameof(srcROffset));

            if (srcIOffset < 0 || srcIOffset >= srcI.Length)
                throw new ArgumentOutOfRangeException(nameof(srcIOffset));

            if (destOffset < 0 || destOffset >= dest.Length)
                throw new ArgumentOutOfRangeException(nameof(destOffset));

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (srcR.Length - srcROffset < count / 2 ||
                srcI.Length - srcIOffset < count / 2 ||
                dest.Length - destOffset < count)
                throw new ArgumentOutOfRangeException(nameof(count));

            for (int i = 0, j = srcROffset, k = srcIOffset, l = destOffset; i < count; i++, j++, k++)
            {
                dest[l++] = srcR[j];
                dest[l++] = srcI[k];
            }
        }
        #endregion

        #region Deinterleave
        /// <summary>
        /// 配列のインターリーブを解除し、単一の配列に格納します。
        /// </summary>
        /// <typeparam name="T">配列のデータ型。</typeparam>
        /// <param name="src">インターリーブされた入力となる T 型配列。</param>
        /// <param name="dest">インターリーブ解除の結果が格納される T 型配列。</param>
        /// <param name="count">読み取り元配列のデータ数。</param>
        public static void Deinterleave<T>(T[] src, T[] dest, int count)
        {
            Deinterleave(src, 0, dest, 0, count);
        }

        /// <summary>
        /// 配列のインターリーブを解除し、単一の配列に格納します。
        /// </summary>
        /// <typeparam name="T">配列のデータ型。</typeparam>
        /// <param name="src">インターリーブされた入力となる T 型配列。</param>
        /// <param name="srcOffset">T 型配列の読み取りが開始されるインデックスのオフセット。</param>
        /// <param name="dest">インターリーブ解除の結果が格納される T 型配列。</param>
        /// <param name="destOffset">結果の T 型配列の格納が開始されるインデックスのオフセット。</param>
        /// <param name="count">読み取り元配列のデータ数。</param>
        public static void Deinterleave<T>(T[] src, int srcOffset, T[] dest, int destOffset, int count)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (dest == null)
                throw new ArgumentNullException(nameof(dest));

            if (srcOffset < 0 || srcOffset >= src.Length)
                throw new ArgumentOutOfRangeException(nameof(srcOffset));

            if (destOffset < 0 || destOffset >= dest.Length)
                throw new ArgumentOutOfRangeException(nameof(destOffset));

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (src.Length - srcOffset < count ||
                (dest.Length - destOffset) * 2 < count)
                throw new ArgumentOutOfRangeException(nameof(count));

            for (int i = 0, j = srcOffset, k = destOffset; i < count; i++, j += 2, k++)
                dest[k] = src[j];
        }

        /// <summary>
        /// 配列のインターリーブを解除し、2つの配列に格納します。
        /// </summary>
        /// <typeparam name="T">配列のデータ型。</typeparam>
        /// <param name="src">インターリーブされた入力となる T 型配列。</param>
        /// <param name="destR">インターリーブ解除の結果が格納される 1つ目の T 型配列。</param>
        /// <param name="destI">インターリーブ解除の結果が格納される 2つ目の T 型配列。</param>
        /// <param name="count">読み取り元配列のデータ数。</param>
        public static void Deinterleave<T>(T[] src, T[] destR, T[] destI, int count)
        {
            Deinterleave(src, 0, destR, 0, destI, 0, count);
        }

        /// <summary>
        /// 配列のインターリーブを解除し、2つの配列に格納します。
        /// </summary>
        /// <typeparam name="T">配列のデータ型。</typeparam>
        /// <param name="src">インターリーブされた入力となる T 型配列。</param>
        /// <param name="srcOffset"></param>
        /// <param name="destR">インターリーブ解除の結果が格納される 1つ目の T 型配列。</param>
        /// <param name="destROffset">1つ目の T 型配列の格納が開始されるインデックスのオフセット。</param>
        /// <param name="destI">インターリーブ解除の結果が格納される 2つ目の T 型配列。</param>
        /// <param name="destIOffset">2つ目の T 型配列の格納が開始されるインデックスのオフセット。</param>
        /// <param name="count">読み取り元配列のデータ数。</param>
        public static void Deinterleave<T>(T[] src, int srcOffset, T[] destR, int destROffset, T[] destI, int destIOffset, int count)
        {
            if (src == null)
                throw new ArgumentNullException(nameof(src));

            if (destR == null)
                throw new ArgumentNullException(nameof(destR));

            if (destI == null)
                throw new ArgumentNullException(nameof(destI));

            if (srcOffset < 0 || srcOffset >= src.Length)
                throw new ArgumentOutOfRangeException(nameof(srcOffset));

            if (destROffset < 0 || destROffset >= destR.Length)
                throw new ArgumentOutOfRangeException(nameof(destROffset));

            if (destIOffset < 0 || destIOffset >= destI.Length)
                throw new ArgumentOutOfRangeException(nameof(destIOffset));

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (src.Length - srcOffset < count * 2 ||
                destR.Length - destROffset < count ||
                destI.Length - destIOffset < count)
                throw new ArgumentOutOfRangeException(nameof(count));

            for (int i = 0, j = srcOffset, k = destROffset, l = destIOffset; i < count; i++, k++, l++)
            {
                destR[k] = src[j++];
                destI[l] = src[j++];
            }
        }
        #endregion
        #endregion
    }
}
