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

namespace SoundUtils
{
    /// <summary>
    /// 各種の窓関数を畳み込むためのメソッド群を提供します。
    /// </summary>
    public static class Window
    {
        public static void Hanning(double[] array)
        {
            int length = array.Length;
            double factor = 2.0 * Math.PI / (double)length;
            double k = (length & 1) == 0 ? 0.0 : 0.5;

            for (int i = 0; i < length; i++, k++)
                array[i] *= 0.5 - 0.5 * Math.Cos(factor * k);
        }

        public static void Hamming(double[] array)
        {
            int length = array.Length;
            double factor = 2.0 * Math.PI / (double)length;
            double k = (length & 1) == 0 ? 0.0 : 0.5;

            for (int i = 0; i < length; i++, k++)
                array[i] *= 0.54 - 0.46 * Math.Cos(factor * k);
        }

        public static void Bartlett(double[] array)
        {
            int length = array.Length;

            if (length % 2 == 0)
                for (int i = 0; i < length; i++)
                    array[i] *= 1.0 - 2.0 * Math.Abs(i / (double)length - 0.5);
            else
                for (int i = 0; i < length; i++)
                    array[i] *= 1.0 - 2.0 * Math.Abs((i + 0.5) / (double)length - 0.5);
        }

        public static void Blackman(double[] array)
        {
            int length = array.Length;
            double factor = 2.0 * Math.PI / (double)length;

            if (length % 2 == 0)
                for (int i = 0; i < length; i++)
                    array[i] *= 0.42 - 0.5 * Math.Cos(factor * i) + 0.08 * Math.Cos(2.0 * factor * i);
            else
                for (int i = 0; i < length; i++)
                    array[i] *= 0.42 - 0.5 * Math.Cos(factor * (i + 0.5)) + 0.08 * Math.Cos(2.0 * factor * (i + 0.5));
        }

        public static void Kaiser(double[] array, double alpha)
        {
            int length = array.Length;
            double tmp;
            alpha *= Math.PI;

            if (alpha == 0.0)
                return;

            if (length % 2 == 0)
                for (int i = 0; i < length; i++)
                {
                    tmp = 2.0 * i / (double)length - 1.0;
                    array[i] *= SoundMath.Bessel0(alpha * Math.Sqrt(1.0 - tmp * tmp)) / SoundMath.Bessel0(alpha);
                }
            else
                for (int i = 0; i < length; i++)
                {
                    tmp = 2.0 * (i + 0.5) / (double)length - 1.0;
                    array[i] *= SoundMath.Bessel0(alpha * Math.Sqrt(1.0 - tmp * tmp)) / SoundMath.Bessel0(alpha);
                }
        }
    }
}
