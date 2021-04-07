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
    [TestClass]
    public class LineBreaks : FunctionClassTest
    {
        /// <summary>
        /// Implementation of https://github.com/SDXorg/test-models/tree/master/tests/line_breaks
        /// </summary>
        [TestInitialize]
        public async Task InitializeTest()
        {
            await Initialize("Line_Breaks.xmile");
        }

        [TestMethod]
        public override void StateMachineTest()
        {
            base.StateMachineTest();
            Assert.AreEqual(7, Machine.Variables.Count());
        }

        /// <summary>
        /// tests the ability of an analyzer to manage a line break in the description of a variable
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
            TestVariable("_Downstream", 7);

            //Act
            Machine.Process();

            //Assert
            TestVariable("_Downstream", 7);
        }
    }
}