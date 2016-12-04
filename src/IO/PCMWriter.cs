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
using System.IO;

namespace SoundUtils.IO
{
    /// <summary>
    /// PCM 形式でストリームに書き込むための機能を提供します。
    /// </summary>
    public class PcmWriter : WaveWriter
    {
        #region -- Constructor --
        /// <summary>
        /// ストリームとパラメータを指定して新しい PCMWriter クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="stream">書き込まれるストリーム。</param>
        /// <param name="samplingRate">サンプリング周波数。</param>
        /// <param name="bitPerSample">サンプルあたりのビット数。</param>
        /// <param name="channelCount">チャネルの数。</param>
        public PcmWriter(Stream stream, int samplingRate, int bitPerSample, int channelCount)
            : base(stream, samplingRate, bitPerSample, channelCount, 44L)
        {
            if (bitPerSample != 8 && bitPerSample != 16)
                throw new ArgumentOutOfRangeException(nameof(bitPerSample));

            if (channelCount < 1 || channelCount > 2)
                throw new ArgumentOutOfRangeException(nameof(channelCount));
        }
        #endregion

        #region -- Public Methods --
        /// <summary>
        /// 指定されたバッファをストリームに書き込みます。
        /// </summary>
        /// <param name="buffer">書き込まれるデータが格納された byte 型配列。</param>
        /// <param name="offset">書き込みを開始する配列のオフセット。</param>
        /// <param name="count">書き込まれるデータ数。</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            if (this.Disposed)
                throw new ObjectDisposedException("BaseStream");

            this.BaseStream.Write(buffer, offset, count);
            this.WrittenBytes += count;
        }

        /// <summary>
        /// 指定されたバッファをストリームに書き込みます。
        /// </summary>
        /// <param name="buffer">書き込まれるデータが格納された short 型配列。</param>
        /// <param name="offset">書き込みを開始する配列のオフセット。</param>
        /// <param name="count">書き込まれるデータ数。</param>
        public override unsafe void Write(short[] buffer, int offset, int count)
        {
            if (this.Disposed)
                throw new ObjectDisposedException("BaseStream");

            if (this.BitPerSample == 16)
                count *= 2;

            byte[] buf = new byte[count];

            if (this.BitPerSample == 8)
                ArrayConvert.RegulateAsInt8(buffer, offset, count, buf);
            else
                ArrayConvert.ToByte(buffer, offset, count, buf, BitConverter.IsLittleEndian);

            this.BaseStream.Write(buf, offset, count);
            this.WrittenBytes += count;
        }

        /// <summary>
        /// 指定されたバッファをストリームに書き込みます。
        /// </summary>
        /// <param name="buffer">書き込まれるデータが格納された double 型配列。</param>
        /// <param name="offset">書き込みを開始する配列のオフセット。</param>
        /// <param name="count">書き込まれるデータ数。</param>
        public override unsafe void Write(double[] buffer, int offset, int count)
        {
            if (this.Disposed)
                throw new ObjectDisposedException("BaseStream");

            if (this.BitPerSample == 16)
                count *= 2;

            byte[] buf = new byte[count];

            if (this.BitPerSample == 8)
                ArrayConvert.RegulateAsInt8(buffer, offset, count, buf);
            else
                ArrayConvert.RegulateAsInt16(buffer, offset, count, buf, BitConverter.IsLittleEndian);

            this.BaseStream.Write(buf, offset, count);
            this.WrittenBytes += count;
        }

        /// <summary>
        /// 指定されたバッファをストリームに書き込みます。
        /// </summary>
        /// <param name="buffer">書き込まれるデータが格納された float 型配列。</param>
        /// <param name="offset">書き込みを開始する配列のオフセット。</param>
        /// <param name="count">書き込まれるデータ数。</param>
        public override unsafe void Write(float[] buffer, int offset, int count)
        {
            if (this.Disposed)
                throw new ObjectDisposedException("BaseStream");

            if (this.BitPerSample == 16)
                count *= 2;

            byte[] buf = new byte[count];

            if (this.BitPerSample == 8)
                ArrayConvert.RegulateAsInt8(buffer, offset, count, buf);
            else
                ArrayConvert.RegulateAsInt16(buffer, offset, count, buf, BitConverter.IsLittleEndian);

            this.BaseStream.Write(buf, offset, count);
            this.WrittenBytes += count;
        }

        /// <summary>
        /// ストリームに対応するすべてのバッファーをクリアし、バッファー内のデータを基になるデバイスに書き込みます。
        /// </summary>
        public override void Flush()
        {
            if (this.Disposed)
                throw new ObjectDisposedException("BaseStream");

            bool little = BitConverter.IsLittleEndian;
            bool big = !little;
            long position = this.BaseStream.Position;

            this.BaseStream.Seek(0L, SeekOrigin.Begin);

            using (BinaryWriter bw = new BinaryWriter(this.BaseStream))
            {
                // 4 bytes, offset 4
                bw.Write(BitOperate.ReverseBytes((int)0x52494646, little));

                // 4 bytes, offset 8
                bw.Write(BitOperate.ReverseBytes((int)(this.WrittenBytes + 36), big));

                // 8 bytes, offset 16
                bw.Write(BitOperate.ReverseBytes((long)0x57415645666D7420, little));

                // 4 bytes, offset 20
                bw.Write(BitOperate.ReverseBytes((int)16, big));

                // 2 bytes, offset 22
                bw.Write(BitOperate.ReverseBytes((short)1, big));

                // 2 bytes, offset 24
                bw.Write(BitOperate.ReverseBytes((short)this.ChannelCount, big));

                // 4 bytes, offset 28
                bw.Write(BitOperate.ReverseBytes((int)this.SamplingRate, big));

                // 4 bytes, offset 32
                bw.Write(BitOperate.ReverseBytes((int)(this.SamplingRate * this.ChannelCount * (this.BitPerSample / 8)), big));

                // 2 bytes, offset 34
                bw.Write(BitOperate.ReverseBytes((short)(this.ChannelCount * (this.BitPerSample / 8)), big));

                // 2 bytes, offset 36
                bw.Write(BitOperate.ReverseBytes((short)this.BitPerSample, big));

                // 4 bytes, offset 40
                bw.Write(BitOperate.ReverseBytes((int)0x64617461, little));

                // 4 bytes, offset 44
                bw.Write(BitOperate.ReverseBytes((int)this.WrittenBytes, big));
            }

            this.BaseStream.Seek(position, SeekOrigin.Begin);
            this.BaseStream.Flush();
        }
        #endregion
    }
}