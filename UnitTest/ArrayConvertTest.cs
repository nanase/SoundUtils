﻿/* SoundUtils

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
    public class ArrayConvertTest
    {
        [Test]
        public void ToDoubleTest()
        {
            var input = new[] { 3.0f, -2.5f, 1.76f, 2.97e-10f };
            var output = new double[4];
            var expect = new double[] { 3.0f, -2.5f, 1.76f, 2.97e-10f };

            input.ToDouble(output);
            Assert.That(output, Is.EqualTo(expect));

            Assert.That(() => ArrayConvert.ToDouble(null, output), Throws.ArgumentNullException);
            Assert.That(() => input.ToDouble(null), Throws.ArgumentNullException);
            Assert.That(() => new float[1].ToDouble(new double[0]), Throws.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void ToSingleTest()
        {
            var input = new[] { 3.0, -2.5, 1.76, 2.97e-10 };
            var output = new float[4];
            var expect = new[] { 3.0f, -2.5f, 1.76f, 2.97e-10f };

            input.ToSingle(output);
            Assert.That(output, Is.EqualTo(expect));

            Assert.That(() => ArrayConvert.ToSingle(null, output), Throws.ArgumentNullException);
            Assert.That(() => input.ToSingle(null), Throws.ArgumentNullException);
            Assert.That(() => new double[1].ToSingle(new float[0]), Throws.TypeOf<ArgumentOutOfRangeException>());
        }
    }
}
