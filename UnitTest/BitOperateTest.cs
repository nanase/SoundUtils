using NUnit.Framework;
using SoundUtils;

namespace UnitTest
{
    [TestFixture]
    public class BitOperateTest
    {
        [Test]
        public void ReverseBytesInt64Test()
        {
            unchecked
            {
                const long valLong = 0x0123456789ABCDEF;
                const long valLongReverse = (long)0xEFCDAB8967452301;

                Assert.That(BitOperate.ReverseBytes(valLong, true), Is.EqualTo(valLongReverse));
                Assert.That(BitOperate.ReverseBytes(valLong, false), Is.EqualTo(valLong));
                Assert.That(BitOperate.ReverseBytes(BitOperate.ReverseBytes(valLong, true), true), Is.EqualTo(valLong));
            }
        }

        [Test]
        public void ReverseBytesInt32Test()
        {
            const int valInt = 0x01234567;
            const int valIntReverse = 0x67452301;

            Assert.That(BitOperate.ReverseBytes(valInt, true), Is.EqualTo(valIntReverse));
            Assert.That(BitOperate.ReverseBytes(valInt, false), Is.EqualTo(valInt));
            Assert.That(BitOperate.ReverseBytes(BitOperate.ReverseBytes(valInt, true), true), Is.EqualTo(valInt));
        }

        [Test]
        public void ReverseBytesInt16Test()
        {
            const short valShort = 0x0123;
            const short valShortReverse = 0x2301;

            Assert.That(BitOperate.ReverseBytes(valShort, true), Is.EqualTo(valShortReverse));
            Assert.That(BitOperate.ReverseBytes(valShort, false), Is.EqualTo(valShort));
            Assert.That(BitOperate.ReverseBytes(BitOperate.ReverseBytes(valShort, true), true), Is.EqualTo(valShort));
        }
    }
}
