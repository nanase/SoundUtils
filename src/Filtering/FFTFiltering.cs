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
    public class FFTFiltering
    {
        #region -- Private Fields --
        private readonly int filterSize, segmentSize, fftSize, overlapSize, bufferSize;
        private readonly double[] fr, fi, xr, fft_c, overlap, output;

        private readonly FastFourier fft;
        #endregion

        #region -- Constructors --
        public FFTFiltering(int filterSize, int segmentSize, int fftSize, int bufferSize)
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
            this.fft_c = new double[fftSize * 2];
            this.overlap = new double[overlapSize];
            this.output = new double[bufferSize];

            this.fft = new FastFourier(fftSize * 2);
        }
        #endregion

        #region -- Public Methods --
        public void SetFilter(double[] impulseResponses)
        {
            if (impulseResponses == null)
                throw new ArgumentNullException("impulseResponses");

            Array.Clear(this.fr, 0, this.fftSize);
            Array.Clear(this.fi, 0, this.fftSize);
            Array.Clear(this.overlap, 0, this.overlapSize);
            Array.Clear(this.output, 0, this.bufferSize);

            Array.Copy(impulseResponses, this.fr, Math.Min(impulseResponses.Length, this.fftSize));
            this.fft.TransformComplex(false, this.fr, this.fi);
        }

        public void Apply(double[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException("buffer");

            if (buffer.Length < bufferSize)
                throw new ArgumentOutOfRangeException("buffer");

            for (int iOffset = 0; iOffset < bufferSize; iOffset += segmentSize)
            {
                Array.Clear(this.fft_c, this.segmentSize, fftSize * 2 - this.segmentSize);
                Channel.Interleave(buffer, iOffset, this.fft_c, 0, this.segmentSize);

                this.fft.TransformComplex(false, this.fft_c);

                for (int i = 0, j = 1, k = 0, l = fftSize * 2; i < l; i += 2, j += 2, k++)
                {
                    double dbl_R = fr[k] * fft_c[i] - fi[k] * fft_c[j];
                    double dbl_I = fr[k] * fft_c[j] + fi[k] * fft_c[i];
                    fft_c[i] = dbl_R;
                    fft_c[j] = dbl_I;
                }

                this.fft.TransformComplex(true, this.fft_c);
                Channel.Deinterleave(this.fft_c, this.xr, this.fftSize);

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
