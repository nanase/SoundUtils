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

using SoundUtils.Filtering.FIR;

namespace SoundUtils.Filtering
{
    public class OverSampling
    {
        private readonly double samplingRate;
        private readonly bool stereo;
        private readonly int magnification, filterSize;

        private readonly SoundFilter filter;

        public OverSampling(double samplingRate, int magnification, bool stereo, int filterSize)
        {
            this.samplingRate = samplingRate;
            this.magnification = magnification;
            this.stereo = stereo;
            this.filterSize = filterSize;

            this.filter = new SoundFilter(stereo, filterSize);

            var filterGenerator = new LowPassFilter()
            {
                SamplingRate = samplingRate * magnification,
                CutoffFrequency = samplingRate / 2 - ImpulseResponse.GetDelta(samplingRate * magnification, filterSize)
            };
            double[] impulse = filterGenerator.Generate(filterSize / 2);

            Window.Blackman(impulse);
            filter.SetFilter(impulse);
        }

        public int Apply(double[] buffer)
        {
            if (this.magnification == 1)
                return this.filterSize;

            filter.Filtering(buffer);

            for (int i = 0, j = 0, inc =  this.magnification * (this.stereo ? 2 : 1); i < filterSize; i += inc)
            {
                buffer[j++] = buffer[i];
                buffer[j++] = buffer[i + 1];
            }

            return this.filterSize / this.magnification;
        }
    }
}
