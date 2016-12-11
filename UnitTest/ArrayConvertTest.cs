using System;
using NUnit.Framework;
using SoundUtils;

namespace UnitTest
{
    [TestFixture]
    public class ArrayConvertTest
    {
        [Test]
        public void CastToDoubleTest()
        {
            var input = new[] { 3.0f, -2.5f, 1.76f, 2.97e-10f };
            var output = new double[4];
            var expect = new double[] { 3.0f, -2.5f, 1.76f, 2.97e-10f };

            ArrayConvert.CastToDouble(input, output);
            Assert.That(output, Is.EqualTo(expect));

            Assert.That(() => ArrayConvert.CastToDouble(null, output), Throws.ArgumentNullException);
            Assert.That(() => ArrayConvert.CastToDouble(input, null), Throws.ArgumentNullException);
            Assert.That(() => ArrayConvert.CastToDouble(new float[1], new double[0]), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void CastToSingleTest()
        {
            var input = new[] { 3.0, -2.5, 1.76, 2.97e-10 };
            var output = new float[4];
            var expect = new[] { 3.0f, -2.5f, 1.76f, 2.97e-10f };

            ArrayConvert.CastToSingle(input, output);
            Assert.That(output, Is.EqualTo(expect));

            Assert.That(() => ArrayConvert.CastToSingle(null, output), Throws.ArgumentNullException);
            Assert.That(() => ArrayConvert.CastToSingle(input, null), Throws.ArgumentNullException);
            Assert.That(() => ArrayConvert.CastToSingle(new double[1], new float[0]), Throws.TypeOf<ArgumentOutOfRangeException>());
        }
    }
}
