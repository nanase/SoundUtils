using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoundUtils;
using SoundUtils.Filtering;
using SoundUtils.Filtering.FIR;

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
    }
}
