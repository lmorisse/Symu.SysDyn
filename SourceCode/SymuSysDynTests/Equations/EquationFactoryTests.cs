#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2021 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Core.Equations;

#endregion

namespace Symu.SysDyn.Tests.Equations
{
    [TestClass]
    public class EquationFactoryTests
    {
        private const string SimpleEquation = "variable_1 + variable2";
        private const string SimpleEquation1 = "variable_1 + Abs(1)"; // with builtin function
        private const string ComplexEquation = ".3 + name + Dt *(variable_1 - variable2)-TIME+SET(3,5) {test}";

        private const string
            ComplexEquation1 = "variable_1/(variable2*variable3)"; // not a simple equation because of the brackets

        private EquationFactoryStruct _factory;

        /// <summary>
        ///     IsNull equation
        /// </summary>
        [DataRow("", 0)]
        [DataRow("2", 2)]
        [DataRow("Normal(1,0)", 1)]
        [DataRow(".01", 0.01F)]
        [DataRow(".45e6", 450000)]
        [DataRow("1/10", 0.1F)]
        [TestMethod]
        public async Task CreateInstanceTest(string function, float value)
        {
            _factory = await EquationFactory.CreateInstance(string.Empty, function);
            Assert.IsNull(_factory.Equation);
            Assert.AreEqual(value, _factory.Value);
        }

        /// <summary>
        ///     Case insensitive
        /// </summary>
        [DataRow("Abs(1)", 1)]
        [DataRow("ABS(1)", 1)]
        [DataRow("abs(1)", 1)]
        [TestMethod]
        public async Task CaseInsensitiveTest(string function, float value)
        {
            _factory = await EquationFactory.CreateInstance(string.Empty, function);
            Assert.IsNull(_factory.Equation);
            Assert.AreEqual(value, _factory.Value);
        }

        /// <summary>
        ///     Unknown function
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task CreateInstanceTest2()
        {
            await Assert.ThrowsExceptionAsync<ArgumentException>(async () =>
                await EquationFactory.CreateInstance(string.Empty, "Func()"));
        }

        [TestMethod]
        public async Task CreateInstanceTest7()
        {
            _factory = await EquationFactory.CreateInstance(string.Empty, SimpleEquation);
            Assert.AreEqual(2, _factory.Equation.Variables.Count);
            Assert.AreEqual("_Variable_1", _factory.Equation.Variables[0]);
            Assert.AreEqual("_Variable2", _factory.Equation.Variables[1]);
        }

        [TestMethod]
        public async Task CreateInstanceTest8()
        {
            _factory = await EquationFactory.CreateInstance(string.Empty, SimpleEquation1);
            Assert.AreEqual(1, _factory.Equation.Variables.Count);
            Assert.AreEqual("_Variable_1", _factory.Equation.Variables[0]);
        }

        [TestMethod]
        public async Task CreateInstanceTest9()
        {
            _factory = await EquationFactory.CreateInstance(string.Empty, ComplexEquation);
            Assert.AreEqual(5, _factory.Equation.Variables.Count);
            Assert.AreEqual("_Name", _factory.Equation.Variables[0]);
            Assert.AreEqual("_Dt", _factory.Equation.Variables[1]);
            Assert.AreEqual("_Variable_1", _factory.Equation.Variables[2]);
            Assert.AreEqual("_Variable2", _factory.Equation.Variables[3]);
            Assert.AreEqual("_Time", _factory.Equation.Variables[4]);
        }

        [TestMethod]
        public async Task CreateInstanceTest10()
        {
            _factory = await EquationFactory.CreateInstance(string.Empty, ComplexEquation1);
            Assert.AreEqual(3, _factory.Equation.Variables.Count);
            Assert.AreEqual("_Variable_1", _factory.Equation.Variables[0]);
            Assert.AreEqual("_Variable2", _factory.Equation.Variables[1]);
            Assert.AreEqual("_Variable3", _factory.Equation.Variables[2]);
        }

        [TestMethod]
        public async Task GetParametersTest5()
        {
            const string function = @"SMTH1((Junior_doctor's__base_salary*Annual__Pay_Change),.5)";
            _factory = await EquationFactory.CreateInstance(string.Empty, function);
            Assert.AreEqual(2, _factory.Equation.Variables.Count);
            Assert.AreEqual("_Junior_doctors_base_salary", _factory.Equation.Variables[0]);
            Assert.AreEqual("_Annual_pay_change", _factory.Equation.Variables[1]);
        }
    }
}