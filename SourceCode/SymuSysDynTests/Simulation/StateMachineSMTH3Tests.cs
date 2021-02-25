#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2021 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Tests.Classes;

#endregion

namespace Symu.SysDyn.Tests.Simulation
{
    [TestClass]
    public class StateMachineSmth3Tests : Smth3ClassTest
    {
        [TestInitialize]
        public async Task InitializeTest()
        {
            await Initialize();
        }

        [TestMethod]
        public void StateMachineTest()
        {
            Assert.IsNotNull(Machine.Simulation);
            Assert.IsNotNull(Machine.Results);
            Assert.IsNotNull(Machine.ReferenceVariables);
            Assert.IsNotNull(Machine.Variables);
            Assert.AreEqual(0, Machine.Results.Count);
            Assert.AreEqual(14, Machine.Variables.Count());
        }

        [TestMethod]
        public async Task OptimizeTest()
        {
            Machine.Optimized = true;
            await Machine.Prepare();
            Assert.AreEqual(12, Machine.Variables.Count());
            var variable = Machine.Variables.Get("_Input");
            Assert.IsNotNull(variable);
            Assert.AreEqual(5, variable.Value);
            Assert.AreEqual("5+Step(10,3)", variable.Equation.OriginalEquation);
            variable = Machine.Variables.Get("_Comp1");
            Assert.IsNotNull(variable);
            Assert.AreEqual(5, variable.Value);
            Assert.AreEqual("Comp1+0.5*(Flow1)", variable.Equation.OriginalEquation);
            variable = Machine.Variables.Get("_Comp2");
            Assert.IsNotNull(variable);
            Assert.AreEqual(5, variable.Value);
            Assert.AreEqual("Comp2+0.5*(Flow2)", variable.Equation.OriginalEquation);
            variable = Machine.Variables.Get("_Comp3");
            Assert.IsNotNull(variable);
            Assert.AreEqual(5, variable.Value);
            Assert.AreEqual("Comp3+0.5*(Flow3)", variable.Equation.OriginalEquation);
            variable = Machine.Variables.Get("_Flow1");
            Assert.IsNotNull(variable);
            Assert.AreEqual(0, variable.Value);
            Assert.AreEqual("(((_Input-_Comp1)*3)/5)", variable.Equation.OriginalEquation);
            variable = Machine.Variables.Get("_Flow2");
            Assert.IsNotNull(variable);
            Assert.AreEqual(0, variable.Value);
            Assert.AreEqual("(((_Comp1-_Comp2)*3)/5)", variable.Equation.OriginalEquation);
            variable = Machine.Variables.Get("_Flow3");
            Assert.IsNotNull(variable);
            Assert.AreEqual(0, variable.Value);
            Assert.AreEqual("(((_Comp2-_Comp3)*3)/5)", variable.Equation.OriginalEquation);
        }
    }
}