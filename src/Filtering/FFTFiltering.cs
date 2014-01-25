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

namespace SoundUtils.Filtering
{
    public class FFTFiltering
    {
        #region -- Private Fields --
        private readonly double sfreq;
        private readonly int filterSize, segmentSize, fftSize, overlapSize, bufferSize;
        private readonly double[] fr, fi, xr, xi, overlap, output;
        #endregion

        #region -- Constructors --
        public FFTFiltering(double samplingFrequency, int filterSize, int segmentSize, int fftSize, int bufferSize)
        {
            if (segmentSize < filterSize)
                throw new ArgumentException("segmentSize は filterSize 未満でなくてはなりません。");

            if (fftSize <= segmentSize)
                throw new ArgumentException("segmentSize は fftSize 未満でなくてはなりません。");

            if (fftSize > bufferSize)
                throw new ArgumentException("fftSize は bufferSize 未満でなくてはなりません。");

            this.sfreq = samplingFrequency;
            this.filterSize = filterSize;
            this.segmentSize = segmentSize;
            this.fftSize = fftSize;
            this.overlapSize = fftSize - segmentSize;
            this.bufferSize = bufferSize;

            this.fr = new double[fftSize];
            this.fi = new double[fftSize];
            this.xr = new double[fftSize];
            this.xi = new double[fftSize];
            this.overlap = new double[overlapSize];
            this.output = new double[bufferSize];
        }
        #endregion

        #region -- Public Methods --
        public void SetFilter(double[] impulseResponses)
        {
            Array.Clear(this.fr, 0, this.fftSize);
            Array.Clear(this.fi, 0, this.fftSize);
            Array.Clear(this.overlap, 0, this.overlapSize);
            Array.Clear(this.output, 0, this.bufferSize);

            Array.Copy(impulseResponses, this.fr, Math.Min(impulseResponses.Length, this.fftSize));
            FastFourier.FFT(this.fftSize, this.fr, this.fi);
        }

        public void Apply(double[] buffer)
        {
            for (int iOffset = 0; iOffset < bufferSize; iOffset += segmentSize)
            {
                Array.Clear(xr, 0, fftSize);
                Array.Clear(xi, 0, fftSize);

                Array.Copy(buffer, iOffset, xr, 0, segmentSize);

                FastFourier.FFT(fftSize, xr, xi);

                for (int i = 0; i < fftSize; i++)
                {
                    double dbl_R = fr[i] * xr[i] - fi[i] * xi[i];
                    double dbl_I = fr[i] * xi[i] + fi[i] * xr[i];
                    xr[i] = dbl_R;
                    xi[i] = dbl_I;
                }

                FastFourier.IFFT(fftSize, xr, xi);

                for (int i = 0; i < overlapSize; i++)
                    xr[i] += overlap[i];

                for (int i = segmentSize; i < fftSize; i++)
                    overlap[i - segmentSize] = xr[i];

                Array.Copy(xr, 0, buffer, iOffset, segmentSize);
            }
        }
        #endregion

        #region -- Public Static Methods --
        public static double GetDelta(double samplingFreq, int delayer)
        {
            if (delayer % 2 == 0)
                delayer--;

            double delta = 3.1 / (delayer - 0.5);
            delta *= samplingFreq;

            return delta;
        }

        public static int GetFilterSize(double samplingFreq, double delta)
        {
            delta /= samplingFreq;
            int delayer = (int)(3.1 / delta + 0.5) - 1;

            if (delayer % 2 == 1)
                delayer++;

            return delayer;
        }

        public static double[] GetFilter(FilterType type, double samplingFreq, double delta, double fe1, double fe2)
        {
            delta /= samplingFreq;

            int delayer = (int)(3.1 / delta + 0.5) - 1;

            if (delayer % 2 == 1)
                delayer++;

            double[] filter = new double[delayer + 1];
            int offset = delayer / 2;

            fe1 /= samplingFreq;
            fe2 /= samplingFreq;

            double fe1_2 = 2.0 * fe1;
            double fe2_2 = 2.0 * fe2;
            double fe1_PI2 = 2.0 * Math.PI * fe1;
            double fe2_PI2 = 2.0 * Math.PI * fe2;

            switch (type)
            {
                case FilterType.LowPass:
                    for (int i = -offset; i <= offset; i++)
                        filter[offset + i] = fe1_2 * SoundMath.Sinc(fe1_PI2 * i);
                    break;

                case FilterType.HighPass:
                    for (int i = -offset; i <= offset; i++)
                        filter[offset + i] = SoundMath.Sinc(Math.PI * i) - fe1_2 * SoundMath.Sinc(fe1_PI2 * i);
                    break;

                case FilterType.BandPass:
                    for (int i = -offset; i <= offset; i++)
                        filter[offset + i] = fe2_2 * SoundMath.Sinc(fe2_PI2 * i) - fe1_2 * SoundMath.Sinc(fe1_PI2 * i);
                    break;

                case FilterType.BandElimination:
                    for (int i = -offset; i <= offset; i++)
                        filter[offset + i] = SoundMath.Sinc(Math.PI * i) -
                                             fe2_2 * SoundMath.Sinc(fe2_PI2 * i) +
                                             fe1_2 * SoundMath.Sinc(fe1_PI2 * i);

                    break;

                default:
                    break;
            }

            Window.Hanning(filter);

            return filter;
        }
        #endregion
    }
}
