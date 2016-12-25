using System;
using System.Collections.Generic;
using NUnit.Framework;
using SoundUtils;

namespace UnitTest
{
    using CaseType = Tuple<Action<double[]>, IEnumerable<double>>;

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
            Assert.That(() => testCase.Item1(null), Throws.ArgumentNullException);

            var emptyArray = new double[0];
            testCase.Item1(emptyArray);
            Assert.That(emptyArray, Is.Empty);

            var testArray = new[] { 1.0, 1.0, 1.0 };
            testCase.Item1(testArray);
            Assert.That(testArray, Is.EqualTo(testCase.Item2).Within(1e-10));
        }
    }
}
