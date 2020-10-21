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
using Symu.SysDyn.Simulation;

#endregion

namespace SymuSysDynTests.Equations
{
    [TestClass]
    public class EquationTests
    {
        private const string Equation0 = "";
        private const string Equation1 = "variable1 + variable2";
        private const string Equation2 = "3 + name + Dt *(variable1 - variable2)-Time+SET(3,5) {test}";
        private Equation _equation;

        [TestMethod]
        public void InitializeTest0()
        {
            var machine = new StateMachine();
            _equation = new Equation(Equation0);
            Assert.IsNull(_equation.InitializedEquation);
            Assert.AreEqual(0, _equation.Variables.Count); 
            Assert.AreEqual(0, _equation.Evaluate(machine.Variables, machine.Simulation));
        }
        [TestMethod]
        public void InitializeTest()
        {
            _equation = new Equation(Equation1);
            Assert.AreEqual("Variable1+Variable2", _equation.InitializedEquation);
            Assert.AreEqual(2, _equation.Variables.Count);
            Assert.AreEqual("Variable1", _equation.Variables[0]);
            Assert.AreEqual("Variable2", _equation.Variables[1]);
        }

        [TestMethod]
        public void InitializeTest2()
        {
            _equation = new Equation(Equation2);
            Assert.AreEqual("3+Name+Dt1*(Variable1-Variable2)-Time2+Set0", _equation.InitializedEquation);
            Assert.AreEqual(3, _equation.Variables.Count);
            Assert.AreEqual("Name", _equation.Variables[0]);
            Assert.AreEqual("Variable1", _equation.Variables[1]);
            Assert.AreEqual("Variable2", _equation.Variables[2]);
        }

        [TestMethod]
        public void GetVariablesTest()
        {
            _equation = new Equation(Equation2);

            Assert.AreEqual(3, _equation.Variables.Count);
            Assert.AreEqual("Name", _equation.Variables[0]);
            Assert.AreEqual("Variable1", _equation.Variables[1]);
            Assert.AreEqual("Variable2", _equation.Variables[2]);
        }
    }
}