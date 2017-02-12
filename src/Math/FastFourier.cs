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

/* このクラスは「汎用FFTパッケージ」を C# に移植したものである。
 * 以下のURLより取得したソースについて
 * をオブジェクト化、未使用の関数の削除を行った。
 * http://www.kurims.kyoto-u.ac.jp/~ooura/fft-j.html
 * 
 * 実用的で素晴らしいライブラリを公開してくださった作者に感謝の意を表する。
 */

/* General Purpose FFT (Fast Fourier/Cosine/Sine Transform) Package

Copyright:
    Copyright(C) 1996-2001 Takuya OOURA
    email: ooura@mmm.t.u-tokyo.ac.jp
    download: http://momonga.t.u-tokyo.ac.jp/~ooura/fft.html
    You may use, copy, modify this code for any purpose and 
    without fee. You may distribute this ORIGINAL package.
 */

using System;

namespace SoundUtils
{
    /// <summary>
    /// 高速フーリエ変換と作業テーブルを保持するオブジェクトを提供します。
    /// </summary>
    public class FastFourier
    {
        #region -- Private Fields --
        private readonly int[] ip;
        private readonly double[] w;
        private readonly int n;
        private double[] interleave;
        #endregion

        #region -- Constructors --
        /// <summary>
        /// FFT サイズを指定して新しい FastFourier クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="n">FFT サイズ。</param>
        public FastFourier(int n)
        {
            if (n < 4)
                throw new ArgumentOutOfRangeException(nameof(n));

            this.n = n;
            ip = new int[n / 2];
            w = new double[n / 2];
        }
        #endregion

        #region -- Public Methods --
        /// <summary>
        /// 複素数がインターリーブされたデータを離散フーリエ変換します。
        /// </summary>
        /// <param name="data">複素数がインターリーブされた配列。</param>
        /// <param name="invert">逆離散フーリエ変換を行うかの真偽値。</param>
        public void TransformComplex(double[] data, bool invert = false)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (data.Length != n)
                throw new ArgumentOutOfRangeException(nameof(data));

            Cdft(n, invert, data, ip, w);

            if (!invert)
                return;

            var f = 2.0 / n;

            for (var j = 0; j < n; j++)
                data[j] *= f;
        }

        /// <summary>
        /// 実数と虚数のデータを離散フーリエ変換します。
        /// </summary>
        /// <param name="real">実数データの配列。</param>
        /// <param name="imaginary">虚数データの配列。</param>
        /// <param name="invert">逆離散フーリエ変換を行うかの真偽値。</param>
        public void TransformComplex(double[] real, double[] imaginary, bool invert = false)
        {
            if (real == null)
                throw new ArgumentNullException(nameof(real));

            if (imaginary == null)
                throw new ArgumentNullException(nameof(imaginary));

            if (real.Length != n / 2)
                throw new ArgumentOutOfRangeException(nameof(real));

            if (imaginary.Length != n / 2)
                throw new ArgumentOutOfRangeException(nameof(imaginary));

            if (interleave == null)
                interleave = new double[n];

            Channel.Interleave(real, imaginary, interleave, n / 2);
            TransformComplex(interleave, invert);
            interleave.Deinterleave(real, imaginary, n / 2);
        }

        /// <summary>
        /// 実数データを離散フーリエ変換します。
        /// </summary>
        /// <param name="data">入力は連続した実数データの配列。
        /// 出力は複素数のインターリーブとなり、かつサンプリング周波数の半分のデータ数です。</param>
        /// <param name="invert">逆離散フーリエ変換を行うかの真偽値。</param>
        public void TransformReal(double[] data, bool invert = false)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (data.Length != n)
                throw new ArgumentOutOfRangeException(nameof(data));

            Rdft(n, invert, data, ip, w);

            if (!invert)
                return;

            var f = 2.0 / n;

