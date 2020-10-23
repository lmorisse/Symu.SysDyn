using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Model;

namespace SymuSysDynTests.Equations
{
    [TestClass()]
    public class ComplexEquationTests
    {
        private const string NormalEquation = "Normal(variable1,0)";
        private const string MinEquation = "Min(variable1,variable2)";
        private const string PlusEquation = "Normal(variable1+variable2,0)";
        private readonly Variables _variables = new Variables();
        private readonly Variable _variable1 = new Variable("Variable1");
        private readonly Variable _variable2 = new Variable("Variable2");

        [TestInitialize]
        public void Initialize()
        {
            _variable1.Value = 1;
            _variable2.Value = 2;
            _variables.Add(_variable1);
            _variables.Add(_variable2);
        }

        [TestMethod()]
        public void EvaluateTest()
        {
            var variable = new Variable("X", NormalEquation);
            _variables.Add(variable);
            Assert.AreEqual(1, variable.Equation.Evaluate(_variables, null));
        }

        [TestMethod()]
        public void EvaluateTest1()
        {
            var variable = new Variable("X", MinEquation);
            _variables.Add(variable);
            Assert.AreEqual(1, variable.Equation.Evaluate(_variables, null));
        }

        [TestMethod()]
        public void EvaluateTest3()
        {
            var variable = new Variable("X", PlusEquation);
            _variables.Add(variable);
            Assert.AreEqual(3, variable.Equation.Evaluate(_variables, null));
        }

    }
}