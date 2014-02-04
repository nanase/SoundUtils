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
            double k = (length & 1) == 0 ? 0.0 : 0.5;
            double n = 1.0 / length;

            for (int i = 0; i < length; i++, k++)
                array[i] *= 1.0 - 2.0 * Math.Abs(k * n - 0.5);
        }

        public static void Nuttall(double[] array)
        {
            int length = array.Length;
            double factor = 2.0 * Math.PI / (double)length;
            double k = (length & 1) == 0 ? 0.0 : 0.5;

            for (int i = 0; i < length; i++, k++)
                array[i] *= 0.355768 -
                            0.487396 * Math.Cos(factor * k) +
                            0.144232 * Math.Cos(2.0 * factor * k) -
                            0.012604 * Math.Cos(3.0 * factor * k);
        }

        public static void Blackman(double[] array)
        {
            int length = array.Length;
            double factor = 2.0 * Math.PI / (double)length;
            double k = (length & 1) == 0 ? 0.0 : 0.5;

            for (int i = 0; i < length; i++, k++)
                array[i] *= 0.42 - 0.5 * Math.Cos(factor * k) + 0.08 * Math.Cos(2.0 * factor * k);
        }

        public static void BlackmanHarris(double[] array)
        {
            int length = array.Length;
            double factor = 2.0 * Math.PI / (double)length;
            double k = (length & 1) == 0 ? 0.0 : 0.5;

            for (int i = 0; i < length; i++, k++)
                array[i] *= 0.35875 -
                            0.48829 * Math.Cos(factor * k) +
                            0.14128 * Math.Cos(2.0 * factor * k) -
                            0.01168 * Math.Cos(3.0 * factor * k);
        }

        public static void BlackmanNuttall(double[] array)
        {
            int length = array.Length;
            double factor = 2.0 * Math.PI / (double)length;
            double k = (length & 1) == 0 ? 0.0 : 0.5;

            for (int i = 0; i < length; i++, k++)
                array[i] *= 0.3635819 -
                            0.4891775 * Math.Cos(factor * k) +
                            0.1365995 * Math.Cos(2.0 * factor * k) -
                            0.0106411 * Math.Cos(3.0 * factor * k);
        }
        public static void Kaiser(double[] array, double alpha)
        {
            int length = array.Length;
            double alpha_tmp;
            double n = 2.0 / length;
            double k = (length & 1) == 0 ? 0.0 : 0.5;

            alpha *= Math.PI;

            if (alpha == 0.0)
                return;

            alpha_tmp = 1.0 / SoundMath.Bessel0(alpha);

            for (int i = 0; i < length; i++, k++)
                array[i] *= SoundMath.Bessel0(alpha * Math.Sqrt(1.0 - Math.Pow(k * n - 1.0, 2.0))) * alpha_tmp;
        }
    }
}
