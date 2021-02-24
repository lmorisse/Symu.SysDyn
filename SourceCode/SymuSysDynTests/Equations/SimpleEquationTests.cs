#region Licence

// Description: SymuBiz - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Core.Engine;
using Symu.SysDyn.Core.Models.XMile;

#endregion

namespace SymuSysDynTests.Equations
{
    [TestClass]
    public class SimpleEquationTests
    {
        private const string Equation = "variable1";
        private const string PlusEquation = "variable1 + variable2";
        private const string MinusEquation = "variable1 - variable2";
        private const string MultiplicationEquation = "variable1 * variable2";
        private const string DivisionEquation = "variable1 / variable2";
        private const string MixEquation = "1 + variable1";
        private const string SameStartEquation = "variable1 / variable1_1";
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
        public async Task CloneTest()
        {
            var variable = await Variable.CreateInstance<Auxiliary>("X", Model, PlusEquation);
            var cloneEquation = await variable.Equation.Clone();
            Assert.AreEqual(3, await cloneEquation.Evaluate(null, Variables, null));
        }

        #region Evaluate

        [TestMethod]
        public async Task EvaluateTest()
        {
            var variable = await Variable.CreateInstance<Auxiliary>("X", Model, PlusEquation);
            Assert.AreEqual(3, await variable.Equation.Evaluate(null, Variables, null));
        }

        [TestMethod]
        public async Task EvaluateTest1()
        {
            var variable = await Variable.CreateInstance<Auxiliary>("X", Model, MinusEquation);
            Assert.AreEqual(-1, await variable.Equation.Evaluate(null, Variables, null));
        }

        [TestMethod]
        public async Task EvaluateTest2()
        {
            var variable = await Variable.CreateInstance<Auxiliary>("X", Model, MultiplicationEquation);
            Assert.AreEqual(2, await variable.Equation.Evaluate(null, Variables, null));
        }

        [TestMethod]
        public async Task EvaluateTest3()
        {
            var variable = await Variable.CreateInstance<Auxiliary>("X", Model, DivisionEquation);
            Assert.AreEqual(0.5F, await variable.Equation.Evaluate(null, Variables, null));
        }

        [TestMethod]
        public async Task EvaluateTest4()
        {
            var variable = await Variable.CreateInstance<Auxiliary>("X", Model, Equation);
            Assert.AreEqual(1, await variable.Equation.Evaluate(null, Variables, null));
        }

        [TestMethod]
        public async Task EvaluateTest5()
        {
            var variable = await Variable.CreateInstance<Auxiliary>("X", Model, MixEquation);
            Assert.AreEqual(2, await variable.Equation.Evaluate(null, Variables, null));
        }

        #endregion

        #region Replace

        [TestMethod]
        public async Task ReplaceTest()
        {
            var variable = await Variable.CreateInstance<Auxiliary>("X", Model, PlusEquation);
            await variable.Equation.Replace("_Variable1", "1", _machine.Simulation);
            await variable.Equation.Replace("_Variable2", "1", _machine.Simulation);
            Assert.AreEqual(2, await variable.Equation.InitialValue());
        }

        [TestMethod]
        public async Task ReplaceTest1()
        {
            var variable = await Variable.CreateInstance<Auxiliary>("X", Model, MinusEquation);
            await variable.Equation.Replace("_Variable1", "2", _machine.Simulation);
            await variable.Equation.Replace("_Variable2", "1", _machine.Simulation);
            Assert.AreEqual(1, await variable.Equation.InitialValue());
        }

        [TestMethod]
        public async Task ReplaceTest2()
        {
            var variable = await Variable.CreateInstance<Auxiliary>("X", Model, MultiplicationEquation);
            await variable.Equation.Replace("_Variable1", "1", _machine.Simulation);
            await variable.Equation.Replace("_Variable2", "1", _machine.Simulation);
            Assert.AreEqual(1, await variable.Equation.InitialValue());
        }

        [TestMethod]
        public async Task ReplaceTest3()
        {
            var variable = await Variable.CreateInstance<Auxiliary>("X", Model, DivisionEquation);
            await variable.Equation.Replace("_Variable1", "1", _machine.Simulation);
            await variable.Equation.Replace("_Variable2", "1", _machine.Simulation);
            Assert.AreEqual(1, await variable.Equation.InitialValue());
        }

        [TestMethod]
        public async Task ReplaceTest4()
        {
            var variable = await Variable.CreateInstance<Auxiliary>("X", Model, Equation);
            await variable.Equation.Replace("_Variable1", "1", _machine.Simulation);
            Assert.AreEqual(1, await variable.Equation.InitialValue());
        }

        [TestMethod]
        public async Task ReplaceTest5()
        {
            var variable = await Variable.CreateInstance<Auxiliary>("X", Model, MixEquation);
            await variable.Equation.Replace("_Variable1", "1", _machine.Simulation);
            Assert.AreEqual(2, await variable.Equation.InitialValue());
        }

        [TestMethod]
        public async Task ReplaceTest6()
        {
            var variable = await Variable.CreateInstance<Auxiliary>("X", Model, SameStartEquation);
            await variable.Equation.Replace("_Variable1", "1", _machine.Simulation);
            Assert.AreEqual("1/_Variable1_1", variable.Equation.InitializedEquation);
        }

        #endregion
    }
}