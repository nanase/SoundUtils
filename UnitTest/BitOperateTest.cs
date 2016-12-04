using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoundUtils;

namespace UnitTest
{
    [TestClass]
    public class BitOperateTest
    {
        [TestMethod]
        public void ReverseBytesInt64Test()
        {
            unchecked
            {
                const long valLong = 0x0123456789ABCDEF;
                const long valLongReverse = (long)0xEFCDAB8967452301;

                Assert.AreEqual(valLongReverse, BitOperate.ReverseBytes(valLong, true));
                Assert.AreEqual(valLong, BitOperate.ReverseBytes(valLong, false));
                Assert.AreEqual(valLong, BitOperate.ReverseBytes(BitOperate.ReverseBytes(valLong, true), true));
            }
        }

        [TestMethod]
        public void ReverseBytesInt32Test()
        {
            unchecked
            {
                const int valInt = 0x01234567;
                const int valIntReverse = 0x67452301;

                Assert.AreEqual(valIntReverse, BitOperate.ReverseBytes(valInt, true));
                Assert.AreEqual(valInt, BitOperate.ReverseBytes(valInt, false));
                Assert.AreEqual(valInt, BitOperate.ReverseBytes(BitOperate.ReverseBytes(valInt, true), true));
            }
        }

        [TestMethod]
        public void ReverseBytesInt16Test()
        {
            unchecked
            {
                const short valShort = (short)0x0123;
                const short valShortReverse = (short)0x2301;

                Assert.AreEqual(valShortReverse, BitOperate.ReverseBytes(valShort, true));
                Assert.AreEqual(valShort, BitOperate.ReverseBytes(valShort, false));
                Assert.AreEqual(valShort, BitOperate.ReverseBytes(BitOperate.ReverseBytes(valShort, true), true));
            }
        }
    }
}
