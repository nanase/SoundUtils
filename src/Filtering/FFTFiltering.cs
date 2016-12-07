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
        private readonly int segmentSize, fftSize, overlapSize, bufferSize;
        private readonly double[] fr, fi, xr, fftC, overlap, output;

        private readonly FastFourier fft;
        #endregion

        #region -- Constructors --

        /// <summary>
        /// パラメータを指定して新しい FFTFiltering クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="segmentSize">セグメントサイズ。segmentSize は filterSize 未満 かつ fftSize 未満でなくてはなりません。</param>
        /// <param name="fftSize">FFT サイズ。fftSize は bufferSize 未満でなくてはなりません。</param>
        /// <param name="bufferSize">バッファサイズ。</param>
        public FftFiltering(int segmentSize, int fftSize, int bufferSize)
        {
            if (fftSize <= segmentSize)
                throw new ArgumentException("segmentSize は fftSize 未満でなくてはなりません。");

            if (fftSize > bufferSize)
                throw new ArgumentException("fftSize は bufferSize 未満でなくてはなりません。");

            this.segmentSize = segmentSize;
            this.fftSize = fftSize;
            overlapSize = fftSize - segmentSize;
            this.bufferSize = bufferSize;

            fr = new double[fftSize];
            fi = new double[fftSize];
            xr = new double[fftSize];
            fftC = new double[fftSize * 2];
            overlap = new double[overlapSize];
            output = new double[bufferSize];

            fft = new FastFourier(fftSize * 2);
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

            Array.Clear(fr, 0, fftSize);
            Array.Clear(fi, 0, fftSize);
            Array.Clear(overlap, 0, overlapSize);
            Array.Clear(output, 0, bufferSize);

            Array.Copy(impulseResponses, fr, Math.Min(impulseResponses.Length, fftSize));
            fft.TransformComplex(false, fr, fi);
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

            for (var iOffset = 0; iOffset < bufferSize; iOffset += segmentSize)
            {
                Array.Clear(fftC, segmentSize, fftSize * 2 - segmentSize);
                Channel.Interleave(buffer, iOffset, fftC, 0, segmentSize);

                fft.TransformComplex(false, fftC);

                for (int i = 0, j = 1, k = 0, l = fftSize * 2; i < l; i += 2, j += 2, k++)
                {
                    var dblR = fr[k] * fftC[i] - fi[k] * fftC[j];
                    var dblI = fr[k] * fftC[j] + fi[k] * fftC[i];
                    fftC[i] = dblR;
                    fftC[j] = dblI;
                }

                fft.TransformComplex(true, fftC);
                Channel.Deinterleave(fftC, xr, fftSize);

                for (var i = 0; i < overlapSize; i++)
                    xr[i] += overlap[i];

                for (var i = segmentSize; i < fftSize; i++)
                    overlap[i - segmentSize] = xr[i];

                Array.Copy(xr, 0, buffer, iOffset, segmentSize);
            }
        }
        #endregion
    }
}
