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
    public class Builtin_max : FunctionClassTest
    {

 

        [TestInitialize]
        public async Task InitializeTest()
        {
            await Initialize("Builtin_max.xmile");
        }

        [TestMethod]
        public override void StateMachineTest()
        {
            base.StateMachineTest();
            Assert.AreEqual(5, Machine.Variables.Count());
        }

        /// <summary>
        /// Check if the variable is the max
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task OptimizeTest()
        {
            Machine.Optimized = false;
            await Machine.Prepare();
            Assert.AreEqual(5, Machine.Variables.Count());
            Assert.IsNotNull(Machine.Variables);
            var maxValue = Machine.Variables.Get("_Output");
            Assert.IsNotNull(maxValue);
            Assert.AreEqual(5, maxValue.Value);
            Machine.Process();
            var variable = Machine.Variables.Get("_Output");
            Assert.IsNotNull(variable);
            Assert.AreNotEqual(maxValue, variable.Value);
           
        }
    }
    
}
