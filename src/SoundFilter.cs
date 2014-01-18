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

namespace SoundUtils
{
    public class SoundFilter
    {
        #region -- Public Fields --
        public const int DefaultFilterSize = 2048 / 4;
        public const int DefaultSegmentSize = 2048 / 4;
        public const int DefaultFFTSize = 4096 / 4;
        #endregion

        #region -- Private Fields --
        private readonly FFTFiltering lfilter, rfilter;
        private readonly bool stereo;
        private readonly double[] lbuffer, rbuffer;
        #endregion

        #region -- Constructors --
        public SoundFilter(bool stereo, double samplingFreq, int bufferSize)
        {
            this.stereo = stereo;
            if (stereo)
            {
                this.lfilter = new FFTFiltering(samplingFreq, DefaultFilterSize, DefaultSegmentSize, DefaultFFTSize, bufferSize / 2);
                this.rfilter = new FFTFiltering(samplingFreq, DefaultFilterSize, DefaultSegmentSize, DefaultFFTSize, bufferSize / 2);

                this.lbuffer = new double[bufferSize / 2];
                this.rbuffer = new double[bufferSize / 2];
            }
            else
            {
                this.lfilter = new FFTFiltering(samplingFreq, DefaultFilterSize, DefaultSegmentSize, DefaultFFTSize, bufferSize);
                this.lbuffer = new double[bufferSize];
            }
        }
        #endregion

        #region -- Public Methods --
        public void SetFilter(FilterType type, double fe1, double fe2)
        {
            this.lfilter.SetFilter(type, fe1, fe2);

            if (this.stereo)
                this.rfilter.SetFilter(type, fe1, fe2);
        }

        public void Filtering(double[] input, double[] output)
        {
            if (this.stereo)
            {
                Channel.Split(input, this.lbuffer, this.rbuffer);
                this.lfilter.Apply(this.lbuffer);
                this.rfilter.Apply(this.rbuffer);
                Channel.Join(this.lbuffer, this.rbuffer, output);
            }
            else
            {
                this.lfilter.Apply(input);
                input.CopyTo(output, 0);
            }
        }
        #endregion
    }
}
