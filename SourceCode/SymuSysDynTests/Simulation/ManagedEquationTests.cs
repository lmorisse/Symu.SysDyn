using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Simulation;

namespace SymuSysDynTests.Simulation
{
    [TestClass()]
    public class ManagedEquationTests
    {
        private const string Equation1 = "variable1 + variable2";
        private const string Equation2 = "name + DT *(variable1 - variable2)";
        [TestMethod()]
        public void InitializeTest()
        {
            Assert.AreEqual("Variable1 + Variable2", ManagedEquation.Initialize(Equation1));
        }
        [TestMethod()]
        public void InitializeTest2()
        {
            Assert.AreEqual("Name + Dt * ( Variable1 - Variable2 )", ManagedEquation.Initialize(Equation2));
        }
        [TestMethod()]
        public void InitializeTest3()
        {
            Assert.AreEqual("Variable1 + Variable2", ManagedEquation.Initialize(Equation1 + " {test}"));
        }
    }
}