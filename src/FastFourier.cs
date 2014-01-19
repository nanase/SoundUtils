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
    public class FastFourier
    {
        public static void FFT(int n, double[] ar, double[] ai)
        {
            int m, mh, i, j, k;
            double wr, wi, xr, xi,
                   theta = -Math.PI * 2.0 / n;

            for (m = n; (mh = m / 2) >= 1; m = mh, theta *= 2.0)
                for (i = 0; i < mh; i++)
                    for (wr = Math.Cos(theta * i), wi = Math.Sin(theta * i), j = i; j < n; j += m)
                    {
                        k = j + mh;
                        xr = ar[j] - ar[k];
                        xi = ai[j] - ai[k];
                        ar[j] += ar[k];
                        ai[j] += ai[k];
                        ar[k] = wr * xr - wi * xi;
                        ai[k] = wr * xi + wi * xr;
                    }

            for (i = 0, j = 1; j < n - 1; j++)
            {
                for (k = n / 2; k > (i ^= k); k /= 2) ;
                if (j < i)
                {
                    xr = ar[j];
                    xi = ai[j];
                    ar[j] = ar[i];
                    ai[j] = ai[i];
                    ar[i] = xr;
                    ai[i] = xi;
                }
            }
        }

        public static void IFFT(int n, double[] ar, double[] ai)
        {
            int m, mh, i, j, k;
            double wr, wi, xr, xi,
                   nd = 1.0 / (double)n,
                   theta = Math.PI * 2.0 * nd;

            for (m = n; (mh = m / 2) >= 1; m = mh, theta *= 2.0)
                for (i = 0; i < mh; i++)
                    for (wr = Math.Cos(theta * i), wi = Math.Sin(theta * i), j = i; j < n; j += m)
                    {
                        k = j + mh;
                        xr = ar[j] - ar[k];
                        xi = ai[j] - ai[k];
                        ar[j] += ar[k];
                        ai[j] += ai[k];
                        ar[k] = wr * xr - wi * xi;
                        ai[k] = wr * xi + wi * xr;
                    }

            for (i = 0, j = 1; j < n - 1; j++)
            {
                for (k = n / 2; k > (i ^= k); k /= 2) ;
                if (j < i)
                {
                    xr = ar[j];
                    xi = ai[j];
                    ar[j] = ar[i];
                    ai[j] = ai[i];
                    ar[i] = xr;
                    ai[i] = xi;
                }
            }

            for (i = 0; i < n; i++)
            {
                ar[i] *= nd;
                ai[i] *= nd;
            }
        }
    }
}
