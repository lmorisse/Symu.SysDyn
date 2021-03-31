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
    public class LineContinuation : FunctionClassTest
    {
        [TestInitialize]
        public async Task InitializeTest()
        {
            await Initialize("Line_Continuation.xmile");
        }

        [TestMethod]
        public override void StateMachineTest()
        {
            base.StateMachineTest();
            Assert.AreEqual(8, Machine.Variables.Count());
        }

        /// <summary>
        /// tests the ability of an analyzer to handle long variable names and long equations
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task OptimizeTest()
        {
            Machine.Optimized = false;
            await Machine.Prepare();
            Assert.AreEqual(8, Machine.Variables.Count());
            Assert.IsNotNull(Machine.Variables);
            var longue_Variable = Machine.Variables.Get("_Target_for_goal_seeking_loop_and_convenient_object_for_testing_user_broken_lines");
            Assert.AreEqual(100, longue_Variable.Value);
            var variable = Machine.Variables.Get("_Flow_descriptor_that_is_somewhat_awkward_in_implementation_and_definitely_unhelpful_in_description");
            Assert.IsNotNull(variable);
            Assert.AreEqual(50, variable.Value);
            Machine.Process();
            Assert.AreEqual(50, variable.Value);

        }
    }
}