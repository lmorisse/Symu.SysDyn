#region Licence

// Description: SymuBiz - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Core.Equations;

#endregion

namespace SymuSysDynTests.Equations
{
    [TestClass]
    public class EquationFactoryTests
    {
        private const string EmptyEquation = "";
        private const string ConstantEquation = "2";
        private const string ConstantEquation1 = "Abs(1)";
        private const string ConstantEquation2 = "Normal(1,0)"; // with builtin function
        private const string ConstantEquation3 = ".01";
        private const string ConstantEquation4 = ".45e6";
        private const string ConstantEquation5 = "1/10";
        private const string SimpleEquation = "variable_1 + variable2";
        private const string SimpleEquation1 = "variable_1 + Abs(1)"; // with builtin function
        private const string ComplexEquation = ".3 + name + Dt *(variable_1 - variable2)-TIME+SET(3,5) {test}";

        private const string
            ComplexEquation1 = "variable_1/(variable2*variable3)"; // not a simple equation because of the brackets

        private EquationFactoryStruct _factory;

        [TestMethod]
        public async Task CreateInstanceTest()
        {
            _factory =await  EquationFactory.CreateInstance(string.Empty, EmptyEquation);
            Assert.IsNull(_factory.Equation);
        }

        [TestMethod]
        public async Task CreateInstanceTest1()
        {
            _factory =await  EquationFactory.CreateInstance(string.Empty, ConstantEquation);

            Assert.IsNull(_factory.Equation);
            Assert.AreEqual(2, _factory.Value);
        }

        [TestMethod]
        public async Task CreateInstanceTest2()
        {
            _factory =await  EquationFactory.CreateInstance(string.Empty, ConstantEquation1);
            Assert.IsNull(_factory.Equation);
            Assert.AreEqual(1, _factory.Value);
        }

        [TestMethod]
        public async Task CreateInstanceTest3()
        {
            _factory =await  EquationFactory.CreateInstance(string.Empty, ConstantEquation2);
            Assert.IsNull(_factory.Equation);
            Assert.AreEqual(1, _factory.Value);
        }

        [TestMethod]
        public async Task CreateInstanceTest4()
        {
            _factory =await  EquationFactory.CreateInstance(string.Empty, ConstantEquation3);
            Assert.IsNull(_factory.Equation);
            Assert.AreEqual(0.01F, _factory.Value);
        }

        [TestMethod]
        public async Task CreateInstanceTest5()
        {
            _factory =await  EquationFactory.CreateInstance(string.Empty, ConstantEquation4);
            Assert.IsNull(_factory.Equation);
            Assert.AreEqual(450000, _factory.Value);
        }

        [TestMethod]
        public async Task CreateInstanceTest6()
        {
            _factory =await  EquationFactory.CreateInstance(string.Empty, ConstantEquation5);
            Assert.IsNull(_factory.Equation);
            Assert.AreEqual(0.1F, _factory.Value);
        }

        [TestMethod]
        public async Task CreateInstanceTest7()
        {
            _factory =await  EquationFactory.CreateInstance(string.Empty, SimpleEquation);
            Assert.IsInstanceOfType(_factory.Equation, typeof(SimpleEquation));
            Assert.AreEqual("_Variable_1+_Variable2", _factory.Equation.InitializedEquation);
            Assert.AreEqual(2, _factory.Equation.Variables.Count);
            Assert.AreEqual("_Variable_1", _factory.Equation.Variables[0]);
            Assert.AreEqual("_Variable2", _factory.Equation.Variables[1]);
        }

        [TestMethod]
        public async Task CreateInstanceTest8()
        {
            _factory =await  EquationFactory.CreateInstance(string.Empty, SimpleEquation1);
            Assert.IsInstanceOfType(_factory.Equation, typeof(SimpleEquation));
            Assert.AreEqual("_Variable_1+1", _factory.Equation.InitializedEquation);
            Assert.AreEqual(1, _factory.Equation.Variables.Count);
            Assert.AreEqual("_Variable_1", _factory.Equation.Variables[0]);
        }

        [TestMethod]
        public async Task CreateInstanceTest9()
        {
            _factory =await  EquationFactory.CreateInstance(string.Empty, ComplexEquation);
            Assert.IsInstanceOfType(_factory.Equation, typeof(ComplexEquation));
            Assert.AreEqual("0.3+_Name+Dt0*(_Variable_1-_Variable2)-Time1+Set2", _factory.Equation.InitializedEquation);
            Assert.AreEqual(3, _factory.Equation.Variables.Count);
            Assert.AreEqual("_Name", _factory.Equation.Variables[0]);
            Assert.AreEqual("_Variable_1", _factory.Equation.Variables[1]);
            Assert.AreEqual("_Variable2", _factory.Equation.Variables[2]);
        }

        [TestMethod]
        public async Task CreateInstanceTest10()
        {
            _factory =await  EquationFactory.CreateInstance(string.Empty, ComplexEquation1);
            Assert.IsInstanceOfType(_factory.Equation, typeof(ComplexEquation));
            Assert.AreEqual("_Variable_1/(_Variable2*_Variable3)", _factory.Equation.InitializedEquation);
            Assert.AreEqual(3, _factory.Equation.Variables.Count);
            Assert.AreEqual("_Variable_1", _factory.Equation.Variables[0]);
            Assert.AreEqual("_Variable2", _factory.Equation.Variables[1]);
            Assert.AreEqual("_Variable3", _factory.Equation.Variables[2]);
        }

        [TestMethod]
        public async Task GetVariablesTest()
        {
            _factory =await  EquationFactory.CreateInstance(string.Empty, ComplexEquation);

            Assert.AreEqual(3, _factory.Equation.Variables.Count);
            Assert.AreEqual("_Name", _factory.Equation.Variables[0]);
            Assert.AreEqual("_Variable_1", _factory.Equation.Variables[1]);
            Assert.AreEqual("_Variable2", _factory.Equation.Variables[2]);
        }


        [TestMethod]
        public async Task GetParametersTest5()
        {
            const string function = @"SMTH1((Junior_doctor's__base_salary*Annual__Pay_Change),.5)";
            _factory =await  EquationFactory.CreateInstance(string.Empty, function);
            Assert.AreEqual(2, _factory.Equation.Variables.Count);
            Assert.AreEqual("_Junior_doctors_base_salary", _factory.Equation.Variables[0]);
            Assert.AreEqual("_Annual_pay_change", _factory.Equation.Variables[1]);
        }
    }
}