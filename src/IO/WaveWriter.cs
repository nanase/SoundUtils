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
    public abstract class WaveWriter : IDisposable
    {
        #region -- Private Fields --
        private bool disposed;
        #endregion

        #region -- Public Properties --
        public Stream BaseStream { get; private set; }

        public int SamplingRate { get; private set; }

        public int BitPerSample { get; private set; }

        public int ChannelCount { get; private set; }

        public long WrittenBytes { get; protected set; }

        public long WrittenSamples { get; protected set; }

        public bool Disposed { get { return this.disposed; } }
        #endregion

        #region -- Constructor --
        public WaveWriter(Stream stream, int samplingRate, int bitPerSample, int channelCount, long entryOffset = 0L)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            if (!stream.CanSeek || !stream.CanWrite)
                throw new InvalidOperationException();

            if (samplingRate <= 0)
                throw new ArgumentOutOfRangeException("samplingRate");

            if (bitPerSample < 8)
                throw new ArgumentOutOfRangeException("bitPerSample");

            if (channelCount < 1)
                throw new ArgumentOutOfRangeException("channelCount");

            this.BaseStream = stream;
            this.SamplingRate = samplingRate;
            this.BitPerSample = bitPerSample;
            this.ChannelCount = channelCount;

            this.BaseStream.Seek(entryOffset, SeekOrigin.Begin);
        }
        #endregion

        #region -- Public Methods --
        public abstract void Write(byte[] buffer, int offset, int count);

        public abstract void Write(short[] buffer, int offset, int count);

        public abstract void Write(float[] buffer, int offset, int count);

        public abstract void Write(double[] buffer, int offset, int count);

        public abstract void Flush();

        /// <summary>
        /// このオブジェクトで使用されているリソースを解放します。
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
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