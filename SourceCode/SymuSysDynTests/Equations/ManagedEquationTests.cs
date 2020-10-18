#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Equations;

#endregion

namespace SymuSysDynTests.Equations
{
    [TestClass]
    public class ManagedEquationTests
    {
        private const string Equation1 = "variable1 + variable2";
        private const string Equation2 = "3 + name + Dt *(variable1 - variable2)-Time+SET(3,5) {test}";
        private ManagedEquation _equation;

        [TestMethod]
        public void InitializeTest()
        {
            _equation = new ManagedEquation(Equation1);
            Assert.AreEqual("Variable1 + Variable2", _equation.ToString());
            Assert.AreEqual(3, _equation.Words.Count);
            Assert.AreEqual("Variable1", _equation.Words[0]);
            Assert.AreEqual("+", _equation.Words[1]);
            Assert.AreEqual("Variable2", _equation.Words[2]);
            var variables = _equation.GetVariables().ToList();
            Assert.AreEqual(2, variables.Count);
            Assert.AreEqual("Variable1", variables[0]);
            Assert.AreEqual("Variable2", variables[1]);
        }

        [TestMethod]
        public void InitializeTest2()
        {
            _equation = new ManagedEquation(Equation2);
            Assert.AreEqual("3 + Name + Dt1 * ( Variable1 - Variable2 ) - Time2 + Set0", _equation.ToString());
            Assert.AreEqual(15, _equation.Words.Count);
            var variables = _equation.GetVariables().ToList();
            Assert.AreEqual(3, variables.Count);
            Assert.AreEqual("Name", variables[0]);
            Assert.AreEqual("Variable1", variables[1]);
            Assert.AreEqual("Variable2", variables[2]);
        }

        [TestMethod]
        public void GetVariablesTest()
        {
            _equation = new ManagedEquation(Equation2);

            var variables = _equation.GetVariables().ToList();
            Assert.AreEqual(3, variables.Count);
            Assert.AreEqual("Name", variables[0]);
            Assert.AreEqual("Variable1", variables[1]);
            Assert.AreEqual("Variable2", variables[2]);
        }
    }
}