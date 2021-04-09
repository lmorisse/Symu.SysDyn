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
    /// Implementation of https://github.com/SDXorg/test-models/tree/master/tests/comparisons
    /// </summary>

    [TestClass]
    public class ComparisonTest : FunctionClassTest
    {
        [TestInitialize]
        public async Task InitializeTest()
        {
            await Initialize("Comparison.xmile");
        }

        [TestMethod]
        public override void StateMachineTest()
        {
            base.StateMachineTest();
            Assert.AreEqual(11, Machine.Variables.Count());
        }

        /// <summary>
        /// Check the good functionnement on elements of comparison
        /// </summary>

        [TestMethod]
        public async Task OptimizeTest()
        {
            //Arrange
            Machine.Optimized = false;
            await Machine.Prepare();
            Assert.AreEqual(11, Machine.Variables.Count());
            Assert.IsNotNull(Machine.Variables);

            //Assert
            TestVariable("_Lt", 1);
            TestVariable("_Lte", 1);
            TestVariable("_Gt", 0);
            TestVariable("_Gte", 0);
            TestVariable("_Eq", 0);
            TestVariable("_Neq", 1);

            //Act
            Machine.Process();

            //Assert
            TestVariable("_Lt", 0);
            TestVariable("_Lte", 1);
            TestVariable("_Gt", 0);
            TestVariable("_Gte", 1);
            TestVariable("_Eq", 1);
            TestVariable("_Neq", 0);
           
        }
    }
}
