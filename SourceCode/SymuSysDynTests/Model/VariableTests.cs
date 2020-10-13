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
        private const string Equation1 = "variable1 + variable2";
        private Variable _variable;


        [TestMethod]
        public void VariableTest()
        {
            _variable = new Variable("name");
            Assert.AreEqual("name", _variable.Name);
        }

        [TestMethod]
        public void VariableTest1()
        {
            _variable = new Variable("name", Equation1);
            Assert.AreEqual("name", _variable.Name);
            Assert.AreEqual(Equation1, _variable.Equation);
            Assert.AreEqual(2, _variable.Children.Count);
        }

        [TestMethod]
        public void ToStringTest()
        {
            _variable = new Variable("name");
            Assert.AreEqual("name", _variable.ToString());
        }

        [TestMethod]
        public void FindChildrenTest()
        {
            _variable = new Variable("name", Equation1);
            Assert.AreEqual(2, _variable.Children.Count);
            Assert.IsTrue(_variable.Children.Contains("variable1"));
            Assert.IsTrue(_variable.Children.Contains("variable2"));
        }

        [TestMethod]
        public void CheckInitialValueTest()
        {
            _variable = new Variable("name", "1");
            Assert.AreEqual(1, _variable.Value);
        }

        [TestMethod]
        public void CheckInitialValueTest1()
        {
            _variable = new Variable("name", Equation1);
            Assert.AreEqual(0, _variable.Value);
        }
    }
}