﻿/* SoundUtils

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

namespace SoundUtils.Filtering
{
    /// <summary>
    /// モノラルまたはステレオの音声データにフィルタリングします。
    /// </summary>
    public class SoundFilter
    {
        #region -- Private Fields --
        private readonly FftFiltering lfilter, rfilter;
        private readonly bool stereo;
        private readonly double[] lbuffer, rbuffer;
        #endregion

        #region -- Constructors --
        /// <summary>
        /// パラメータを指定して新しい SoundFilter クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="stereo">ステレオの場合は true、モノラルの場合は false。</param>
        /// <param name="bufferSize">フィルタが適用されるバッファのサイズ。
        /// stereo が true の場合、各チャネルのバッファサイズは bufferSize の半分となります。</param>
        public SoundFilter(bool stereo, int bufferSize)
        {
            if (bufferSize < 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));

            if (bufferSize % 2 != 0)
                throw new ArgumentOutOfRangeException(nameof(bufferSize));

            this.stereo = stereo;

            if (stereo)
            {
                bufferSize /= 2;
                rfilter = new FftFiltering(bufferSize / 8, bufferSize, bufferSize);
                rbuffer = new double[bufferSize];
            }

            lfilter = new FftFiltering(bufferSize / 8, bufferSize, bufferSize);
            lbuffer = new double[bufferSize];
        }
        #endregion

        #region -- Public Methods --
        /// <summary>
        /// 適用するフィルタを設定します。
        /// </summary>
        /// <param name="impulseResponses">フィルタのインパルス応答。</param>
        public void SetFilter(double[] impulseResponses)
        {
            if (impulseResponses == null)
                throw new ArgumentNullException(nameof(impulseResponses));

            lfilter.SetFilter(impulseResponses);

            if (stereo)
                rfilter.SetFilter(impulseResponses);
        }

        /// <summary>
        /// フィルタリングを行います。
        /// </summary>
        /// <param name="buffer">フィルタリングされるバッファ。</param>
        public void Filtering(double[] buffer)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            if (stereo)
            {
                buffer.Split(lbuffer, rbuffer);
                lfilter.Apply(lbuffer);
                rfilter.Apply(rbuffer);
                lbuffer.Join(rbuffer, buffer);
            }
            else
            {
                lfilter.Apply(buffer);
            }
        }

        /// <summary>
        /// フィルタリングを行います。
        /// </summary>
        /// <param name="input">フィルタリングされる入力バッファ。</param>
        /// <param name="output">フィルタリングの結果が格納される出力バッファ。</param>
        public void Filtering(double[] input, double[] output)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (output == null)
                throw new ArgumentNullException(nameof(output));

            if (input.Length != output.Length)
                throw new ArgumentOutOfRangeException(nameof(input));

            if (stereo)
            {
                input.Split(lbuffer, rbuffer);
                lfilter.Apply(lbuffer);
                rfilter.Apply(rbuffer);
                lbuffer.Join(rbuffer, output);
            }
            else
            {
                input.CopyTo(output, 0);
                lfilter.Apply(output);
            }
        }
        #endregion
    }
}