            for (var j = 0; j < n; j++)
                data[j] *= f;
        }
        #endregion

        #region -- Private Static Methods --
        private static void Cdft(int n, bool invert, double[] a, int[] ip, double[] w)
        {
            if (n > ip[0] << 2)
                Makewt(n >> 2, ip, w);

            if (n > 4)
            {
                if (invert)
                {
                    Bitrv2Conj(n, ip, 2, a);
                    Cftbsub(n, a, w);
                }
                else
                {
                    Bitrv2(n, ip, 2, a);
                    Cftfsub(n, a, w);
                }
            }
            else if (n == 4)
                Cftfsub(n, a, w);
        }

        private static void Rdft(int n, bool invert, double[] a, int[] ip, double[] w)
        {
            var nw = ip[0];

            if (n > nw << 2)
            {
                nw = n >> 2;
                Makewt(nw, ip, w);
            }

            var nc = ip[1];

            if (n > nc << 2)
            {
                nc = n >> 2;
                Makect(nc, ip, w, nw);
            }

            if (invert)
            {
                a[1] = 0.5 * (a[0] - a[1]);
                a[0] -= a[1];
                if (n > 4)
                {
                    Rftbsub(n, a, nc, w, nw);
                    Bitrv2(n, ip, 2, a);
                    Cftbsub(n, a, w);
                }
                else if (n == 4)
                    Cftfsub(n, a, w);
            }
            else
            {
                if (n > 4)
                {
                    Bitrv2(n, ip, 2, a);
                    Cftfsub(n, a, w);
                    Rftfsub(n, a, nc, w, nw);
                }
                else if (n == 4)
                    Cftfsub(n, a, w);

                var xi = a[0] - a[1];
                a[0] += a[1];
                a[1] = xi;
            }
        }

        /* -------- initializing routines -------- */
        private static void Makewt(int nw, int[] ip, double[] w)
        {
            ip[0] = nw;
            ip[1] = 1;

            if (nw <= 2)
                return;

            var nwh = nw >> 1;
            var delta = Math.Atan(1.0) / nwh;
            w[0] = 1;
            w[1] = 0;
            w[nwh] = Math.Cos(delta * nwh);
            w[nwh + 1] = w[nwh];

            if (nwh <= 2)
                return;

            for (var j = 2; j < nwh; j += 2)
            {
                var x = Math.Cos(delta * j);
                var y = Math.Sin(delta * j);
                w[j] = x;
                w[j + 1] = y;
                w[nw - j] = y;
                w[nw - j + 1] = x;
            }

            Bitrv2(nw, ip, 2, w);
        }

        private static void Makect(int nc, int[] ip, double[] c, int offset)
        {
            ip[1] = nc;

            if (nc <= 1)
                return;

            var nch = nc >> 1;
            var delta = Math.Atan(1.0) / nch;
            c[offset] = Math.Cos(delta * nch);
            c[offset + nch] = 0.5 * c[offset];

            for (var j = 1; j < nch; j++)
            {
                c[offset + j] = 0.5 * Math.Cos(delta * j);
                c[offset + nc - j] = 0.5 * Math.Sin(delta * j);
            }
        }

        /* -------- child routines -------- */
        private static void Bitrv2(int n, int[] ip, int offset, double[] a)
        {
            int j1, k1;
            double xr, xi, yr, yi;

            ip[offset] = 0;
            var l = n;
            var m = 1;

            while (m << 3 < l)
            {
                l >>= 1;

                for (var j = 0; j < m; j++)
                {
                    ip[offset + m + j] = ip[offset + j] + l;
                }

                m <<= 1;
            }

            var m2 = 2 * m;

            if (m << 3 == l)
            {
                for (var k = 0; k < m; k++)
                {
                    for (var j = 0; j < k; j++)
                    {
                        j1 = 2 * j + ip[offset + k];
                        k1 = 2 * k + ip[offset + j];
                        xr = a[j1];
                        xi = a[j1 + 1];
                        yr = a[k1];
                        yi = a[k1 + 1];
                        a[j1] = yr;
                        a[j1 + 1] = yi;
                        a[k1] = xr;
                        a[k1 + 1] = xi;
                        j1 += m2;
                        k1 += 2 * m2;
                        xr = a[j1];
                        xi = a[j1 + 1];
                        yr = a[k1];
                        yi = a[k1 + 1];
                        a[j1] = yr;
                        a[j1 + 1] = yi;
                        a[k1] = xr;
                        a[k1 + 1] = xi;
                        j1 += m2;
                        k1 -= m2;
                        xr = a[j1];
                        xi = a[j1 + 1];
                        yr = a[k1];
                        yi = a[k1 + 1];
                        a[j1] = yr;
                        a[j1 + 1] = yi;
                        a[k1] = xr;
                        a[k1 + 1] = xi;
                        j1 += m2;
                        k1 += 2 * m2;
                        xr = a[j1];
                        xi = a[j1 + 1];
                        yr = a[k1];
                        yi = a[k1 + 1];
                        a[j1] = yr;
                        a[j1 + 1] = yi;
                        a[k1] = xr;
                        a[k1 + 1] = xi;
                    }

                    j1 = 2 * k + m2 + ip[offset + k];
                    k1 = j1 + m2;
                    xr = a[j1];
                    xi = a[j1 + 1];
                    yr = a[k1];
                    yi = a[k1 + 1];
                    a[j1] = yr;
                    a[j1 + 1] = yi;
                    a[k1] = xr;
                    a[k1 + 1] = xi;
                }
            }
            else
            {
                for (var k = 1; k < m; k++)
                {
                    for (var j = 0; j < k; j++)
                    {
                        j1 = 2 * j + ip[offset + k];
                        k1 = 2 * k + ip[offset + j];
                        xr = a[j1];
                        xi = a[j1 + 1];
                        yr = a[k1];
                        yi = a[k1 + 1];
                        a[j1] = yr;
                        a[j1 + 1] = yi;
                        a[k1] = xr;
                        a[k1 + 1] = xi;
                        j1 += m2;
                        k1 += m2;
                        xr = a[j1];
                        xi = a[j1 + 1];
                        yr = a[k1];
                        yi = a[k1 + 1];
                        a[j1] = yr;
                        a[j1 + 1] = yi;
                        a[k1] = xr;
                        a[k1 + 1] = xi;
                    }
                }
            }
        }

        private static void Bitrv2Conj(int n, int[] ip, int offset, double[] a)
        {
            int j1, k1;
            double xr, xi, yr, yi;

            ip[offset] = 0;
            var l = n;
            var m = 1;

            while (m << 3 < l)
            {
                l >>= 1;

                for (var j = 0; j < m; j++)
                {
                    ip[offset + m + j] = ip[offset + j] + l;
                }

                m <<= 1;
            }

            var m2 = 2 * m;

            if (m << 3 == l)
            {
                for (var k = 0; k < m; k++)
                {
                    for (var j = 0; j < k; j++)
                    {
                        j1 = 2 * j + ip[offset + k];
                        k1 = 2 * k + ip[offset + j];
                        xr = a[j1];
                        xi = -a[j1 + 1];
                        yr = a[k1];
                        yi = -a[k1 + 1];
                        a[j1] = yr;
                        a[j1 + 1] = yi;
                        a[k1] = xr;
                        a[k1 + 1] = xi;
                        j1 += m2;
                        k1 += 2 * m2;
                        xr = a[j1];
                        xi = -a[j1 + 1];
                        yr = a[k1];
                        yi = -a[k1 + 1];
                        a[j1] = yr;
                        a[j1 + 1] = yi;
                        a[k1] = xr;
                        a[k1 + 1] = xi;
                        j1 += m2;
                        k1 -= m2;
                        xr = a[j1];
                        xi = -a[j1 + 1];
                        yr = a[k1];
                        yi = -a[k1 + 1];
                        a[j1] = yr;
                        a[j1 + 1] = yi;
                        a[k1] = xr;
                        a[k1 + 1] = xi;
                        j1 += m2;
                        k1 += 2 * m2;
                        xr = a[j1];
                        xi = -a[j1 + 1];
                        yr = a[k1];
                        yi = -a[k1 + 1];
                        a[j1] = yr;
                        a[j1 + 1] = yi;
                        a[k1] = xr;
                        a[k1 + 1] = xi;
                    }

                    k1 = 2 * k + ip[offset + k];
                    a[k1 + 1] = -a[k1 + 1];
                    j1 = k1 + m2;
                    k1 = j1 + m2;
                    xr = a[j1];
                    xi = -a[j1 + 1];
                    yr = a[k1];
                    yi = -a[k1 + 1];
                    a[j1] = yr;
                    a[j1 + 1] = yi;
                    a[k1] = xr;
                    a[k1 + 1] = xi;
                    k1 += m2;
                    a[k1 + 1] = -a[k1 + 1];
                }
            }
            else
            {
                a[1] = -a[1];
                a[m2 + 1] = -a[m2 + 1];

                for (var k = 1; k < m; k++)
                {
                    for (var j = 0; j < k; j++)
                    {
                        j1 = 2 * j + ip[offset + k];
                        k1 = 2 * k + ip[offset + j];
                        xr = a[j1];
                        xi = -a[j1 + 1];
                        yr = a[k1];
                        yi = -a[k1 + 1];
                        a[j1] = yr;
                        a[j1 + 1] = yi;
                        a[k1] = xr;
                        a[k1 + 1] = xi;
                        j1 += m2;
                        k1 += m2;
                        xr = a[j1];
                        xi = -a[j1 + 1];
                        yr = a[k1];
                        yi = -a[k1 + 1];
                        a[j1] = yr;
                        a[j1 + 1] = yi;
                        a[k1] = xr;
                        a[k1 + 1] = xi;
                    }

                    k1 = 2 * k + ip[offset + k];
                    a[k1 + 1] = -a[k1 + 1];
                    a[k1 + m2 + 1] = -a[k1 + m2 + 1];
                }
            }
        }

        private static void Cftfsub(int n, double[] a, double[] w)
        {
            var l = 2;

            if (n > 8)
            {
                Cft1St(n, a, w);
                l = 8;
                while (l << 2 < n)
                {
                    Cftmdl(n, l, a, w);
                    l <<= 2;
                }
            }

            if (l << 2 == n)
            {
                for (var j = 0; j < l; j += 2)
                {
                    var j1 = j + l;
                    var j2 = j1 + l;
                    var j3 = j2 + l;
                    var x0R = a[j] + a[j1];
                    var x0I = a[j + 1] + a[j1 + 1];
                    var x1R = a[j] - a[j1];
                    var x1I = a[j + 1] - a[j1 + 1];
                    var x2R = a[j2] + a[j3];
                    var x2I = a[j2 + 1] + a[j3 + 1];
                    var x3R = a[j2] - a[j3];
                    var x3I = a[j2 + 1] - a[j3 + 1];
                    a[j] = x0R + x2R;
                    a[j + 1] = x0I + x2I;
                    a[j2] = x0R - x2R;
                    a[j2 + 1] = x0I - x2I;
                    a[j1] = x1R - x3I;
                    a[j1 + 1] = x1I + x3R;
                    a[j3] = x1R + x3I;
                    a[j3 + 1] = x1I - x3R;
                }
            }
            else
            {
                for (var j = 0; j < l; j += 2)
                {
                    var j1 = j + l;
                    var x0R = a[j] - a[j1];
                    var x0I = a[j + 1] - a[j1 + 1];
                    a[j] += a[j1];
                    a[j + 1] += a[j1 + 1];
                    a[j1] = x0R;
                    a[j1 + 1] = x0I;
                }
            }
        }

        private static void Cftbsub(int n, double[] a, double[] w)
        {
            //void cft1st(int n, double *a, double *w);
            //void cftmdl(int n, int l, double *a, double *w);

            var l = 2;

            if (n > 8)
            {
                Cft1St(n, a, w);
                l = 8;

                while (l << 2 < n)
                {
                    Cftmdl(n, l, a, w);
                    l <<= 2;
                }
            }

            if (l << 2 == n)
            {
                for (var j = 0; j < l; j += 2)
                {
                    var j1 = j + l;
                    var j2 = j1 + l;
                    var j3 = j2 + l;
                    var x0R = a[j] + a[j1];
                    var x0I = -a[j + 1] - a[j1 + 1];
                    var x1R = a[j] - a[j1];
                    var x1I = -a[j + 1] + a[j1 + 1];
                    var x2R = a[j2] + a[j3];
                    var x2I = a[j2 + 1] + a[j3 + 1];
                    var x3R = a[j2] - a[j3];
                    var x3I = a[j2 + 1] - a[j3 + 1];
                    a[j] = x0R + x2R;
                    a[j + 1] = x0I - x2I;
                    a[j2] = x0R - x2R;
                    a[j2 + 1] = x0I + x2I;
                    a[j1] = x1R - x3I;
                    a[j1 + 1] = x1I - x3R;
                    a[j3] = x1R + x3I;
                    a[j3 + 1] = x1I + x3R;
                }
            }
            else
            {
                for (var j = 0; j < l; j += 2)
                {
                    var j1 = j + l;
                    var x0R = a[j] - a[j1];
                    var x0I = -a[j + 1] + a[j1 + 1];
                    a[j] += a[j1];
                    a[j + 1] = -a[j + 1] - a[j1 + 1];
                    a[j1] = x0R;
                    a[j1 + 1] = x0I;
                }
            }
        }

        private static void Cft1St(int n, double[] a, double[] w)
        {
            var x0R = a[0] + a[2];
            var x0I = a[1] + a[3];
            var x1R = a[0] - a[2];
            var x1I = a[1] - a[3];
            var x2R = a[4] + a[6];
            var x2I = a[5] + a[7];
            var x3R = a[4] - a[6];
            var x3I = a[5] - a[7];
            a[0] = x0R + x2R;
            a[1] = x0I + x2I;
            a[4] = x0R - x2R;
            a[5] = x0I - x2I;
            a[2] = x1R - x3I;
            a[3] = x1I + x3R;
            a[6] = x1R + x3I;
            a[7] = x1I - x3R;
            var wk1R = w[2];
            x0R = a[8] + a[10];
            x0I = a[9] + a[11];
            x1R = a[8] - a[10];
            x1I = a[9] - a[11];
            x2R = a[12] + a[14];
            x2I = a[13] + a[15];
            x3R = a[12] - a[14];
            x3I = a[13] - a[15];
            a[8] = x0R + x2R;
            a[9] = x0I + x2I;
            a[12] = x2I - x0I;
            a[13] = x0R - x2R;
            x0R = x1R - x3I;
            x0I = x1I + x3R;
            a[10] = wk1R * (x0R - x0I);
            a[11] = wk1R * (x0R + x0I);
            x0R = x3I + x1R;
            x0I = x3R - x1I;
            a[14] = wk1R * (x0I - x0R);
            a[15] = wk1R * (x0I + x0R);
            var k1 = 0;

            for (var j = 16; j < n; j += 16)
            {
                k1 += 2;
                var k2 = 2 * k1;
                var wk2R = w[k1];
                var wk2I = w[k1 + 1];
                wk1R = w[k2];
                var wk1I = w[k2 + 1];
                var wk3R = wk1R - 2 * wk2I * wk1I;
                var wk3I = 2 * wk2I * wk1R - wk1I;
                x0R = a[j] + a[j + 2];
                x0I = a[j + 1] + a[j + 3];
                x1R = a[j] - a[j + 2];
                x1I = a[j + 1] - a[j + 3];
                x2R = a[j + 4] + a[j + 6];
                x2I = a[j + 5] + a[j + 7];
                x3R = a[j + 4] - a[j + 6];
                x3I = a[j + 5] - a[j + 7];
                a[j] = x0R + x2R;
                a[j + 1] = x0I + x2I;
                x0R -= x2R;
                x0I -= x2I;
                a[j + 4] = wk2R * x0R - wk2I * x0I;
                a[j + 5] = wk2R * x0I + wk2I * x0R;
                x0R = x1R - x3I;
                x0I = x1I + x3R;
                a[j + 2] = wk1R * x0R - wk1I * x0I;
                a[j + 3] = wk1R * x0I + wk1I * x0R;
                x0R = x1R + x3I;
                x0I = x1I - x3R;
                a[j + 6] = wk3R * x0R - wk3I * x0I;
                a[j + 7] = wk3R * x0I + wk3I * x0R;
                wk1R = w[k2 + 2];
                wk1I = w[k2 + 3];
                wk3R = wk1R - 2 * wk2R * wk1I;
                wk3I = 2 * wk2R * wk1R - wk1I;
                x0R = a[j + 8] + a[j + 10];
                x0I = a[j + 9] + a[j + 11];
                x1R = a[j + 8] - a[j + 10];
                x1I = a[j + 9] - a[j + 11];
                x2R = a[j + 12] + a[j + 14];
                x2I = a[j + 13] + a[j + 15];
                x3R = a[j + 12] - a[j + 14];
                x3I = a[j + 13] - a[j + 15];
                a[j + 8] = x0R + x2R;
                a[j + 9] = x0I + x2I;
                x0R -= x2R;
                x0I -= x2I;
                a[j + 12] = -wk2I * x0R - wk2R * x0I;
                a[j + 13] = -wk2I * x0I + wk2R * x0R;
                x0R = x1R - x3I;
                x0I = x1I + x3R;
                a[j + 10] = wk1R * x0R - wk1I * x0I;
                a[j + 11] = wk1R * x0I + wk1I * x0R;
                x0R = x1R + x3I;
                x0I = x1I - x3R;
                a[j + 14] = wk3R * x0R - wk3I * x0I;
                a[j + 15] = wk3R * x0I + wk3I * x0R;
            }
        }

        private static void Cftmdl(int n, int l, double[] a, double[] w)
        {
            var m = l << 2;

            for (var j = 0; j < l; j += 2)
            {
                var j1 = j + l;
                var j2 = j1 + l;
                var j3 = j2 + l;
                var x0R = a[j] + a[j1];
                var x0I = a[j + 1] + a[j1 + 1];
                var x1R = a[j] - a[j1];
                var x1I = a[j + 1] - a[j1 + 1];
                var x2R = a[j2] + a[j3];
                var x2I = a[j2 + 1] + a[j3 + 1];
                var x3R = a[j2] - a[j3];
                var x3I = a[j2 + 1] - a[j3 + 1];
                a[j] = x0R + x2R;
                a[j + 1] = x0I + x2I;
                a[j2] = x0R - x2R;
                a[j2 + 1] = x0I - x2I;
                a[j1] = x1R - x3I;
                a[j1 + 1] = x1I + x3R;
                a[j3] = x1R + x3I;
                a[j3 + 1] = x1I - x3R;
            }

            var wk1R = w[2];

            for (var j = m; j < l + m; j += 2)
            {
                var j1 = j + l;
                var j2 = j1 + l;
                var j3 = j2 + l;
                var x0R = a[j] + a[j1];
                var x0I = a[j + 1] + a[j1 + 1];
                var x1R = a[j] - a[j1];
                var x1I = a[j + 1] - a[j1 + 1];
                var x2R = a[j2] + a[j3];
                var x2I = a[j2 + 1] + a[j3 + 1];
                var x3R = a[j2] - a[j3];
                var x3I = a[j2 + 1] - a[j3 + 1];
                a[j] = x0R + x2R;
                a[j + 1] = x0I + x2I;
                a[j2] = x2I - x0I;
                a[j2 + 1] = x0R - x2R;
                x0R = x1R - x3I;
                x0I = x1I + x3R;
                a[j1] = wk1R * (x0R - x0I);
                a[j1 + 1] = wk1R * (x0R + x0I);
                x0R = x3I + x1R;
                x0I = x3R - x1I;
                a[j3] = wk1R * (x0I - x0R);
                a[j3 + 1] = wk1R * (x0I + x0R);
            }

            var k1 = 0;
            var m2 = 2 * m;

            for (var k = m2; k < n; k += m2)
            {
                k1 += 2;
                var k2 = 2 * k1;
                var wk2R = w[k1];
                var wk2I = w[k1 + 1];
                wk1R = w[k2];
                var wk1I = w[k2 + 1];
                var wk3R = wk1R - 2 * wk2I * wk1I;
                var wk3I = 2 * wk2I * wk1R - wk1I;

                for (var j = k; j < l + k; j += 2)
                {
                    var j1 = j + l;
                    var j2 = j1 + l;
                    var j3 = j2 + l;
                    var x0R = a[j] + a[j1];
                    var x0I = a[j + 1] + a[j1 + 1];
                    var x1R = a[j] - a[j1];
                    var x1I = a[j + 1] - a[j1 + 1];
                    var x2R = a[j2] + a[j3];
                    var x2I = a[j2 + 1] + a[j3 + 1];
                    var x3R = a[j2] - a[j3];
                    var x3I = a[j2 + 1] - a[j3 + 1];
                    a[j] = x0R + x2R;
                    a[j + 1] = x0I + x2I;
                    x0R -= x2R;
                    x0I -= x2I;
                    a[j2] = wk2R * x0R - wk2I * x0I;
                    a[j2 + 1] = wk2R * x0I + wk2I * x0R;
                    x0R = x1R - x3I;
                    x0I = x1I + x3R;
                    a[j1] = wk1R * x0R - wk1I * x0I;
                    a[j1 + 1] = wk1R * x0I + wk1I * x0R;
                    x0R = x1R + x3I;
                    x0I = x1I - x3R;
                    a[j3] = wk3R * x0R - wk3I * x0I;
                    a[j3 + 1] = wk3R * x0I + wk3I * x0R;
                }

                wk1R = w[k2 + 2];
                wk1I = w[k2 + 3];
                wk3R = wk1R - 2 * wk2R * wk1I;
                wk3I = 2 * wk2R * wk1R - wk1I;

                for (var j = k + m; j < l + k + m; j += 2)
                {
                    var j1 = j + l;
                    var j2 = j1 + l;
                    var j3 = j2 + l;
                    var x0R = a[j] + a[j1];
                    var x0I = a[j + 1] + a[j1 + 1];
                    var x1R = a[j] - a[j1];
                    var x1I = a[j + 1] - a[j1 + 1];
                    var x2R = a[j2] + a[j3];
                    var x2I = a[j2 + 1] + a[j3 + 1];
                    var x3R = a[j2] - a[j3];
                    var x3I = a[j2 + 1] - a[j3 + 1];
                    a[j] = x0R + x2R;
                    a[j + 1] = x0I + x2I;
                    x0R -= x2R;
                    x0I -= x2I;
                    a[j2] = -wk2I * x0R - wk2R * x0I;
                    a[j2 + 1] = -wk2I * x0I + wk2R * x0R;
                    x0R = x1R - x3I;
                    x0I = x1I + x3R;
                    a[j1] = wk1R * x0R - wk1I * x0I;
                    a[j1 + 1] = wk1R * x0I + wk1I * x0R;
                    x0R = x1R + x3I;
                    x0I = x1I - x3R;
                    a[j3] = wk3R * x0R - wk3I * x0I;
                    a[j3 + 1] = wk3R * x0I + wk3I * x0R;
                }
            }
        }

        private static void Rftfsub(int n, double[] a, int nc, double[] c, int offset)
        {
            var m = n >> 1;
            var ks = 2 * nc / m;
            var kk = 0;

            for (var j = 2; j < m; j += 2)
            {
                var k = n - j;
                kk += ks;
                var wkr = 0.5 - c[offset + nc - kk];
                var wki = c[offset + kk];
                var xr = a[j] - a[k];
                var xi = a[j + 1] + a[k + 1];
                var yr = wkr * xr - wki * xi;
                var yi = wkr * xi + wki * xr;
                a[j] -= yr;
                a[j + 1] -= yi;
                a[k] += yr;
                a[k + 1] -= yi;
            }
        }

        private static void Rftbsub(int n, double[] a, int nc, double[] c, int offset)
        {
            a[1] = -a[1];
            var m = n >> 1;
            var ks = 2 * nc / m;
            var kk = 0;

            for (var j = 2; j < m; j += 2)
            {
                var k = n - j;
                kk += ks;
                var wkr = 0.5 - c[offset + nc - kk];
                var wki = c[offset + kk];
                var xr = a[j] - a[k];
                var xi = a[j + 1] + a[k + 1];
                var yr = wkr * xr + wki * xi;
                var yi = wkr * xi - wki * xr;
                a[j] -= yr;
                a[j + 1] = yi - a[j + 1];
                a[k] += yr;
                a[k + 1] = yi - a[k + 1];
            }

            a[m + 1] = -a[m + 1];
        }
        #endregion
    }
}
