using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Model;

namespace SymuSysDynTests.Equations
{
    [TestClass()]
    public class ConstantEquationTests
    {
        private const string Equation = "1";
        private readonly Variables _variables = new Variables();

        [TestMethod()]
        public void EvaluateTest()
        {
            var variable = new Variable("X", Equation);
            _variables.Add(variable);
            Assert.AreEqual(1, variable.Equation.Evaluate(_variables, null));
        }

    }
}