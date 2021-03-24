﻿#region Licence

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
        /// <returns></returns>
        [TestMethod]
        public async Task OptimizeTest()
        {
            Machine.Optimized = false;
            await Machine.Prepare();
            Assert.AreEqual(11, Machine.Variables.Count());
            Assert.IsNotNull(Machine.Variables);
            var variable = Machine.Variables.Get("_Lt");
            Assert.IsNotNull(variable);
            Assert.AreEqual(1, variable.Value);
            variable = Machine.Variables.Get("_Lte");
            Assert.IsNotNull(variable);
            Assert.AreEqual(1, variable.Value);
            variable = Machine.Variables.Get("_Gt");
            Assert.IsNotNull(variable);
            Assert.AreEqual(0, variable.Value);
            variable = Machine.Variables.Get("_Gte");
            Assert.IsNotNull(variable);
            Assert.AreEqual(0, variable.Value);
            variable = Machine.Variables.Get("_Eq");
            Assert.IsNotNull(variable);
            Assert.AreEqual(0, variable.Value);
            variable = Machine.Variables.Get("_Neq");
            Assert.IsNotNull(variable);
            Assert.AreEqual(1, variable.Value);
            Machine.Process();
            variable = Machine.Variables.Get("_Lt");
            Assert.IsNotNull(variable);
            Assert.AreEqual(0, variable.Value);
            variable = Machine.Variables.Get("_Lte");
            Assert.IsNotNull(variable);
            Assert.AreEqual(1, variable.Value);
            variable = Machine.Variables.Get("_Gt");
            Assert.IsNotNull(variable);
            Assert.AreEqual(0, variable.Value);
            variable = Machine.Variables.Get("_Gte");
            Assert.IsNotNull(variable);
            Assert.AreEqual(1, variable.Value);
            variable = Machine.Variables.Get("_Eq");
            Assert.IsNotNull(variable);
            Assert.AreEqual(1, variable.Value);
            variable = Machine.Variables.Get("_Neq");
            Assert.IsNotNull(variable);
            Assert.AreEqual(0, variable.Value);
        }
    }
}
