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
