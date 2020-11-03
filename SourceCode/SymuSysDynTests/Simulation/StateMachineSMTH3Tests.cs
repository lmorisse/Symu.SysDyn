#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace SymuSysDynTests.Simulation
{
    [TestClass]
    public class StateMachineSmth3Tests : Smth3ClassTest
    {
        [TestMethod]
        public void StateMachineTest()
        {
            Assert.IsNotNull(Machine.Simulation);
            Assert.IsNotNull(Machine.Results);
            Assert.IsNotNull(Machine.ReferenceVariables);
            Assert.AreEqual(0, Machine.Results.Count);
            //Initialize
            Assert.AreEqual(5, Machine.ReferenceVariables["_Input"].Value);
            Assert.AreEqual(5, Machine.ReferenceVariables["_Initial"].Value);
            Assert.AreEqual(5, Machine.ReferenceVariables["_Averaging"].Value);
            Assert.AreEqual(5, Machine.ReferenceVariables["_Comp1"].Value);
            Assert.AreEqual(5, Machine.ReferenceVariables["_Comp2"].Value);
            Assert.AreEqual(5, Machine.ReferenceVariables["_Comp3"].Value);
            Assert.AreEqual(0, Machine.ReferenceVariables["_Flow1"].Value);
            Assert.AreEqual(0, Machine.ReferenceVariables["_Flow2"].Value);
            Assert.AreEqual(0, Machine.ReferenceVariables["_Flow3"].Value);
        }

        [TestMethod]
        public void OptimizeTest()
        {
            Machine.Optimized = true;
            Assert.AreEqual(9, Machine.Variables.Count());
            Machine.OptimizeVariables();
            Assert.AreEqual(7, Machine.Variables.Count());
            var variable = Machine.Variables.Get("_Input");
            Assert.IsNotNull(variable);
            Assert.AreEqual(5, variable.Value);
            Assert.AreEqual("5+Step0", variable.Equation.InitializedEquation);
            variable = Machine.Variables.Get("_Comp1");
            Assert.IsNotNull(variable);
            Assert.AreEqual(5, variable.Value);
            Assert.AreEqual("_Comp1+0.5*(_Flow1)", variable.Equation.InitializedEquation);
            variable = Machine.Variables.Get("_Comp2");
            Assert.IsNotNull(variable);
            Assert.AreEqual(5, variable.Value);
            Assert.AreEqual("_Comp2+0.5*(_Flow2)", variable.Equation.InitializedEquation);
            variable = Machine.Variables.Get("_Comp3");
            Assert.IsNotNull(variable);
            Assert.AreEqual(5, variable.Value);
            Assert.AreEqual("_Comp3+0.5*(_Flow3)", variable.Equation.InitializedEquation);
            variable = Machine.Variables.Get("_Flow1");
            Assert.IsNotNull(variable);
            Assert.AreEqual(0, variable.Value);
            Assert.AreEqual("(_Input-_Comp1)*3/5", variable.Equation.InitializedEquation);
            variable = Machine.Variables.Get("_Flow2");
            Assert.IsNotNull(variable);
            Assert.AreEqual(0, variable.Value);
            Assert.AreEqual("(_Comp1-_Comp2)*3/5", variable.Equation.InitializedEquation);
            variable = Machine.Variables.Get("_Flow3");
            Assert.IsNotNull(variable);
            Assert.AreEqual(0, variable.Value);
            Assert.AreEqual("(_Comp2-_Comp3)*3/5", variable.Equation.InitializedEquation);
        }
    }
}