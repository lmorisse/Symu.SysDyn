using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Functions;

namespace SymuSysDynTests.Functions
{
    [TestClass()]
    public class NormalTests : BaseClassTest
    {
        [TestMethod]
        public void NormalTest()
        {
            var function = new Normal("Normal(1, 2)");
            Assert.AreEqual("1", function.Mean);
            Assert.AreEqual("2", function.StandardDeviation);
            Assert.AreEqual(string.Empty, function.Seed);
        }
        [TestMethod]
        public void NormalTest1()
        {
            var function = new Normal("Normal(1, 2, 3)");
            Assert.AreEqual("1", function.Mean);
            Assert.AreEqual("2", function.StandardDeviation);
            Assert.AreEqual("3", function.Seed);
        }
        /// <summary>
        /// Without seed
        /// </summary>
        [TestMethod]
        public void EvaluateTest()
        {
            var function = new Normal("Normal(0, 0)");
            Assert.AreEqual(0, function.Evaluate(Machine.Variables, Machine.Simulation));
        }
        /// <summary>
        /// With seed
        /// </summary>
        [TestMethod]
        public void EvaluateTest1()
        {
            var function = new Normal("Normal(0, 0, 1)");
            Assert.AreEqual(0, function.Evaluate(Machine.Variables, Machine.Simulation));
        }
    }
}