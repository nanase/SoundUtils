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

namespace SoundUtils.Filtering.IIR
{
    public class Resonator : InfiniteImpulseResponse
    {
        #region -- Public Properties --
        public double[] Frequencies { get; set; }

        public double Amplifier { get; set; }

        public double Strength { get; set; }
        #endregion

        #region -- Protected Methods --
        protected override void GenerateValues(double[] array, int size)
        {
            double amp = this.Amplifier / (size / 2);

            Array.Clear(array, 0, size);

            if (this.Strength == 0.0)
                array[0] = amp;
            else
                for (int j = 0; j < this.Frequencies.Length; j++)
                    for (int i = 0; i < size; i++)
                        array[i] += Math.Sin(i * this.Frequencies[j] * 2.0 * Math.PI / this.SamplingRate) * amp *
                                    Math.Exp(-Math.Pow(i, 2.0) / Math.Pow(this.Strength, 2.0));
        }
        #endregion
    }
}
