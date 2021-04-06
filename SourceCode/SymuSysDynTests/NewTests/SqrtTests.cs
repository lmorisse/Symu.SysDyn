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
    public class SqrtTests : FunctionClassTest
    {
        [TestInitialize]
        public async Task InitializeTest()
        {
            await Initialize("SqrtTest.xmile");
        }

        [TestMethod]
        public override void StateMachineTest()
        {
            base.StateMachineTest();
            Assert.AreEqual(7, Machine.Variables.Count());
        }

        /// <summary>
        /// Check the natural sqrt function
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task OptimizeTest()
        {
            Machine.Optimized = false;
            await Machine.Prepare();
            Assert.AreEqual(7, Machine.Variables.Count());
            Assert.IsNotNull(Machine.Variables);
            var variable = Machine.Variables.Get("_Test_sqrt");
            Assert.IsNotNull(variable);
            Assert.AreEqual(0.03F, (float)Math.Round(variable.Value, 2));
            Machine.Process();
            variable = Machine.Variables.Get("_Test_sqrt");
            Assert.IsNotNull(variable);
            Assert.AreEqual(32.38F, (float)Math.Round(variable.Value, 2));
        }
    }
}
