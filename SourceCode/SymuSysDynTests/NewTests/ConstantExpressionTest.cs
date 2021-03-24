using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Core.Engine;
using Symu.SysDyn.Core.Models.XMile;
using Symu.SysDyn.Tests.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Symu.SysDyn.Tests.NewTests
{
    [TestClass]
    public class ConstantExpressionTest :FunctionClassTest
    {
        [TestInitialize]
        public async Task InitializeTest()
        {
            await Initialize("Constant_expression.xmile");
        }

        [TestMethod]
        public override void StateMachineTest()
        {
            base.StateMachineTest();
            Assert.AreEqual(5, Machine.Variables.Count());
        }

        /// <summary>
        /// Check that a model elements parses properly an expression made only of numbers
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task OptimizeTest()
        {
            Machine.Optimized = false;
            await Machine.Prepare();
            Assert.AreEqual(5, Machine.Variables.Count());
            Assert.IsNotNull(Machine.Variables);
            var variable = Machine.Variables.Get("_My_variable");
            Assert.IsNotNull(variable);
            Assert.AreEqual(3.33F, (float)Math.Round(variable.Value,2));
        }
    }
}
