#region Licence

// Description: SymuBiz - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Engine;
using Symu.SysDyn.Models;
using Symu.SysDyn.Models.XMile;

#endregion

namespace SymuSysDynTests.Equations
{
    [TestClass]
    public class ComplexEquationTests
    {
        private const string NormalEquation = "Normal(variable1,0)";
        private const string MinEquation = "Min(variable1,variable2)";
        private const string PlusEquation = "Normal(variable1+variable2,0)";
        private const string BracketsEquation = "((variable1)/(variable2))";
        private const string SameVariableEquation = "((variable1)+(variable1))";
        private const string SameStartEquation = "((variable1)+(variable1_1))";
        private readonly StateMachine _machine = new StateMachine();
        private readonly Variable _variable1 = new Variable("Variable1");
        private readonly Variable _variable2 = new Variable("Variable2");
        private XMileModel Model => _machine.Models.RootModel;
        private VariableCollection Variables => Model.Variables;

        [TestInitialize]
        public void Initialize()
        {
            _variable1.Value = 1;
            _variable2.Value = 2;
            Variables.Add(_variable1);
            Variables.Add(_variable2);
        }

        [TestMethod]
        public void CloneTest()
        {
            var variable = Variable.CreateInstance("X", Model, NormalEquation);
            var cloneEquation = variable.Equation.Clone();
            Assert.AreEqual(1, cloneEquation.Evaluate(null, Variables, null));
        }

        #region Evaluate

        [TestMethod]
        public void EvaluateTest()
        {
            var variable = Variable.CreateInstance("X", Model, NormalEquation);
            Assert.AreEqual(1, variable.Equation.Evaluate(null, Variables, null));
        }

        [TestMethod]
        public void EvaluateTest1()
        {
            var variable = Variable.CreateInstance("X", Model, MinEquation);
            Assert.AreEqual(1, variable.Equation.Evaluate(null, Variables, null));
        }

        [TestMethod]
        public void EvaluateTest3()
        {
            var variable = Variable.CreateInstance("X", Model, PlusEquation);
            Assert.AreEqual(3, variable.Equation.Evaluate(null, Variables, null));
        }

        [TestMethod]
        public void EvaluateTest4()
        {
            var variable = Variable.CreateInstance("X", Model, BracketsEquation);
            Assert.AreEqual(0.5F, variable.Equation.Evaluate(null, Variables, null));
        }

        #endregion

        #region Replace

        [TestMethod]
        public void ReplaceTest()
        {
            var variable = Variable.CreateInstance("X", Model, NormalEquation);
            variable.Equation.Replace("_Variable1", "1", _machine.Simulation);
            Assert.AreEqual(1, variable.Equation.InitialValue());
        }

        [TestMethod]
        public void ReplaceTest1()
        {
            var variable = Variable.CreateInstance("X", Model, MinEquation);
            variable.Equation.Replace("_Variable1", "1", _machine.Simulation);
            variable.Equation.Replace("_Variable2", "2", _machine.Simulation);
            Assert.AreEqual(1, variable.Equation.InitialValue());
        }

        [TestMethod]
        public void ReplaceTest2()
        {
            var variable = Variable.CreateInstance("X", Model, PlusEquation);
            variable.Equation.Replace("_Variable1", "1", _machine.Simulation);
            variable.Equation.Replace("_Variable2", "2", _machine.Simulation);
            Assert.AreEqual(3, variable.Equation.InitialValue());
        }

        [TestMethod]
        public void ReplaceTest3()
        {
            var variable = Variable.CreateInstance("X", Model, BracketsEquation);
            variable.Equation.Replace("_Variable1", "1", _machine.Simulation);
            variable.Equation.Replace("_Variable2", "1", _machine.Simulation);
            Assert.AreEqual(1, variable.Equation.InitialValue());
        }

        [TestMethod]
        public void ReplaceTest4()
        {
            var variable = Variable.CreateInstance("X", Model, SameVariableEquation);
            variable.Equation.Replace("_Variable1", "1", _machine.Simulation);
            Assert.AreEqual(2, variable.Equation.InitialValue());
        }

        [TestMethod]
        public void ReplaceTest5()
        {
            var variable = Variable.CreateInstance("X", Model, SameStartEquation);
            variable.Equation.Replace("_Variable1", "1", _machine.Simulation);
            Assert.AreEqual("((1)+(_Variable1_1))", variable.Equation.InitializedEquation);
        }

        #endregion
    }
}