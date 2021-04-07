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
    /// Implementation of https://github.com/SDXorg/test-models/tree/master/tests/initial_function
    /// </summary>
    [TestClass]
    public class InitialFunctionTest : FunctionClassTest
    {
        [TestInitialize]
        public async Task InitializeTest()
        {
            await Initialize("Initial_function.xmile");
        }

        [TestMethod]
        public override void StateMachineTest()
        {
            base.StateMachineTest();
            Assert.AreEqual(7, Machine.Variables.Count());
        }

        /// <summary>
        /// Check if dynamic variables are in initial position
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task OptimizeTest()
        {
            //Arrange
            Machine.Optimized = false;
            await Machine.Prepare();
            Assert.AreEqual(7, Machine.Variables.Count());
            Assert.IsNotNull(Machine.Variables);

            //Assert
            TestVariable("_Stocka", 10);
            TestVariable("_Stocka_initial_value", 10);
            TestTwoVariables("_Stocka", "_Inflowa");
            TestTwoVariables("_Inflowa", "_Stocka_initial_value");

            //Act
            Machine.Process();

            //Assert
            TestVariable("_Stocka", 10240);
            TestVariable("_Inflowa", 10240);
            TestTwoVariables("_Stocka", "_Inflowa");
            TestVariable("_Stocka_initial_value", 10);
        }
    }
}