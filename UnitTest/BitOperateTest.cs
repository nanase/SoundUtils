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

                Assert.That(valLong.ReverseBytes(), Is.EqualTo(valLongReverse));
                Assert.That(valLong.ReverseBytes().ReverseBytes(), Is.EqualTo(valLong));
            }
        }

        [Test]
        public void ReverseBytesInt32Test()
        {
            const int valInt = 0x01234567;
            const int valIntReverse = 0x67452301;

            Assert.That(valInt.ReverseBytes(), Is.EqualTo(valIntReverse));
            Assert.That(valInt.ReverseBytes().ReverseBytes(), Is.EqualTo(valInt));
        }

        [Test]
        public void ReverseBytesInt16Test()
        {
            const short valShort = 0x0123;
            const short valShortReverse = 0x2301;

            Assert.That(valShort.ReverseBytes(), Is.EqualTo(valShortReverse));
            Assert.That(valShort.ReverseBytes().ReverseBytes(), Is.EqualTo(valShort));
        }
    }
}
