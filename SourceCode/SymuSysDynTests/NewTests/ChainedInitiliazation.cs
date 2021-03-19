#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2021 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Core.Engine;
using Symu.SysDyn.Core.Models.XMile;
using Symu.SysDyn.Tests.Classes;

#endregion

namespace Symu.SysDyn.Tests.NewTests
{
    [TestClass]
    public class ChainedInitialization : FunctionClassTest
    {
        [TestInitialize]
        public async Task InitializeTest()
        {
            await Initialize("Chained_Initialization.xmile");
        }

        [TestMethod]
        public void StateMachineTest()
        {
            base.StateMachineTest();
            Assert.AreEqual(12, Machine.Variables.Count());
        }

        /// <summary>
        /// Check if stock B need stock C and D, if C and D are initialized after B, its supported by application
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task OptimizeTest()
        {
            Machine.Optimized = false;
            await Machine.Prepare();
            var variable = Machine.Variables.Get("_Stock_b");
            Assert.IsNotNull(variable);
            Assert.AreEqual(15, variable.Value);
        }
    }
}
