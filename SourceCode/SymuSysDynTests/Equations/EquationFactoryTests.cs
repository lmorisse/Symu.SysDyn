#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;
using NCalc2;
using Symu.SysDyn.Equations;
using Symu.SysDyn.Functions;

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
        private const string ComplexEquation1 = "variable_1/(variable2*variable3)"; // not a simple equation because of the brackets
        private IEquation _equation;

        [TestMethod]
        public void CreateInstanceTest()
        {
            _equation = EquationFactory.CreateInstance(EmptyEquation, out _);
            Assert.IsNull(_equation);
        }
        [TestMethod]
        public void CreateInstanceTest1()
        {
            _equation = EquationFactory.CreateInstance(ConstantEquation, out var value);

            Assert.IsNull(_equation);
            Assert.AreEqual(2, value);

            //Assert.AreEqual("2", _equation.InitializedEquation);
            //Assert.AreEqual(0, _equation.Variables.Count);
            //Assert.IsInstanceOfType(_equation, typeof(ConstantEquation));
        }
        [TestMethod]
        public void CreateInstanceTest2()
        {
            _equation = EquationFactory.CreateInstance(ConstantEquation1, out var value);
            Assert.IsNull(_equation);
            Assert.AreEqual(1, value);
            //Assert.AreEqual("1", _equation.InitializedEquation);
            //Assert.AreEqual(0, _equation.Variables.Count);
            //Assert.IsInstanceOfType(_equation, typeof(ConstantEquation));
        }
        [TestMethod]
        public void CreateInstanceTest3()
        {
            _equation = EquationFactory.CreateInstance(ConstantEquation2, out var value);
            Assert.IsNull(_equation);
            Assert.AreEqual(1, value);
            //Assert.AreEqual("1", _equation.InitializedEquation);
            //Assert.AreEqual(0, _equation.Variables.Count);
            //Assert.IsInstanceOfType(_equation, typeof(ConstantEquation));
        }
        [TestMethod]
        public void CreateInstanceTest4()
        {
            _equation = EquationFactory.CreateInstance(ConstantEquation3, out var value);
            Assert.IsNull(_equation);
            Assert.AreEqual(0.01F, value);
            //Assert.AreEqual("0.01", _equation.InitializedEquation);
            //Assert.AreEqual(0, _equation.Variables.Count);
            //Assert.IsInstanceOfType(_equation, typeof(ConstantEquation));
        }
        [TestMethod]
        public void CreateInstanceTest5()
        {
            _equation = EquationFactory.CreateInstance(ConstantEquation4, out var value);
            Assert.IsNull(_equation);
            Assert.AreEqual(450000, value);
            //Assert.AreEqual("450000", _equation.InitializedEquation);
            //Assert.AreEqual(0, _equation.Variables.Count);
            //Assert.IsInstanceOfType(_equation, typeof(ConstantEquation));
        }
        [TestMethod]
        public void CreateInstanceTest6()
        {
            _equation = EquationFactory.CreateInstance(ConstantEquation5, out var value);
            Assert.IsNull(_equation);
            Assert.AreEqual(0.1F, value);
            //Assert.AreEqual("0.1", _equation.InitializedEquation);
            //Assert.AreEqual(0, _equation.Variables.Count);
            //Assert.IsInstanceOfType(_equation, typeof(ConstantEquation));
        }
        [TestMethod]
        public void CreateInstanceTest7()
        {
            _equation = EquationFactory.CreateInstance(SimpleEquation, out _);
            Assert.IsInstanceOfType(_equation, typeof(SimpleEquation));
            Assert.AreEqual("Variable_1+Variable2", _equation.InitializedEquation);
            Assert.AreEqual(2, _equation.Variables.Count);
            Assert.AreEqual("Variable_1", _equation.Variables[0]);
            Assert.AreEqual("Variable2", _equation.Variables[1]);
        }
        [TestMethod]
        public void CreateInstanceTest8()
        {
            _equation = EquationFactory.CreateInstance(SimpleEquation1, out _);
            Assert.IsInstanceOfType(_equation, typeof(SimpleEquation));
            Assert.AreEqual("Variable_1+1", _equation.InitializedEquation);
            Assert.AreEqual(1, _equation.Variables.Count);
            Assert.AreEqual("Variable_1", _equation.Variables[0]);
        }

        [TestMethod]
        public void CreateInstanceTest9()
        {
            _equation = EquationFactory.CreateInstance(ComplexEquation, out _);
            Assert.IsInstanceOfType(_equation, typeof(ComplexEquation));
            Assert.AreEqual("0.3+Name+Dt0*(Variable_1-Variable2)-Time1+Set2", _equation.InitializedEquation);
            Assert.AreEqual(3, _equation.Variables.Count);
            Assert.AreEqual("Name", _equation.Variables[0]);
            Assert.AreEqual("Variable_1", _equation.Variables[1]);
            Assert.AreEqual("Variable2", _equation.Variables[2]);
        }

        [TestMethod]
        public void CreateInstanceTest10()
        {
            _equation = EquationFactory.CreateInstance(ComplexEquation1, out _);
            Assert.IsInstanceOfType(_equation, typeof(ComplexEquation));
            Assert.AreEqual("Variable_1/(Variable2*Variable3)", _equation.InitializedEquation);
            Assert.AreEqual(3, _equation.Variables.Count);
            Assert.AreEqual("Variable_1", _equation.Variables[0]);
            Assert.AreEqual("Variable2", _equation.Variables[1]);
            Assert.AreEqual("Variable3", _equation.Variables[2]);
        }

        [TestMethod]
        public void GetVariablesTest()
        {
            _equation = EquationFactory.CreateInstance(ComplexEquation, out _);

            Assert.AreEqual(3, _equation.Variables.Count);
            Assert.AreEqual("Name", _equation.Variables[0]);
            Assert.AreEqual("Variable_1", _equation.Variables[1]);
            Assert.AreEqual("Variable2", _equation.Variables[2]);
        }



        [TestMethod()]
        public void GetParametersTest5()
        {
            const string function = @"SMTH1((Junior_doctor&apos;s__base_salary*Annual__Pay_Change),.5)";
            _equation = EquationFactory.CreateInstance(function, out _);
            Assert.AreEqual(2, _equation.Variables.Count);
            Assert.AreEqual("Junior_doctors_base_salary", _equation.Variables[0]);
            Assert.AreEqual("Annual_pay_change", _equation.Variables[1]);
        }
    }
}