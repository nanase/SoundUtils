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
    public class WaveFormatWriter : IDisposable
    {
        #region -- Private Fields --
        private bool disposed;
        #endregion

        #region -- Public Properties --
        public Stream BaseStream { get; private set; }

        public int SamplingRate { get; private set; }

        public int BitPerSample { get; private set; }

        public int ChannelCount { get; private set; }

        public long WrittenBytes { get; private set; }

        public long WrittenSamples { get; private set; }
        #endregion

        #region -- Constructor --
        public WaveFormatWriter(Stream stream, int samplingRate, int bitPerSample, int channelCount)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            if (!stream.CanSeek || !stream.CanWrite)
                throw new InvalidOperationException();

            if (samplingRate <= 0)
                throw new ArgumentOutOfRangeException("samplingRate");

            if (bitPerSample != 8 && bitPerSample != 16)
                throw new ArgumentOutOfRangeException("bitPerSample");

            if (channelCount < 1 || channelCount > 2)
                throw new ArgumentOutOfRangeException("channelCount");

            this.BaseStream = stream;
            this.SamplingRate = samplingRate;
            this.BitPerSample = bitPerSample;
            this.ChannelCount = channelCount;

            this.BaseStream.Seek(44L, SeekOrigin.Begin);
        }
        #endregion

        #region -- Public Methods --
        public void Write(byte[] buffer, int offset, int count)
        {
            if (this.disposed)
                throw new ObjectDisposedException("BaseStream");

            this.BaseStream.Write(buffer, offset, count);
            this.WrittenBytes += count;
        }

        unsafe public void Write(short[] buffer, int offset, int count)
        {
            if (this.disposed)
                throw new ObjectDisposedException("BaseStream");

            byte[] buf;

            if (this.BitPerSample == 8)
            {
                buf = new byte[count];

                for (int i = offset, j = 0, length = offset + count; i < length; i++, j++)
                    buf[j] = (byte)Math.Round((buffer[i] / 65536.0 + 0.5) * 255);

                this.BaseStream.Write(buf, 0, count);
                this.WrittenBytes += count;
            }
            else
            {
                buf = new byte[count * 2];
                short tmp;
                byte* b0 = (byte*)&tmp, b1 = b0 + 1;

                if (BitConverter.IsLittleEndian)
                    for (int i = offset, j = 0, length = offset + count; i < length; i++)
                    {
                        tmp = buffer[i];
                        buf[j++] = *b0;
                        buf[j++] = *b1;
                    }
                else
                    for (int i = offset, j = 0, length = offset + count; i < length; i++)
                    {
                        tmp = buffer[i];
                        buf[j++] = *b1;
                        buf[j++] = *b0;
                    }

                this.BaseStream.Write(buf, 0, count * 2);
                this.WrittenBytes += count * 2;
            }
        }

        unsafe public void Write(double[] buffer, int offset, int count)
        {
            if (this.disposed)
                throw new ObjectDisposedException("BaseStream");

            byte[] buf;
            double dtmp;

            if (this.BitPerSample == 8)
            {
                buf = new byte[count];

                for (int i = offset, j = 0, length = offset + count; i < length; i++, j++)
                {
                    dtmp = buffer[i];
                    if (double.IsNaN(dtmp) || double.IsInfinity(dtmp))
                        continue;
                    else if (dtmp > 1.0)
                        buf[j] = 255;
                    else if (dtmp < -1.0)
                        buf[j] = 0;
                    else
                        buf[j] = (byte)Math.Round((dtmp + 1.0) * 127.5);
                }

                this.BaseStream.Write(buf, 0, count);
                this.WrittenBytes += count;
            }
            else
            {
                buf = new byte[count * 2];
                short tmp;
                byte* b0 = (byte*)&tmp, b1 = b0 + 1;

                if (BitConverter.IsLittleEndian)
                    for (int i = offset, j = 0, length = offset + count; i < length; i++)
                    {
                        dtmp = buffer[i];
                        if (double.IsNaN(dtmp) || double.IsInfinity(dtmp))
                        {
                            j += 2;
                            continue;
                        }
                        else if (dtmp > 1.0)
                            tmp = short.MaxValue;
                        else if (dtmp < -1.0)
                            tmp = short.MinValue;
                        else
                            tmp = (short)(buffer[i] * 32767.5);

                        buf[j++] = *b0;
                        buf[j++] = *b1;
                    }
                else
                    for (int i = offset, j = 0, length = offset + count; i < length; i++)
                    {
                        dtmp = buffer[i];
                        if (double.IsNaN(dtmp) || double.IsInfinity(dtmp))
                        {
                            j += 2;
                            continue;
                        }
                        else if (dtmp > 1.0)
                            tmp = short.MaxValue;
                        else if (dtmp < -1.0)
                            tmp = short.MinValue;
                        else
                            tmp = (short)(buffer[i] * 32767.5);

                        buf[j++] = *b1;
                        buf[j++] = *b0;
                    }

                this.BaseStream.Write(buf, 0, count * 2);
                this.WrittenBytes += count * 2;
            }
        }

        unsafe public void Write(float[] buffer, int offset, int count)
        {
            if (this.disposed)
                throw new ObjectDisposedException("BaseStream");

            byte[] buf;
            float dtmp;

            if (this.BitPerSample == 8)
            {
                buf = new byte[count];

                for (int i = offset, j = 0, length = offset + count; i < length; i++, j++)
                {
                    dtmp = buffer[i];
                    if (float.IsNaN(dtmp) || float.IsInfinity(dtmp))
                        continue;
                    else if (dtmp > 1.0f)
                        buf[j] = 255;
                    else if (dtmp < -1.0f)
                        buf[j] = 0;
                    else
                        buf[j] = (byte)Math.Round((dtmp + 1.0f) * 127.5f);
                }

                this.BaseStream.Write(buf, 0, count);
                this.WrittenBytes += count;
            }
            else
            {
                buf = new byte[count * 2];
                short tmp;
                byte* b0 = (byte*)&tmp, b1 = b0 + 1;

                if (BitConverter.IsLittleEndian)
                    for (int i = offset, j = 0, length = offset + count; i < length; i++)
                    {
                        dtmp = buffer[i];
                        if (float.IsNaN(dtmp) || float.IsInfinity(dtmp))
                        {
                            j += 2;
                            continue;
                        }
                        else if (dtmp > 1.0f)
                            tmp = short.MaxValue;
                        else if (dtmp < -1.0f)
                            tmp = short.MinValue;
                        else
                            tmp = (short)(buffer[i] * 32767.5f);

                        buf[j++] = *b0;
                        buf[j++] = *b1;
                    }
                else
                    for (int i = offset, j = 0, length = offset + count; i < length; i++)
                    {
                        dtmp = buffer[i];
                        if (float.IsNaN(dtmp) || float.IsInfinity(dtmp))
                        {
                            j += 2;
                            continue;
                        }
                        else if (dtmp > 1.0)
                            tmp = short.MaxValue;
                        else if (dtmp < -1.0)
                            tmp = short.MinValue;
                        else
                            tmp = (short)(buffer[i] * 32767.5f);

                        buf[j++] = *b1;
                        buf[j++] = *b0;
                    }

                this.BaseStream.Write(buf, 0, count * 2);
                this.WrittenBytes += count * 2;
            }
        }

        /// <summary>
        /// このオブジェクトで使用されているリソースを解放します。
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Flush()
        {
            if (this.disposed)
                throw new ObjectDisposedException("BaseStream");

            bool little = BitConverter.IsLittleEndian;
            bool big = !little;

            this.BaseStream.Seek(0L, SeekOrigin.Begin);
            using (BinaryWriter bw = new BinaryWriter(this.BaseStream))
            {
                // 4 bytes, offset 4
                bw.Write(BitOperate.Reverse((int)0x52494646, little));

                // 4 bytes, offset 8
                bw.Write(BitOperate.Reverse((int)(this.WrittenBytes + 36), big));

                // 8 bytes, offset 16
                bw.Write(BitOperate.Reverse((long)0x57415645666D7420, little));

                // 4 bytes, offset 20
                bw.Write(BitOperate.Reverse((int)16, big));

                // 2 bytes, offset 22
                bw.Write(BitOperate.Reverse((short)1, big));

                // 2 bytes, offset 24
                bw.Write(BitOperate.Reverse((short)this.ChannelCount, big));

                // 4 bytes, offset 28
                bw.Write(BitOperate.Reverse((int)this.SamplingRate, big));

                // 4 bytes, offset 32
                bw.Write(BitOperate.Reverse((int)(this.SamplingRate * this.ChannelCount * (this.BitPerSample / 8)), big));

                // 2 bytes, offset 34
                bw.Write(BitOperate.Reverse((short)(this.ChannelCount * (this.BitPerSample / 8)), big));

                // 2 bytes, offset 36
                bw.Write(BitOperate.Reverse((short)this.BitPerSample, big));

                // 4 bytes, offset 40
                bw.Write(BitOperate.Reverse((int)0x64617461, little));

                // 4 bytes, offset 44
                bw.Write(BitOperate.Reverse((int)this.WrittenBytes, big));
            }
        }
        #endregion

        #region -- Protected Methods --
        /// <summary>
        /// このオブジェクトによって使用されているアンマネージリソースを解放し、オプションでマネージリソースも解放します。
        /// </summary>
        /// <param name="disposing">マネージリソースとアンマネージリソースの両方を解放する場合は true。アンマネージリソースだけを解放する場合は false。</param>
        protected void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                this.Flush();
                this.BaseStream.Dispose();

                this.disposed = true;
            }
        }
        #endregion
    }
}