using Microsoft.VisualStudio.TestTools.UnitTesting;
using SoundUtils;

namespace UnitTest
{
    [TestClass]
    public class ArrayConvertTest
    {
        [TestMethod]
        public void CastToDoubleTest()
        {
            var input = new float[] { 3.0f, -2.5f, 1.76f, 2.97e-10f };
            var output = new double[4];
            var expect = new double[] { 3.0f, -2.5f, 1.76f, 2.97e-10f };

            ArrayConvert.CastToDouble(input, output);
            CollectionAssert.AreEqual(expect, output);
        }

        [TestMethod]
        public void CastToSingleTest()
        {
            var input = new double[] { 3.0, -2.5, 1.76, 2.97e-10 };
            var output = new float[4];
            var expect = new float[] { 3.0f, -2.5f, 1.76f, 2.97e-10f };

            ArrayConvert.CastToSingle(input, output);
            CollectionAssert.AreEqual(expect, output);
        }
    }
}
