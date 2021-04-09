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

namespace Symu.SysDyn.Tests.NewTests
{
    /// <summary>
    /// Implementation of https://github.com/SDXorg/test-models/tree/master/tests/function_capitalization
    /// </summary>
    [TestClass]
    public class Function_capitalization : FunctionClassTest
    {
        [TestInitialize]
        public async Task InitializeTest()
        {
            await Initialize("Function_capitalization.xmile");
        }

        [TestMethod]
        public override void StateMachineTest()
        {
            base.StateMachineTest();
            Assert.AreEqual(9, Machine.Variables.Count());
        }

        /// <summary>
        /// Check if non-dynamic variable consider capitalization on the xmile file and test the abs(int) function
        /// </summary>
        [TestMethod]
        public async Task OptimizeTest()
        {
            //Arrange
            Machine.Optimized = false;
            await Machine.Prepare();
            Assert.IsNotNull(Machine.Variables);

            //Assert
            TestVariable("_Test1", 5);
            TestVariable("_Test2", 5);
            TestVariable("_Test3", 5);
        }
    }
}