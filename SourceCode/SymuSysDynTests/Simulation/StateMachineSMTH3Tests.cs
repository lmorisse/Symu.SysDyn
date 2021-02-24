#region Licence

// Description: SymuBiz - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SymuSysDynTests.Classes;

#endregion

namespace SymuSysDynTests.Simulation
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