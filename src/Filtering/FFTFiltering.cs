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

/* このプログラムは以下のページのプログラムを移植したものである。
 * [FFT畳み込み積分(FFT Convolution), Overlap-Add Method]
 * http://www.softist.com/programming/overlap-add-method/overlap-add-method.htm
 *
 * [免責事項]
 * http://www.softist.com/disclaimer.htm
 */

using System;

namespace SoundUtils.Filtering
{
    /// <summary>
    /// 畳み込みの代わりに FFT による周波数空間の乗算によってフィルタリングを適用します。
    /// </summary>
    public class FftFiltering
    {
        #region -- Private Fields --
        private readonly int filterSize, segmentSize, fftSize, overlapSize, bufferSize;
        private readonly double[] fr, fi, xr, fftC, overlap, output;

        private readonly FastFourier fft;
        #endregion

        #region -- Constructors --
        /// <summary>
        /// パラメータを指定して新しい FFTFiltering クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="filterSize">フィルタサイズ。</param>
        /// <param name="segmentSize">セグメントサイズ。segmentSize は filterSize 未満 かつ fftSize 未満でなくてはなりません。</param>
        /// <param name="fftSize">FFT サイズ。fftSize は bufferSize 未満でなくてはなりません。</param>
        /// <param name="bufferSize">バッファサイズ。</param>
        public FftFiltering(int filterSize, int segmentSize, int fftSize, int bufferSize)
        {
            if (segmentSize < filterSize)
                throw new ArgumentException("segmentSize は filterSize 未満でなくてはなりません。");

            if (fftSize <= segmentSize)
                throw new ArgumentException("segmentSize は fftSize 未満でなくてはなりません。");

            if (fftSize > bufferSize)
                throw new ArgumentException("fftSize は bufferSize 未満でなくてはなりません。");

            this.filterSize = filterSize;
            this.segmentSize = segmentSize;
            this.fftSize = fftSize;
            this.overlapSize = fftSize - segmentSize;
            this.bufferSize = bufferSize;

            this.fr = new double[fftSize];
            this.fi = new double[fftSize];
            this.xr = new double[fftSize];
            this.fftC = new double[fftSize * 2];
            this.overlap = new double[overlapSize];
            this.output = new double[bufferSize];

            this.fft = new FastFourier(fftSize * 2);
        }
        #endregion

        #region -- Public Methods --
        /// <summary>
        /// 指定されたインパルス応答をフィルタリングに用いるよう設定します。
        /// </summary>
        /// <param name="impulseResponses">インパルス応答。</param>
        public void SetFilter(double[] impulseResponses)
        {
            if (impulseResponses == null)
                throw new ArgumentNullException(nameof(impulseResponses));

            Array.Clear(this.fr, 0, this.fftSize);
            Array.Clear(this.fi, 0, this.fftSize);
            Array.Clear(this.overlap, 0, this.overlapSize);
            Array.Clear(this.output, 0, this.bufferSize);

            Array.Copy(impulseResponses, this.fr, Math.Min(impulseResponses.Length, this.fftSize));
            this.fft.TransformComplex(false, this.fr, this.fi);
        }

        /// <summary>
        /// バッファにフィルタリングを適用します。
        /// </summary>
        /// <param name="buffer">フィルタリングされるバッファ。</param>
        public void Apply(double[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            if (buffer.Length < bufferSize)
                throw new ArgumentOutOfRangeException(nameof(buffer));

            for (int iOffset = 0; iOffset < bufferSize; iOffset += segmentSize)
            {
                Array.Clear(this.fftC, this.segmentSize, fftSize * 2 - this.segmentSize);
                Channel.Interleave(buffer, iOffset, this.fftC, 0, this.segmentSize);

                this.fft.TransformComplex(false, this.fftC);

                for (int i = 0, j = 1, k = 0, l = fftSize * 2; i < l; i += 2, j += 2, k++)
                {
                    double dblR = fr[k] * fftC[i] - fi[k] * fftC[j];
                    double dblI = fr[k] * fftC[j] + fi[k] * fftC[i];
                    fftC[i] = dblR;
                    fftC[j] = dblI;
                }

                this.fft.TransformComplex(true, this.fftC);
                Channel.Deinterleave(this.fftC, this.xr, this.fftSize);

                for (int i = 0; i < overlapSize; i++)
                    xr[i] += overlap[i];

                for (int i = segmentSize; i < fftSize; i++)
                    overlap[i - segmentSize] = xr[i];

                Array.Copy(xr, 0, buffer, iOffset, segmentSize);
            }
        }
        #endregion
    }
}
