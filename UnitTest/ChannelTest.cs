using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoundUtils;

namespace UnitTest
{
    [TestClass]
    public class ChannelTest
    {
        [TestMethod]
        public void SplitTest()
        {
            var src0 = Enumerable.Range(0, 8).ToArray();
            var src1 = Enumerable.Range(0, 7).ToArray();
            var lch = new int[4];
            var rch = new int[4];

            Channel.Split(src0, lch, rch);

            CollectionAssert.AreEqual(new[] { 0, 2, 4, 6 }, lch);
            CollectionAssert.AreEqual(new[] { 1, 3, 5, 7 }, rch);

            Array.Clear(lch, 0, 4);
            Array.Clear(rch, 0, 4);

            Channel.Split(src1, lch, rch);

            CollectionAssert.AreEqual(new[] { 0, 2, 4, 0 }, lch);
            CollectionAssert.AreEqual(new[] { 1, 3, 5, 0 }, rch);
        }

        [TestMethod]
        public void JoinTest()
        {
            var lch = Enumerable.Range(0, 4).ToArray();
            var rch = Enumerable.Range(4, 4).ToArray();
            var dst0 = new int[8];

            Channel.Join(lch, rch, dst0);

            CollectionAssert.AreEqual(new[] { 0, 4, 1, 5, 2, 6, 3, 7 }, dst0);
        }

        [TestMethod]
        public void InterleaveTest1()
        {
            var src = Enumerable.Range(0, 4).ToArray();
            var dst = new int[8];

            Channel.Interleave(src, dst, 4);

            CollectionAssert.AreEqual(new[] { 0, 0, 1, 0, 2, 0, 3, 0 }, dst);

            Array.Clear(dst, 0, 8);
            Channel.Interleave(src, 2, dst, 0, 2);

            CollectionAssert.AreEqual(new[] { 2, 0, 3, 0, 0, 0, 0, 0 }, dst);
        }

        [TestMethod]
        public void InterleaveTest2()
        {
            var srcR = Enumerable.Range(0, 4).ToArray();
            var srcI = Enumerable.Range(0, 4).ToArray();
            var dst = new int[8];

            Channel.Interleave(srcR, srcI, dst, 4);

            CollectionAssert.AreEqual(new[] { 0, 0, 1, 1, 2, 2, 3, 3 }, dst);

            Array.Clear(dst, 0, 8);
            Channel.Interleave(srcR, 1, srcI, 2, dst, 0, 2);

            CollectionAssert.AreEqual(new[] { 1, 2, 2, 3, 0, 0, 0, 0 }, dst);
        }
    }
}
