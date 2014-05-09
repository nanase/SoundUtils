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
                const long val_long = (long)0x0123456789ABCDEF;
                const long val_long_reverse = (long)0xEFCDAB8967452301;

                Assert.AreEqual(val_long_reverse, BitOperate.ReverseBytes(val_long, true));
                Assert.AreEqual(val_long, BitOperate.ReverseBytes(val_long, false));
                Assert.AreEqual(val_long, BitOperate.ReverseBytes(BitOperate.ReverseBytes(val_long, true), true));
            }
        }

        [TestMethod]
        public void ReverseBytesInt32Test()
        {
            unchecked
            {
                const int val_int = (int)0x01234567;
                const int val_int_reverse = (int)0x67452301;

                Assert.AreEqual(val_int_reverse, BitOperate.ReverseBytes(val_int, true));
                Assert.AreEqual(val_int, BitOperate.ReverseBytes(val_int, false));
                Assert.AreEqual(val_int, BitOperate.ReverseBytes(BitOperate.ReverseBytes(val_int, true), true));
            }
        }

        [TestMethod]
        public void ReverseBytesInt16Test()
        {
            unchecked
            {
                const short val_short = (short)0x0123;
                const short val_short_reverse = (short)0x2301;

                Assert.AreEqual(val_short_reverse, BitOperate.ReverseBytes(val_short, true));
                Assert.AreEqual(val_short, BitOperate.ReverseBytes(val_short, false));
                Assert.AreEqual(val_short, BitOperate.ReverseBytes(BitOperate.ReverseBytes(val_short, true), true));
            }
        }
    }
}
