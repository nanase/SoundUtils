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
    public class ImpulseGenerator
    {
        public static void Generate(double[] input, double[] output)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            if (output == null)
                throw new ArgumentNullException("output");

            if (input.Length * 2 != output.Length)
                throw new ArgumentException();

            int filterLength = output.Length;
            int blockSize = filterLength / 2;

            FastFourier fft = new FastFourier(filterLength * 2);

            Array.Clear(output, 0, filterLength);
            double[] im_dummy = new double[filterLength];

            Array.Copy(input, 0, output, blockSize, blockSize);
            Array.Reverse(output, blockSize, blockSize);
            Array.Copy(input, 0, output, 0, blockSize);

            fft.TransformComplex(true, output, im_dummy);

            Array.Reverse(output, 0, blockSize);
            Array.Reverse(output, blockSize, blockSize);
            Array.Reverse(output, 0, filterLength);
        }
    }
}
