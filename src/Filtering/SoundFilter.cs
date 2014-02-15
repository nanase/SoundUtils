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

namespace SoundUtils.Filtering
{
    /// <summary>
    /// モノラルまたはステレオの音声データにフィルタリングします。
    /// </summary>
    public class SoundFilter
    {
        #region -- Private Fields --
        private readonly FFTFiltering lfilter, rfilter;
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
            this.stereo = stereo;

            if (stereo)
            {
                bufferSize /= 2;
                this.rfilter = new FFTFiltering(bufferSize / 8, bufferSize / 8, bufferSize, bufferSize);
                this.rbuffer = new double[bufferSize];
            }

            this.lfilter = new FFTFiltering(bufferSize / 8, bufferSize / 8, bufferSize, bufferSize);
            this.lbuffer = new double[bufferSize];
        }
        #endregion

        #region -- Public Methods --
        /// <summary>
        /// 適用するフィルタを設定します。
        /// </summary>
        /// <param name="impulseResponses">フィルタのインパルス応答。</param>
        public void SetFilter(double[] impulseResponses)
        {
            this.lfilter.SetFilter(impulseResponses);

            if (this.stereo)
                this.rfilter.SetFilter(impulseResponses);
        }

        /// <summary>
        /// フィルタリングを行います。
        /// </summary>
        /// <param name="buffer">フィルタリングされるバッファ。</param>
        public void Filtering(double[] buffer)
        {
            if (this.stereo)
            {
                Channel.Split(buffer, this.lbuffer, this.rbuffer);
                this.lfilter.Apply(this.lbuffer);
                this.rfilter.Apply(this.rbuffer);
                Channel.Join(this.lbuffer, this.rbuffer, buffer);
            }
            else
            {
                this.lfilter.Apply(buffer);
            }
        }

        /// <summary>
        /// フィルタリングを行います。
        /// </summary>
        /// <param name="input">フィルタリングされる入力バッファ。</param>
        /// <param name="output">フィルタリングの結果が格納される出力バッファ。</param>
        public void Filtering(double[] input, double[] output)
        {
            if (this.stereo)
            {
                Channel.Split(input, this.lbuffer, this.rbuffer);
                this.lfilter.Apply(this.lbuffer);
                this.rfilter.Apply(this.rbuffer);
                Channel.Join(this.lbuffer, this.rbuffer, output);
            }
            else
            {
                input.CopyTo(output, 0);
                this.lfilter.Apply(output);
            }
        }
        #endregion
    }
}
