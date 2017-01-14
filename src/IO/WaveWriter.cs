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
using System.IO;

namespace SoundUtils.IO
{
    /// <summary>
    /// Wave 形式の書き込みを抽象化したクラスです。
    /// </summary>
    public abstract class WaveWriter : IDisposable
    {
        #region -- Public Properties --
        /// <summary>
        /// 継承されたクラスで書き込まれるストリームを取得します。
        /// </summary>
        public Stream BaseStream { get; }

        /// <summary>
        /// サンプリング周波数を取得します。単位は ヘルツ (Hz) です。
        /// </summary>
        public int SamplingRate { get; private set; }

        /// <summary>
        /// 1 サンプルあたりのビット数を取得します。
        /// </summary>
        public int BitPerSample { get; private set; }

        /// <summary>
        /// チャネルの数を取得します。
        /// </summary>
        public int ChannelCount { get; private set; }

        /// <summary>
        /// ストリームに書き込まれた累計バイト数を取得します。
        /// </summary>
        public long WrittenBytes { get; protected set; }

        /// <summary>
        /// ストリームに書き込まれた累計サンプル数を取得します。
        /// </summary>
        public long WrittenSamples { get; protected set; }

        /// <summary>
        /// このインスタンスが Dispose メソッドで破棄されたかの真偽値を取得します。
        /// </summary>
        public bool Disposed { get; private set; }

        #endregion

        #region -- Constructor --
        /// <summary>
        /// ストリームとパラメータを指定して新しい WaveWriter クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="stream">書き込まれるストリーム。</param>
        /// <param name="samplingRate">サンプリング周波数。</param>
        /// <param name="bitPerSample">サンプルあたりのビット数。</param>
        /// <param name="channelCount">チャネルの数。</param>
        /// <param name="entryOffset">書き込みを開始するバイトオフセット。</param>
        protected WaveWriter(Stream stream, int samplingRate, int bitPerSample, int channelCount, long entryOffset = 0L)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (!stream.CanSeek || !stream.CanWrite)
                throw new InvalidOperationException("シークができないか、読み取りのできないストリームは使用できません。");

            if (samplingRate <= 0)
                throw new ArgumentOutOfRangeException(nameof(samplingRate), "無効なサンプリング周波数が指定されました。サンプリング周波数は 1(Hz) 以上の整数である必要があります。");

            if (bitPerSample < 2)
                throw new ArgumentOutOfRangeException(nameof(bitPerSample), "無効なビット数が指定されました。ビット数は 2 以上である必要があります。");

            if (channelCount < 1)
                throw new ArgumentOutOfRangeException(nameof(channelCount), "無効なチャネル数が指定されました。チャネル数は 1 以上である必要があります。");

            BaseStream = stream;
            SamplingRate = samplingRate;
            BitPerSample = bitPerSample;
            ChannelCount = channelCount;

            BaseStream.Seek(entryOffset, SeekOrigin.Begin);
        }
        #endregion

        #region -- Public Methods --
        /// <summary>
        /// 指定されたバッファをストリームに書き込みます。
        /// </summary>
        /// <param name="buffer">書き込まれるデータが格納された byte 型配列。</param>
        /// <param name="offset">書き込みを開始する配列のオフセット。</param>
        /// <param name="count">書き込まれるデータ数。</param>
        public abstract void Write(byte[] buffer, int offset, int count);

        /// <summary>
        /// 指定されたバッファをストリームに書き込みます。
        /// </summary>
        /// <param name="buffer">書き込まれるデータが格納された short 型配列。</param>
        /// <param name="offset">書き込みを開始する配列のオフセット。</param>
        /// <param name="count">書き込まれるデータ数。</param>
        public abstract void Write(short[] buffer, int offset, int count);

        /// <summary>
        /// 指定されたバッファをストリームに書き込みます。
        /// </summary>
        /// <param name="buffer">書き込まれるデータが格納された float 型配列。</param>
        /// <param name="offset">書き込みを開始する配列のオフセット。</param>
        /// <param name="count">書き込まれるデータ数。</param>
        public abstract void Write(float[] buffer, int offset, int count);

        /// <summary>
        /// 指定されたバッファをストリームに書き込みます。
        /// </summary>
        /// <param name="buffer">書き込まれるデータが格納された double 型配列。</param>
        /// <param name="offset">書き込みを開始する配列のオフセット。</param>
        /// <param name="count">書き込まれるデータ数。</param>
        public abstract void Write(double[] buffer, int offset, int count);

        /// <summary>
        /// ストリームに対応するすべてのバッファーをクリアし、バッファー内のデータを基になるデバイスに書き込みます。
        /// </summary>
        public abstract void Flush();

        /// <summary>
        /// このオブジェクトで使用されているリソースを解放します。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion

        #region -- Protected Methods --
        /// <summary>
        /// このオブジェクトによって使用されているアンマネージリソースを解放し、オプションでマネージリソースも解放します。
        /// </summary>
        /// <param name="disposing">マネージリソースとアンマネージリソースの両方を解放する場合は true。アンマネージリソースだけを解放する場合は false。</param>
        protected virtual void Dispose(bool disposing)
        {
            if (Disposed)
                return;

            Flush();
            BaseStream.Dispose();

            Disposed = true;
        }
        #endregion
    }
}