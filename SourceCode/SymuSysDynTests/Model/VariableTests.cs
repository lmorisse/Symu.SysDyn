#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Model;

#endregion

namespace SymuSysDynTests.Model
{
    [TestClass]
    public class VariableTests
    {
        private const string Equation0 = "10 {unit}\r";
        private const string Equation1 = "variable1 + variable2 {unit}\r";
        private const string Equation2 = "name + DT *(variable1 - variable2)";
        private const string Equation3 = "STEP(3, 5)";
        private const string Equation4 = "";
        private Variable _variable;

        /// <summary>
        ///     No equation
        /// </summary>
        [TestMethod]
        public void VariableTest()
        {
            _variable = new Variable("name");
            Assert.AreEqual("Name", _variable.Name);
            Assert.IsNull(_variable.Equation);
            Assert.IsNull(_variable.Children);
        }

        /// <summary>
        ///     Equation
        /// </summary>
        [TestMethod]
        public void VariableTest1()
        {
            _variable = new Variable("name", Equation1);
            Assert.AreEqual("Name", _variable.Name);
            Assert.AreEqual(0, _variable.Value);
            Assert.IsNotNull(_variable.Equation);
            Assert.AreEqual(2, _variable.Children.Count);
            Assert.IsTrue(_variable.Children.Contains("Variable1"));
            Assert.IsTrue(_variable.Children.Contains("Variable2"));
        }

        /// <summary>
        ///     Initial value
        /// </summary>
        [TestMethod]
        public void VariableTest2()
        {
            _variable = new Variable("name", "1");
            Assert.AreEqual("Name", _variable.Name);
            Assert.AreEqual(1, _variable.Value);
            Assert.AreEqual("1", _variable.Equation.InitializedEquation);
            Assert.AreEqual(0, _variable.Children.Count);
        }

        /// <summary>
        ///     With built in functions
        /// </summary>
        [TestMethod]
        public void VariableTest3()
        {
            _variable = new Variable("name", Equation2);
            Assert.AreEqual("Name", _variable.Name);
            Assert.AreEqual(0, _variable.Value);
            Assert.IsNotNull(_variable.Equation);
            Assert.AreEqual(2, _variable.Children.Count);
            Assert.IsTrue(_variable.Children.Contains("Variable1"));
            Assert.IsTrue(_variable.Children.Contains("Variable2"));
        }

        /// <summary>
        ///     With built in functions
        /// </summary>
        [TestMethod]
        public void VariableTest4()
        {
            _variable = new Variable("name", Equation3);
            Assert.AreEqual("Name", _variable.Name);
            Assert.AreEqual(0, _variable.Value);
            Assert.IsNotNull(_variable.Equation);
            Assert.AreEqual(0, _variable.Children.Count);
        }

        [TestMethod]
        public void ToStringTest()
        {
            _variable = new Variable("name");
            Assert.AreEqual("Name", _variable.ToString());
        }

        [TestMethod]
        public void CheckInitialValueTest()
        {
            Assert.AreEqual(10, Variable.CheckInitialValue(Equation0));
            Assert.AreEqual(0, Variable.CheckInitialValue(Equation1));
            Assert.AreEqual(0, Variable.CheckInitialValue(Equation4));
        }
    }
}