using System.Linq;
using SystemAnalyzer.Matrices;
using SystemAnalyzer.Utils;
using NUnit.Framework;

namespace SystemAnalyzer.Tests
{
    internal class UtilTests
    {
        [Test, TestCase(19, 2)]
        public void MinorSizeTest(int matrixSize, int minorSize)
        {
            var loops = new CombinationIterator(init: 0, max: matrixSize - 1, loopCount: minorSize);
            var count = MathUtils.Combinations(matrixSize, minorSize);

            Assert.AreEqual(count, loops.Iterations.Count());
        }
    }
}
