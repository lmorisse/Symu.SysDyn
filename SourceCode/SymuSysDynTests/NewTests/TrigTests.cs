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
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Core.Engine;
using Symu.SysDyn.Core.Models.XMile;
using Symu.SysDyn.Tests.Classes;
#endregion

namespace Symu.SysDyn.Tests.NewTests
{
    [TestClass]
    public class TrigTests : FunctionClassTest
    {

        [TestInitialize]
        public async Task InitializeTest()
        {
            await Initialize("Trig.xmile");
        }

        [TestMethod]
        public override void StateMachineTest()
        {
            base.StateMachineTest();
            Assert.AreEqual(12, Machine.Variables.Count());
        }

        /// <summary>
        /// Tests the AND, OR and NOT logicals
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task OptimizeTest()
        {
            Machine.Optimized = false;
            await Machine.Prepare();

            Assert.AreEqual(12, Machine.Variables.Count());
            Assert.IsNotNull(Machine.Variables);
            TestVariable("_Stocka", -10);
            TestVariableFloat("_Test_arccos", 2.57F, 2);
            TestVariableFloat("_Test_arcsin", 0.58F, 2);
            TestVariableFloat("_Test_arctan", -0.58F, 2);
            TestVariableFloat("_Test_cos", -0.84F, 2);
            TestVariableFloat("_Test_sin", 0.54F, 2);
            TestVariableFloat("_Test_tan", -0.65F, 2);

            Machine.Process();

            TestVariable("_Stocka", -5);
            TestVariableFloat("_Test_arccos", 1.28F, 2);
            TestVariableFloat("_Test_arcsin", 1.28F, 2);
            TestVariableFloat("_Test_arctan", 1.28F, 2);
            TestVariableFloat("_Test_cos", 0.28F, 2);
            TestVariableFloat("_Test_sin", 0.96F, 2);
            TestVariableFloat("_Test_tan", 3.38F, 2);
        }
    }

}
