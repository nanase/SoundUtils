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
