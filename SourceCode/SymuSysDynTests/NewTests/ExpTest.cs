#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2021 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Tests.Classes;
#endregion

namespace Symu.SysDyn.Tests.NewTests
{
    [TestClass]
    public class ExpTest : FunctionClassTest
    {
        [TestInitialize]
        public async Task InitializeTest()
        {
            await Initialize("Exp.xmile");
        }

        [TestMethod]
        public override void StateMachineTest()
        {
            base.StateMachineTest();
            Assert.AreEqual(7, Machine.Variables.Count());
        }

        /// <summary>
        /// Check the exponential function
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task OptimizeTest()
        {
            Machine.Optimized = false;
            await Machine.Prepare();
            Assert.AreEqual(7, Machine.Variables.Count());
            Assert.IsNotNull(Machine.Variables);
            var variable = Machine.Variables.Get("_Test_exp");
            Assert.IsNotNull(variable);
            Assert.AreEqual(0.0067F, (float)Math.Round(variable.Value, 4));
            Machine.Process();
            variable = Machine.Variables.Get("_Test_exp");
            Assert.IsNotNull(variable);
            Assert.AreEqual(148.41F, (float)Math.Round(variable.Value, 2));
        }
    }

}
