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
using System.Linq;
using NUnit.Framework;
using SoundUtils;

namespace UnitTest
{
    [TestFixture]
    public class ChannelTest
    {
        [Test]
        public void SplitTest()
        {
            var src = Enumerable.Range(0, 8).ToArray();
            var lch = new int[4];
            var rch = new int[4];

            src.Split(lch, rch);

            Assert.That(lch, Is.EqualTo(new[] { 0, 2, 4, 6 }));
            Assert.That(rch, Is.EqualTo(new[] { 1, 3, 5, 7 }));

            Assert.That(() => src.Split(null, null), Throws.ArgumentNullException);
            Assert.That(() => Channel.Split(null, lch, null), Throws.ArgumentNullException);
            Assert.That(() => Channel.Split(null, null, rch), Throws.ArgumentNullException);
            Assert.That(() => new int[2].Split(new int[0], new int[0]), Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(() => new int[0].Split(new int[1], new int[1]), Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(() => new int[2].Split(new int[1], new int[0]), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void JoinTest()
        {
            var lch = Enumerable.Range(0, 4).ToArray();
            var rch = Enumerable.Range(4, 4).ToArray();
            var dst = new int[8];

            lch.Join(rch, dst);

            Assert.That(dst, Is.EqualTo(new[] { 0, 4, 1, 5, 2, 6, 3, 7 }));

            Assert.That(() => lch.Join(null, null), Throws.ArgumentNullException);
            Assert.That(() => Channel.Join(null, rch, null), Throws.ArgumentNullException);
            Assert.That(() => Channel.Join(null, null, dst), Throws.ArgumentNullException);
            Assert.That(() => new int[1].Join(new int[1], new int[0]), Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(() => new int[0].Join(new int[1], new int[1]), Throws.TypeOf<ArgumentOutOfRangeException>());
            Assert.That(() => new int[1].Join(new int[0], new int[1]), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void InterleaveTest1()
        {
            var src = Enumerable.Range(0, 4).ToArray();
            var dst = new int[8];

            src.Interleave(dst, 4);

            Assert.That(dst, Is.EqualTo(new[] { 0, 0, 1, 0, 2, 0, 3, 0 }));

            Array.Clear(dst, 0, 8);
            src.Interleave(2, dst, 0, 2);

            Assert.That(dst, Is.EqualTo(new[] { 2, 0, 3, 0, 0, 0, 0, 0 }));
        }

        [Test]
        public void InterleaveTest2()
        {
            var srcR = Enumerable.Range(0, 4).ToArray();
            var srcI = Enumerable.Range(0, 4).ToArray();
            var dst = new int[8];

            Channel.Interleave(srcR, srcI, dst, 4);

            Assert.That(dst, Is.EqualTo(new[] { 0, 0, 1, 1, 2, 2, 3, 3 }));

            Array.Clear(dst, 0, 8);
            Channel.Interleave(srcR, 1, srcI, 2, dst, 0, 2);

            Assert.That(dst, Is.EqualTo(new[] { 1, 2, 2, 3, 0, 0, 0, 0 }));
        }

        [Test]
        public void DeinterleaveTest1()
        {
            var src = Enumerable.Range(0, 8).ToArray();
            var dst = new int[4];

            src.Deinterleave(dst, 4);

            Assert.That(dst, Is.EqualTo(new[] { 0, 2, 4, 6 }));

            Array.Clear(dst, 0, 4);
            src.Deinterleave(1, dst, 0, 3);

            Assert.That(dst, Is.EqualTo(new[] { 1, 3, 5, 0 }));
        }

        [Test]
        public void DeinterleaveTest2()
        {
            var src = Enumerable.Range(0, 8).ToArray();
            var dstR = new int[4];
            var dstI = new int[4];

            src.Deinterleave(dstR, dstI, 4);

            Assert.That(dstR, Is.EqualTo(new[] { 0, 2, 4, 6 }));
            Assert.That(dstI, Is.EqualTo(new[] { 1, 3, 5, 7 }));

            Array.Clear(dstR, 0, 4);
            Array.Clear(dstI, 0, 4);
            src.Deinterleave(1, dstR, 0, dstI, 0, 3);

            Assert.That(dstR, Is.EqualTo(new[] { 1, 3, 5, 0 }));
            Assert.That(dstI, Is.EqualTo(new[] { 2, 4, 6, 0 }));
        }
    }
}
