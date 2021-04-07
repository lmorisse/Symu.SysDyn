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
    /// <summary>
    /// Implementation of https://github.com/SDXorg/test-models/blob/master/tests/parentheses
    /// </summary>
    [TestClass]
    public class ParentheseTest : FunctionClassTest
    {
        [TestInitialize]
        public async Task InitializeTest()
        {
            await Initialize("ParentheseTest.xmile");
        }

        [TestMethod]
        public override void StateMachineTest()
        {
            base.StateMachineTest();
            Assert.AreEqual(8, Machine.Variables.Count());
        }

        /// <summary>
        /// Check variables with equations containing parentheses
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task OptimizeTest()
        {
            //Arrange
            Machine.Optimized = false;
            await Machine.Prepare();
            Assert.AreEqual(8, Machine.Variables.Count());
            Assert.IsNotNull(Machine.Variables);

            //Assert
            TestVariable("_Equation", 144);

            //Act
            Machine.Process();

            //Assert
            TestVariable("_Equation", 144);
        }
    }
}
