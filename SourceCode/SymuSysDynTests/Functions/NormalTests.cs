#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Functions;
using Symu.SysDyn.Model;

#endregion

namespace SymuSysDynTests.Functions
{
    [TestClass]
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
        ///     Without seed
        /// </summary>
        [TestMethod]
        public void EvaluateTest()
        {
            var function = new Normal("Normal(0, 0)");
            Assert.AreEqual(0, function.Evaluate(null, Machine.Variables, Machine.Simulation));
        }

        /// <summary>
        ///     With seed
        /// </summary>
        [TestMethod]
        public void EvaluateTest1()
        {
            var function = new Normal("Normal(0, 0, 1)");
            Assert.AreEqual(0, function.Evaluate(null, Machine.Variables, Machine.Simulation));
        }

        /// <summary>
        ///     With seed
        /// </summary>
        [TestMethod]
        public void EvaluateTest2()
        {
            var function = new Normal("Normal(variable1+variable2, 0)");
            Variable.CreateInstance(Machine.Variables, "variable1", "1");
            Variable.CreateInstance(Machine.Variables, "variable2", "2");
            Assert.AreEqual(3, function.Evaluate(null, Machine.Variables, Machine.Simulation));
        }

        /// <summary>
        ///     With seed
        /// </summary>
        [TestMethod]
        public void EvaluateTest3()
        {
            var function = new Normal("Normal(variable1, variable2)");
            Variable.CreateInstance(Machine.Variables, "variable1", "1");
            Variable.CreateInstance(Machine.Variables, "variable2", "0");
            Assert.AreEqual(1, function.Evaluate(null, Machine.Variables, Machine.Simulation));
        }
    }
}