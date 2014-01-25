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

namespace SoundUtils.Filtering.FIR
{
    public abstract class ImpulseResponse
    {
        #region -- Public Properties --
        public double SamplingRate { get; set; }
        #endregion
        
        #region -- Public Methods --
        public double[] Generate(double delta)
        {
            return this.Generate(ImpulseResponse.GetFilterSize(this.SamplingRate, delta));
        }

        public double[] Generate(int length)
        {
            double[] array = new double[length];

            this.GenerateValues(array, length);

            return array;
        }

        public void Generate(double[] array)
        {
            int length = array.Length;

            this.GenerateValues(array, length);
        }

        public void Generate(double[] array, double delta)
        {
            this.Generate(array, ImpulseResponse.GetFilterSize(this.SamplingRate, delta));
        }

        public void Generate(double[] array, int length)
        {
            if (array.Length < length)
                throw new ArgumentOutOfRangeException("length");

            int arrayLength = array.Length;

            this.GenerateValues(array, Math.Min(arrayLength, length));
        }
        #endregion

        #region -- Public Static Methods --
        public static double GetDelta(double samplingRate, int delayer)
        {
            // 奇数で返す
            if ((delayer & 1) == 0)
                delayer--;

            return (3.1 / (delayer - 0.5)) * samplingRate;
        }

        public static int GetFilterSize(double samplingRate, double delta)
        {
            delta /= samplingRate;
            int delayer = (int)(3.1 / delta + 0.5) - 1;

            // 偶数で返す
            if ((delayer & 1) == 1)
                delayer++;

            return delayer;
        }
        #endregion

        #region -- Protected Methods --
        protected abstract void GenerateValues(double[] array, int size);
        #endregion
    }
}
