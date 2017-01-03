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
using System.Collections.Generic;
using NUnit.Framework;
using SoundUtils;

namespace UnitTest
{
    internal class CaseType
    {
        public Action<double[]> WindowMethod { get; }
        public IEnumerable<double> ConvolutedResult { get;}

        public CaseType(Action<double[]> windowMethod, IEnumerable<double> convolutedResult)
        {
            WindowMethod = windowMethod;
            ConvolutedResult = convolutedResult;
        }

        public override string ToString() => $"{WindowMethod.Method.Name}: {string.Join(", ", ConvolutedResult)}";
    }

    [TestFixture]
    internal class WindowTest
    {
        private static readonly IEnumerable<CaseType> WindowTestCase = new List<CaseType>
        {
            new CaseType(Window.Hanning, new[] { 0.25, 1.0, 0.25 }),
            new CaseType(Window.Hamming, new[] { 0.31, 1.0, 0.31 }),
            new CaseType(Window.Bartlett, new[] { 1.0 / 3.0, 1.0, 1.0 / 3.0 }),
            new CaseType(Window.Nuttall, new[] { 0.052558, 1.0, 0.052558 }),
            new CaseType(Window.Blackman, new[] { 0.13, 1.0, 0.13 }),
            new CaseType(Window.BlackmanHarris, new[] { 0.055645, 1.0, 0.055645 }),
            new CaseType(Window.BlackmanNuttall, new[] { 0.0613345, 1.0, 0.0613345 }),
            new CaseType(Window.FlatTop, new[] { -0.238, 4.64, -0.238 }),
            new CaseType(Window.Welch, new[] { 5.0 / 9.0, 1.0, 5.0 / 9.0 }),
        };

        [Test]
        [TestCaseSource(typeof(WindowTest), nameof(WindowTestCase))]
        public void NoParameterWindowTest(CaseType testCase)
        {
            Assert.That(() => testCase.WindowMethod(null), Throws.ArgumentNullException);

            var emptyArray = new double[0];
            testCase.WindowMethod(emptyArray);
            Assert.That(emptyArray, Is.Empty);

            var testArray = new[] { 1.0, 1.0, 1.0 };
            testCase.WindowMethod(testArray);
            Assert.That(testArray, Is.EqualTo(testCase.ConvolutedResult).Within(1e-10));
        }
    }
}
