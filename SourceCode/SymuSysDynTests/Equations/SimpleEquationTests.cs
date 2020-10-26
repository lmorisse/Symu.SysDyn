using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Model;

namespace SymuSysDynTests.Equations
{
    [TestClass()]
    public class SimpleEquationTests
    {
        private const string Equation = "variable1";
        private const string PlusEquation = "variable1 + variable2";
        private const string MinusEquation = "variable1 - variable2";
        private const string MultiplicationEquation = "variable1 * variable2";
        private const string DivisionEquation = "variable1 / variable2";
        private const string MixEquation = "1 + variable1";
        private const string SameStartEquation = "variable1 / variable1_1";
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

        #region Evaluate
        [TestMethod()]
        public void EvaluateTest()
        {
            var variable = new Variable("X", PlusEquation);
            _variables.Add(variable);
            Assert.AreEqual(3, variable.Equation.Evaluate(_variables, null));
        }

        [TestMethod()]
        public void EvaluateTest1()
        {
            var variable = new Variable("X", MinusEquation);
            _variables.Add(variable);
            Assert.AreEqual(-1, variable.Equation.Evaluate(_variables, null));
        }

        [TestMethod()]
        public void EvaluateTest2()
        {
            var variable = new Variable("X", MultiplicationEquation);
            _variables.Add(variable);
            Assert.AreEqual(2, variable.Equation.Evaluate(_variables, null));
        }

        [TestMethod()]
        public void EvaluateTest3()
        {
            var variable = new Variable("X", DivisionEquation);
            _variables.Add(variable);
            Assert.AreEqual(0.5F, variable.Equation.Evaluate(_variables, null));
        }

        [TestMethod()]
        public void EvaluateTest4()
        {
            var variable = new Variable("X", Equation);
            _variables.Add(variable);
            Assert.AreEqual(1, variable.Equation.Evaluate(_variables, null));
        }

        [TestMethod()]
        public void EvaluateTest5()
        {
            var variable = new Variable("X", MixEquation);
            _variables.Add(variable);
            Assert.AreEqual(2, variable.Equation.Evaluate(_variables, null));
        }

        #endregion

        #region Replace
        [TestMethod()]
        public void ReplaceTest()
        {
            var variable = new Variable("X", PlusEquation);
            variable.Equation.Replace("Variable1", "1");
            variable.Equation.Replace("Variable2", "1");
            Assert.AreEqual(2, variable.Equation.InitialValue());
        }

        [TestMethod()]
        public void ReplaceTest1()
        {
            var variable = new Variable("X", MinusEquation);
            variable.Equation.Replace("Variable1", "2");
            variable.Equation.Replace("Variable2", "1");
            Assert.AreEqual(1, variable.Equation.InitialValue());
        }

        [TestMethod()]
        public void ReplaceTest2()
        {
            var variable = new Variable("X", MultiplicationEquation);
            variable.Equation.Replace("Variable1", "1");
            variable.Equation.Replace("Variable2", "1");
            Assert.AreEqual(1, variable.Equation.InitialValue());
        }

        [TestMethod()]
        public void ReplaceTest3()
        {
            var variable = new Variable("X", DivisionEquation);
            variable.Equation.Replace("Variable1", "1");
            variable.Equation.Replace("Variable2", "1");
            Assert.AreEqual(1, variable.Equation.InitialValue());
        }

        [TestMethod()]
        public void ReplaceTest4()
        {
            var variable = new Variable("X", Equation);
            variable.Equation.Replace("Variable1", "1");
            Assert.AreEqual(1, variable.Equation.InitialValue());
        }

        [TestMethod()]
        public void ReplaceTest5()
        {
            var variable = new Variable("X", MixEquation);
            variable.Equation.Replace("Variable1", "1");
            Assert.AreEqual(2, variable.Equation.InitialValue());
        }

        [TestMethod()]
        public void ReplaceTest6()
        {
            var variable = new Variable("X", SameStartEquation);
            variable.Equation.Replace("Variable1", "1");
            Assert.AreEqual("1/Variable1_1", variable.Equation.InitializedEquation);
        }
        #endregion
    }
}