#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Equations;

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
        private const string SimpleEquation = "variable1 + variable2";
        private const string SimpleEquation1 = "variable1 + Abs(1)"; // with builtin function
        private const string ComplexEquation = "3 + name + Dt *(variable1 - variable2)-Time+SET(3,5) {test}";
        private IEquation _equation;

        [TestMethod]
        public void CreateInstanceTest()
        {
            _equation = EquationFactory.CreateInstance(EmptyEquation);
            Assert.IsNull(_equation);
        }
        [TestMethod]
        public void CreateInstanceTest1()
        {
            _equation = EquationFactory.CreateInstance(ConstantEquation);
            Assert.AreEqual("2", _equation.InitializedEquation);
            Assert.AreEqual(0, _equation.Variables.Count);
            Assert.IsInstanceOfType(_equation, typeof(ConstantEquation));
        }
        [TestMethod]
        public void CreateInstanceTest2()
        {
            _equation = EquationFactory.CreateInstance(ConstantEquation1);
            Assert.AreEqual("1", _equation.InitializedEquation);
            Assert.AreEqual(0, _equation.Variables.Count);
            Assert.IsInstanceOfType(_equation, typeof(ConstantEquation));
        }
        [TestMethod]
        public void CreateInstanceTest3()
        {
            _equation = EquationFactory.CreateInstance(ConstantEquation2);
            Assert.AreEqual("1", _equation.InitializedEquation);
            Assert.AreEqual(0, _equation.Variables.Count);
            Assert.IsInstanceOfType(_equation, typeof(ConstantEquation));
        }
        [TestMethod]
        public void CreateInstanceTest4()
        {
            _equation = EquationFactory.CreateInstance(SimpleEquation);
            Assert.IsInstanceOfType(_equation, typeof(SimpleEquation));
            Assert.AreEqual("Variable1+Variable2", _equation.InitializedEquation);
            Assert.AreEqual(2, _equation.Variables.Count);
            Assert.AreEqual("Variable1", _equation.Variables[0]);
            Assert.AreEqual("Variable2", _equation.Variables[1]);
        }
        [TestMethod]
        public void CreateInstanceTest5()
        {
            _equation = EquationFactory.CreateInstance(SimpleEquation1);
            Assert.IsInstanceOfType(_equation, typeof(SimpleEquation));
            Assert.AreEqual("Variable1+1", _equation.InitializedEquation);
            Assert.AreEqual(1, _equation.Variables.Count);
            Assert.AreEqual("Variable1", _equation.Variables[0]);
        }

        [TestMethod]
        public void CreateInstanceTest6()
        {
            _equation = EquationFactory.CreateInstance(ComplexEquation);
            Assert.IsInstanceOfType(_equation, typeof(ComplexEquation));
            Assert.AreEqual("3+Name+Dt0*(Variable1-Variable2)-Time1+Set2", _equation.InitializedEquation);
            Assert.AreEqual(3, _equation.Variables.Count);
            Assert.AreEqual("Name", _equation.Variables[0]);
            Assert.AreEqual("Variable1", _equation.Variables[1]);
            Assert.AreEqual("Variable2", _equation.Variables[2]);
        }

        [TestMethod]
        public void GetVariablesTest()
        {
            _equation = EquationFactory.CreateInstance(ComplexEquation);

            Assert.AreEqual(3, _equation.Variables.Count);
            Assert.AreEqual("Name", _equation.Variables[0]);
            Assert.AreEqual("Variable1", _equation.Variables[1]);
            Assert.AreEqual("Variable2", _equation.Variables[2]);
        }
    }
}