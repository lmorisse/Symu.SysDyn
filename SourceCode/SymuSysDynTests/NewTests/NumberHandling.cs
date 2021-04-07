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
    /// Implementation of https://github.com/SDXorg/test-models/tree/master/tests/number_handling
    /// </summary>

    [TestClass]
    public class NumberHandling : FunctionClassTest
    {
        [TestInitialize]
        public async Task InitializeTest()
        {
            await Initialize("NumberHandling.xmile");
        }

        [TestMethod]
        public override void StateMachineTest()
        {
            base.StateMachineTest();
            Assert.AreEqual(9, Machine.Variables.Count());
        }

        /// <summary>
        /// Test the integer division
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
            TestVariableFloat("_Quotient_target", 0.75F, 2);
            TestVariable("_Numerator", 3);
            TestVariable("_Denominator", 4);
            TestVariableFloat("_Quotient", 0.75F, 2);
            TestVariable("_Equality", 1);

            //Act
            Machine.Process();

            //Assert
            TestVariableFloat("_Quotient_target", 0.75F, 2);
            TestVariable("_Numerator", 3);
            TestVariable("_Denominator", 4);
            TestVariableFloat("_Quotient", 0.75F, 2);
            TestVariable("_Equality", 1);



        }
    }
}
