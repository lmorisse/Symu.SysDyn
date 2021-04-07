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
    /// <summary>
    /// Implementation of https://github.com/SDXorg/test-models/tree/master/tests/logicals
    /// </summary>
    [TestClass]
    public class LogicalsTest : FunctionClassTest
    {



        [TestInitialize]
        public async Task InitializeTest()
        {
            await Initialize("Logicals.xmile");
        }

        [TestMethod]
        public override void StateMachineTest()
        {
            base.StateMachineTest();
            Assert.AreEqual(9, Machine.Variables.Count());
        }

        /// <summary>
        /// Tests the AND, OR and NOT logicals
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task OptimizeTest()
        {
            //Arrange
            Machine.Optimized = false;
            await Machine.Prepare();
            Assert.AreEqual(9, Machine.Variables.Count());
            Assert.IsNotNull(Machine.Variables);

            //Assert
            TestVariable("_False_input", 0);
            TestVariable("_True_input", 1);
            TestVariable("_Not_output", 1);
            TestVariable("_And_output", 0);
            TestVariable("_Or_output", 1);

            //Act
            Machine.Process();

            //Assert
            TestVariable("_False_input", 0);
            TestVariable("_True_input", 1);
            TestVariable("_Not_output", 1);
            TestVariable("_And_output", 0);
            TestVariable("_Or_output", 1);
        }
    }

}
