#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace SymuSysDynTests.Simulation
{
    [TestClass]
    public class StateMachineSMTH3Tests : SMTH3ClassTest
    {
        [TestMethod]
        public void StateMachineTest()
        {
            Assert.IsNotNull(Machine.Simulation);
            Assert.IsNotNull(Machine.Results);
            Assert.IsNotNull(Machine.ReferenceVariables);
            Assert.AreEqual(0, Machine.Results.Count);
            //Initialize
            Assert.AreEqual(5, Machine.ReferenceVariables["Input"].Value);
            Assert.AreEqual(5, Machine.ReferenceVariables["Initial"].Value);
            Assert.AreEqual(5, Machine.ReferenceVariables["Averaging"].Value);
            Assert.AreEqual(5, Machine.ReferenceVariables["Comp1"].Value);
            Assert.AreEqual(5, Machine.ReferenceVariables["Comp2"].Value);
            Assert.AreEqual(5, Machine.ReferenceVariables["Comp3"].Value);
            Assert.AreEqual(0, Machine.ReferenceVariables["Flow1"].Value);
            Assert.AreEqual(0, Machine.ReferenceVariables["Flow2"].Value);
            Assert.AreEqual(0, Machine.ReferenceVariables["Flow3"].Value);
        }

        [TestMethod()]
        public void OptimizeTest()
        {
            Machine.Optimize();
            Assert.AreEqual(7, Machine.Variables.Count());
            var variable = Machine.Variables.Get("Input");
            Assert.IsNotNull(variable);
            Assert.AreEqual(5, variable.Value);
            Assert.AreEqual("5+Step0", variable.Equation.InitializedEquation);
            variable = Machine.Variables.Get("Comp1");
            Assert.IsNotNull(variable);
            Assert.AreEqual(5, variable.Value);
            Assert.AreEqual("Comp1+Dt0*(Flow1)", variable.Equation.InitializedEquation);
            variable = Machine.Variables.Get("Comp2");
            Assert.IsNotNull(variable);
            Assert.AreEqual(5, variable.Value);
            Assert.AreEqual("Comp2+Dt0*(Flow2)", variable.Equation.InitializedEquation);
            variable = Machine.Variables.Get("Comp3");
            Assert.IsNotNull(variable);
            Assert.AreEqual(5, variable.Value);
            Assert.AreEqual("Comp3+Dt0*(Flow3)", variable.Equation.InitializedEquation);
            variable = Machine.Variables.Get("Flow1");
            Assert.IsNotNull(variable);
            Assert.AreEqual(0, variable.Value);
            Assert.AreEqual("(Input-Comp1)*3/5", variable.Equation.InitializedEquation);
            variable = Machine.Variables.Get("Flow2");
            Assert.IsNotNull(variable);
            Assert.AreEqual(0, variable.Value);
            Assert.AreEqual("(Comp1-Comp2)*3/5", variable.Equation.InitializedEquation);
            variable = Machine.Variables.Get("Flow3");
            Assert.IsNotNull(variable);
            Assert.AreEqual(0, variable.Value);
            Assert.AreEqual("(Comp2-Comp3)*3/5", variable.Equation.InitializedEquation);
        }

    }
}