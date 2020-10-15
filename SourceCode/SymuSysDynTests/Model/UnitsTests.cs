using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Model;

namespace SymuSysDynTests.Model
{
    [TestClass()]
    public class UnitsTests
    {
        /// <summary>
        /// Non passing tests
        /// </summary>
        [TestMethod()]
        public void CreateInstanceFromEquationTest()
        {
            Assert.IsNull(Units.CreateInstanceFromEquation(null));
            Assert.IsNull(Units.CreateInstanceFromEquation(string.Empty));
            Assert.IsNull(Units.CreateInstanceFromEquation("test"));
        }
        /// <summary>
        /// Passing test
        /// </summary>
        [TestMethod()]
        public void CreateInstanceFromEquationTest1()
        {
            var test = Units.CreateInstanceFromEquation("test {unit1/unit2}");
            Assert.IsNotNull(test);
            Assert.AreEqual("Unit1 / Unit2", test.Eqn);
        }
    }
}