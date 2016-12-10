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
            var src0 = Enumerable.Range(0, 8).ToArray();
            var lch = new int[4];
            var rch = new int[4];

            Channel.Split(src0, lch, rch);

            Assert.That(lch, Is.EqualTo(new[] { 0, 2, 4, 6 }));
            Assert.That(rch, Is.EqualTo(new[] { 1, 3, 5, 7 }));
        }

        [Test]
        public void JoinTest()
        {
            var lch = Enumerable.Range(0, 4).ToArray();
            var rch = Enumerable.Range(4, 4).ToArray();
            var dst0 = new int[8];

            Channel.Join(lch, rch, dst0);
            
            Assert.That(dst0, Is.EqualTo(new[] { 0, 4, 1, 5, 2, 6, 3, 7 }));
        }

        [Test]
        public void InterleaveTest1()
        {
            var src = Enumerable.Range(0, 4).ToArray();
            var dst = new int[8];

            Channel.Interleave(src, dst, 4);
            
            Assert.That(dst, Is.EqualTo(new[] { 0, 0, 1, 0, 2, 0, 3, 0 }));

            Array.Clear(dst, 0, 8);
            Channel.Interleave(src, 2, dst, 0, 2);

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

            Channel.Deinterleave(src, dst, 4);
            
            Assert.That(dst, Is.EqualTo(new[] { 0, 2, 4, 6 }));

            Array.Clear(dst, 0, 4);
            Channel.Deinterleave(src, 1, dst, 0, 3);

            Assert.That(dst, Is.EqualTo(new[] { 1, 3, 5, 0 }));
        }

        [Test]
        public void DeinterleaveTest2()
        {
            var src = Enumerable.Range(0, 8).ToArray();
            var dstR = new int[4];
            var dstI = new int[4];

            Channel.Deinterleave(src, dstR, dstI, 4);

            Assert.That(dstR, Is.EqualTo(new[] { 0, 2, 4, 6 }));
            Assert.That(dstI, Is.EqualTo(new[] { 1, 3, 5, 7 }));

            Array.Clear(dstR, 0, 4);
            Array.Clear(dstI, 0, 4);
            Channel.Deinterleave(src, 1, dstR, 0, dstI, 0, 3);

            Assert.That(dstR, Is.EqualTo(new[] { 1, 3, 5, 0 }));
            Assert.That(dstI, Is.EqualTo(new[] { 2, 4, 6, 0 }));
        }
    }
}
