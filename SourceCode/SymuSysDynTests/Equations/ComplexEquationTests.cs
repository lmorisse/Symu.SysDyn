using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Equations;
using Symu.SysDyn.Model;

namespace SymuSysDynTests.Equations
{
    [TestClass()]
    public class ComplexEquationTests
    {
        private const string NormalEquation = "Normal(variable1,0)";
        private const string MinEquation = "Min(variable1,variable2)";
        private const string PlusEquation = "Normal(variable1+variable2,0)";
        private const string BracketsEquation = "((variable1)/(variable2))";
        private const string SameVariableEquation = "((variable1)+(variable1))";
        private const string SameStartEquation = "((variable1)+(variable1_1))";
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
        public void CloneTest()
        {
            var variable = new Variable("X", NormalEquation);
            _variables.Add(variable);
            var cloneEquation = variable.Equation.Clone();
            Assert.AreEqual(1, cloneEquation.Evaluate(null, _variables, null));
        }
        #region Evaluate
        [TestMethod()]
        public void EvaluateTest()
        {
            var variable = new Variable("X", NormalEquation);
            _variables.Add(variable);
            Assert.AreEqual(1, variable.Equation.Evaluate(null, _variables, null));
        }

        [TestMethod()]
        public void EvaluateTest1()
        {
            var variable = new Variable("X", MinEquation);
            _variables.Add(variable);
            Assert.AreEqual(1, variable.Equation.Evaluate(null, _variables, null));
        }

        [TestMethod()]
        public void EvaluateTest3()
        {
            var variable = new Variable("X", PlusEquation);
            _variables.Add(variable);
            Assert.AreEqual(3, variable.Equation.Evaluate(null, _variables, null));
        }

        [TestMethod()]
        public void EvaluateTest4()
        {
            var variable = new Variable("X", BracketsEquation);
            _variables.Add(variable);
            Assert.AreEqual(0.5F, variable.Equation.Evaluate(null, _variables, null));
        }
        #endregion

        #region Replace

        [TestMethod()]
        public void ReplaceTest()
        {
            var variable = new Variable("X", NormalEquation);
            variable.Equation.Replace("Variable1", "1");
            Assert.AreEqual(1, variable.Equation.InitialValue());
        }
        [TestMethod()]
        public void ReplaceTest1()
        {
            var variable = new Variable("X", MinEquation);
            variable.Equation.Replace("Variable1", "1");
            variable.Equation.Replace("Variable2", "2");
            Assert.AreEqual(1, variable.Equation.InitialValue());
        }
        [TestMethod()]
        public void ReplaceTest2()
        {
            var variable = new Variable("X", PlusEquation);
            variable.Equation.Replace("Variable1", "1");
            variable.Equation.Replace("Variable2", "2");
            Assert.AreEqual(3, variable.Equation.InitialValue());
        }

        [TestMethod()]
        public void ReplaceTest3()
        {
            var variable = new Variable("X", BracketsEquation);
            variable.Equation.Replace("Variable1", "1");
            variable.Equation.Replace("Variable2", "1");
            Assert.AreEqual(1, variable.Equation.InitialValue());
        }

        [TestMethod()]
        public void ReplaceTest4()
        {
            var variable = new Variable("X", SameVariableEquation);
            variable.Equation.Replace("Variable1", "1");
            Assert.AreEqual(2, variable.Equation.InitialValue());
        }

        [TestMethod()]
        public void ReplaceTest5()
        {
            var variable = new Variable("X", SameStartEquation);
            variable.Equation.Replace("Variable1", "1");
            Assert.AreEqual("((1)+(Variable1_1))", variable.Equation.InitializedEquation);
        }
        #endregion
    }
}