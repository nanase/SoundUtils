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

namespace SoundUtils.Filtering.FIR
{
    /// <summary>
    /// 任意のインパルス応答を生成するための機能を提供します。
    /// </summary>
    public class ImpulseGenerator
    {
        /// <summary>
        /// 周波数特性から有限インパルス応答を生成します。
        /// </summary>
        /// <param name="input">周波数応答。</param>
        /// <param name="output">生成された有限インパルス応答。</param>
        public static void Generate(double[] input, double[] output)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (output == null)
                throw new ArgumentNullException(nameof(output));

            if (input.Length * 2 != output.Length)
                throw new ArgumentException();

            var filterLength = output.Length;
            var blockSize = filterLength / 2;

            var fft = new FastFourier(filterLength * 2);

            Array.Clear(output, 0, filterLength);
            var imDummy = new double[filterLength];

            Array.Copy(input, 0, output, blockSize, blockSize);
            Array.Reverse(output, blockSize, blockSize);
            Array.Copy(input, 0, output, 0, blockSize);

            fft.TransformComplex(true, output, imDummy);

            Array.Reverse(output, 0, blockSize);
            Array.Reverse(output, blockSize, blockSize);
            Array.Reverse(output, 0, filterLength);
        }
    }
}
