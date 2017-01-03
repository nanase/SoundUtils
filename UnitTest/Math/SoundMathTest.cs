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
using NUnit.Framework;
using SoundUtils;

namespace UnitTest
{
    [TestFixture]
    internal class SoundMathTest
    {
        [Test]
        public void SincTest()
        {
            Assert.That(SoundMath.Sinc(0.0), Is.EqualTo(1.0));
            Assert.That(SoundMath.Sinc(Math.PI / 2.0), Is.EqualTo(2.0 / Math.PI));
            Assert.That(SoundMath.Sinc(1.0), Is.EqualTo(SoundMath.Sinc(-1.0)));

            Assert.That(SoundMath.Sinc(double.PositiveInfinity), Is.NaN);
            Assert.That(SoundMath.Sinc(double.NegativeInfinity), Is.NaN);
            Assert.That(SoundMath.Sinc(double.NaN), Is.NaN);
        }

        [Test]
        public void InvertedFactorialTest()
        {
            Assert.That(SoundMath.InvertedFactorial(0), Is.EqualTo(1.0));
            Assert.That(SoundMath.InvertedFactorial(1), Is.EqualTo(1.0));
            Assert.That(SoundMath.InvertedFactorial(2), Is.EqualTo(0.5));
            Assert.That(SoundMath.InvertedFactorial(3), Is.EqualTo(0.5 / 3.0));

            Assert.That(() => SoundMath.InvertedFactorial(-1), Throws.TypeOf<ArgumentOutOfRangeException>());
        }
    }
}
