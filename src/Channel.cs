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
        /// <param name="channelL">L チャネルに分割される T 型配列。</param>
        /// <param name="channelR">R チャネルに分割される T 型配列。</param>
        public static void Split<T>(this T[] source, T[] channelL, T[] channelR)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            Deinterleave(source, channelL, channelR, source.Length / 2);
        }

        /// <summary>
        /// 2 つのチャネルを 1 つの配列に合成します。
        /// </summary>
        /// <typeparam name="T">配列のデータ型。</typeparam>
        /// <param name="channelL">合成元の L チャネルの T 型配列。</param>
        /// <param name="channelR">合成元の R チャネルの T 型配列。</param>
        /// <param name="destination">合成先の T 型配列。</param>
        public static void Join<T>(this T[] channelL, T[] channelR, T[] destination)
        {
            if (destination == null)
                throw new ArgumentNullException(nameof(destination));

            Interleave(channelL, channelR, destination, destination.Length / 2);
        }

        #region Interleave

        /// <summary>
        /// 単一の配列と既定の値をインターリーブします。
        /// </summary>
        /// <typeparam name="T">配列のデータ型。</typeparam>
        /// <param name="source">インターリーブされる T 型配列。</param>
        /// <param name="destination">インターリーブされた結果が格納されるの T 型配列</param>
        /// <param name="count">インターリーブされる配列から読み取られるデータ数。</param>
        public static void Interleave<T>(this T[] source, T[] destination, int count)
        {
            Interleave(source, 0, destination, 0, count);
        }

        /// <summary>
        /// 単一の配列と既定の値をインターリーブします。
        /// </summary>
        /// <typeparam name="T">配列のデータ型。</typeparam>
        /// <param name="source">インターリーブされる T 型配列。</param>
        /// <param name="srcOffset">読み取りが開始されるインデックスのオフセット。</param>
        /// <param name="destination">インターリーブされた結果が格納される T 型配列</param>
        /// <param name="destOffset">書き込みが開始されるインデックスのオフセット。</param>
        /// <param name="count">インターリーブされる配列から読み取られるデータ数。</param>
        public static void Interleave<T>(this T[] source, int srcOffset, T[] destination, int destOffset, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (destination == null)
                throw new ArgumentNullException(nameof(destination));

            if (srcOffset < 0 || srcOffset >= source.Length)
                throw new ArgumentOutOfRangeException(nameof(srcOffset));

            if (destOffset < 0 || destOffset >= destination.Length)
                throw new ArgumentOutOfRangeException(nameof(destOffset));

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (source.Length - srcOffset < count / 2 ||
                destination.Length - destOffset < count)
                throw new ArgumentOutOfRangeException(nameof(count));

            for (int i = 0, j = srcOffset, k = destOffset; i < count; i++, j++, k++)
            {
                destination[k] = source[j];
                destination[++k] = default(T);
            }
        }

        /// <summary>
        /// 2つの配列をインターリーブします。
        /// </summary>
        /// <typeparam name="T">配列のデータ型。</typeparam>
        /// <param name="sourceR">1つ目の T 型配列。</param>
        /// <param name="sourceI">2つ目の T 型配列。</param>
        /// <param name="destination">インタリーブされた結果が格納される T 型配列。</param>
        /// <param name="count">インターリーブされる配列から読み取られるデータ数。</param>
        public static void Interleave<T>(T[] sourceR, T[] sourceI, T[] destination, int count)
        {
            Interleave(sourceR, 0, sourceI, 0, destination, 0, count);
        }

        /// <summary>
        /// 2つの配列をインターリーブします。
        /// </summary>
        /// <typeparam name="T">配列のデータ型。</typeparam>
        /// <param name="sourceR">1つ目の T 型配列。</param>
        /// <param name="sourceROffset">1つ目の T 型配列の読み取りが開始されるインデックスのオフセット。</param>
        /// <param name="sourceI">2つ目の T 型配列。</param>
        /// <param name="sourceIOffset">2つ目の T 型配列の読み取りが開始されるインデックスのオフセット。</param>
        /// <param name="destination">インタリーブされた結果が格納される T 型配列。</param>
        /// <param name="destinationOffset">結果の T 型配列の格納が開始されるインデックスのオフセット。</param>
        /// <param name="count">インターリーブされる配列から読み取られるデータ数。</param>
        public static void Interleave<T>(T[] sourceR, int sourceROffset, T[] sourceI, int sourceIOffset, T[] destination, int destinationOffset, int count)
        {
            if (sourceR == null)
                throw new ArgumentNullException(nameof(sourceR));

            if (sourceI == null)
                throw new ArgumentNullException(nameof(sourceI));

            if (destination == null)
                throw new ArgumentNullException(nameof(destination));

            if (sourceROffset < 0 || sourceROffset >= sourceR.Length)
                throw new ArgumentOutOfRangeException(nameof(sourceROffset));

            if (sourceIOffset < 0 || sourceIOffset >= sourceI.Length)
                throw new ArgumentOutOfRangeException(nameof(sourceIOffset));

            if (destinationOffset < 0 || destinationOffset >= destination.Length)
                throw new ArgumentOutOfRangeException(nameof(destinationOffset));

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (sourceR.Length - sourceROffset < count / 2 ||
                sourceI.Length - sourceIOffset < count / 2 ||
                destination.Length - destinationOffset < count)
                throw new ArgumentOutOfRangeException(nameof(count));

            for (int i = 0, j = sourceROffset, k = sourceIOffset, l = destinationOffset; i < count; i++, j++, k++)
            {
                destination[l++] = sourceR[j];
                destination[l++] = sourceI[k];
            }
        }

        #endregion

        #region Deinterleave

        /// <summary>
        /// 配列のインターリーブを解除し、単一の配列に格納します。
        /// </summary>
        /// <typeparam name="T">配列のデータ型。</typeparam>
        /// <param name="source">インターリーブされた入力となる T 型配列。</param>
        /// <param name="destination">インターリーブ解除の結果が格納される T 型配列。</param>
        /// <param name="count">読み取り元配列のデータ数。</param>
        public static void Deinterleave<T>(this T[] source, T[] destination, int count)
        {
            Deinterleave(source, 0, destination, 0, count);
        }

        /// <summary>
        /// 配列のインターリーブを解除し、単一の配列に格納します。
        /// </summary>
        /// <typeparam name="T">配列のデータ型。</typeparam>
        /// <param name="source">インターリーブされた入力となる T 型配列。</param>
        /// <param name="sourceOffset">T 型配列の読み取りが開始されるインデックスのオフセット。</param>
        /// <param name="destination">インターリーブ解除の結果が格納される T 型配列。</param>
        /// <param name="destinationOffset">結果の T 型配列の格納が開始されるインデックスのオフセット。</param>
        /// <param name="count">読み取り元配列のデータ数。</param>
        public static void Deinterleave<T>(this T[] source, int sourceOffset, T[] destination, int destinationOffset, int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (destination == null)
                throw new ArgumentNullException(nameof(destination));

            if (sourceOffset < 0 || sourceOffset >= source.Length)
                throw new ArgumentOutOfRangeException(nameof(sourceOffset));

            if (destinationOffset < 0 || destinationOffset >= destination.Length)
                throw new ArgumentOutOfRangeException(nameof(destinationOffset));

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (source.Length - sourceOffset < count ||
                (destination.Length - destinationOffset) * 2 < count)
                throw new ArgumentOutOfRangeException(nameof(count));

            for (int i = 0, j = sourceOffset, k = destinationOffset; i < count; i++, j += 2, k++)
                destination[k] = source[j];
        }

        /// <summary>
        /// 配列のインターリーブを解除し、2つの配列に格納します。
        /// </summary>
        /// <typeparam name="T">配列のデータ型。</typeparam>
        /// <param name="source">インターリーブされた入力となる T 型配列。</param>
        /// <param name="destinationR">インターリーブ解除の結果が格納される 1つ目の T 型配列。</param>
        /// <param name="destinationI">インターリーブ解除の結果が格納される 2つ目の T 型配列。</param>
        /// <param name="count">読み取り元配列のデータ数。</param>
        public static void Deinterleave<T>(this T[] source, T[] destinationR, T[] destinationI, int count)
        {
            Deinterleave(source, 0, destinationR, 0, destinationI, 0, count);
        }

        /// <summary>
        /// 配列のインターリーブを解除し、2つの配列に格納します。
        /// </summary>
        /// <typeparam name="T">配列のデータ型。</typeparam>
        /// <param name="source">インターリーブされた入力となる T 型配列。</param>
        /// <param name="sourceOffset"></param>
        /// <param name="destinationR">インターリーブ解除の結果が格納される 1つ目の T 型配列。</param>
        /// <param name="destinationROffset">1つ目の T 型配列の格納が開始されるインデックスのオフセット。</param>
        /// <param name="destinationI">インターリーブ解除の結果が格納される 2つ目の T 型配列。</param>
        /// <param name="destinationIOffset">2つ目の T 型配列の格納が開始されるインデックスのオフセット。</param>
        /// <param name="count">読み取り元配列のデータ数。</param>
        public static void Deinterleave<T>(
            this T[] source,
            int sourceOffset,
            T[] destinationR, 
            int destinationROffset,
            T[] destinationI,
            int destinationIOffset, 
            int count)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (destinationR == null)
                throw new ArgumentNullException(nameof(destinationR));

            if (destinationI == null)
                throw new ArgumentNullException(nameof(destinationI));

            if (sourceOffset < 0 || sourceOffset >= source.Length)
                throw new ArgumentOutOfRangeException(nameof(sourceOffset));

            if (destinationROffset < 0 || destinationROffset >= destinationR.Length)
                throw new ArgumentOutOfRangeException(nameof(destinationROffset));

            if (destinationIOffset < 0 || destinationIOffset >= destinationI.Length)
                throw new ArgumentOutOfRangeException(nameof(destinationIOffset));

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (source.Length - sourceOffset < count * 2 ||
                destinationR.Length - destinationROffset < count ||
                destinationI.Length - destinationIOffset < count)
                throw new ArgumentOutOfRangeException(nameof(count));

            for (int i = 0, j = sourceOffset, k = destinationROffset, l = destinationIOffset; i < count; i++, k++, l++)
            {
                destinationR[k] = source[j++];
                destinationI[l] = source[j++];
            }
        }

        #endregion
        #endregion
    }
}
