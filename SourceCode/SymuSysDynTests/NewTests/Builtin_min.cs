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
    public class Builtin_min : FunctionClassTest
    {
    
        [TestInitialize]
        public async Task InitializeTest()
        {
            await Initialize("Builtin_min.xmile");
        }

        [TestMethod]
        public override void StateMachineTest()
        {
            base.StateMachineTest();
            Assert.AreEqual(5, Machine.Variables.Count());
        }

        /// <summary>
        /// Check if the variable is the min
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task OptimizeTest()
        {
            Machine.Optimized = false;
            await Machine.Prepare();
            Assert.AreEqual(5, Machine.Variables.Count());
            Assert.IsNotNull(Machine.Variables);
            var minValue = Machine.Variables.Get("_Output");
            Assert.IsNotNull(minValue);
            Assert.AreEqual(0, minValue.Value);
            Machine.Process();
            var variable = Machine.Variables.Get("_Output");
            Assert.IsNotNull(variable);
            Assert.AreNotEqual(minValue, variable.Value);

        }
    }

}
