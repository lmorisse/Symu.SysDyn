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
    /// Implementation of https://github.com/SDXorg/test-models/tree/master/tests/builtin_max
    /// </summary>

    [TestClass]
    public class XidzZidzTests : FunctionClassTest
    {



        [TestInitialize]
        public async Task InitializeTest()
        {
            await Initialize("Xidz_Zidz.xmile");
        }

        [TestMethod]
        public override void StateMachineTest()
        {
            base.StateMachineTest();
            Assert.AreEqual(9, Machine.Variables.Count());
        }

        /// <summary>
        /// Check if the variable is the max
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
            TestVariable("_Test_zidz", 0);
            TestVariableFloat("_Test_xidz", 2.75F, 2);

            //Act
            Machine.Process();

            //Assert
            TestVariable("_Test_zidz", 0);
            TestVariableFloat("_Test_xidz", 2.75F, 2);

        }
    }

}
