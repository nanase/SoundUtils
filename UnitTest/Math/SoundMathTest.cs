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
