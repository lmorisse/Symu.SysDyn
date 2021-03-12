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

namespace Symu.SysDyn.Tests.Simulation
{
    [TestClass]
    public class ActiveInitialTest
    {
        protected const string TestFile = ClassPath.classpath + "Active_initial.xmile";

        /// <summary>
        ///     It is not the Machine.Variables
        /// </summary>
        protected XMileModel Variables { get; } = new XMileModel("1");

        protected XDocument XDoc { get; private set; }
        protected XNamespace Ns { get; private set; }
        protected XElement XElement { get; set; }
        protected StateMachine Machine { get; private set; }

        [TestInitialize]
        public async Task InitializeTest()
        {
            Machine = await StateMachine.CreateStateMachine(TestFile);
            XDoc = XDocument.Load(TestFile);
            Ns = XDoc.Root?.Attributes("xmlns").First().Value;
            XElement = XDoc.Root?.Descendants(Ns + "variables").First();
        }

        [TestMethod]
        public void StateMachineTest()
        {
            Assert.IsNotNull(Machine.Simulation);
            Assert.IsNotNull(Machine.Results);
            Assert.IsNotNull(Machine.ReferenceVariables);
            Assert.IsNotNull(Machine.Variables);
            Assert.AreEqual(7, Machine.Variables.Count());
        }

        /// <summary>
        /// Check if dynamic variables are in initial position
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task OptimizeTest()
        {
            Machine.Optimized = false;
            await Machine.Prepare();
            Assert.AreEqual(7, Machine.Variables.Count());
            Assert.IsNotNull(Machine.Variables);
            var initialValue = Machine.Variables.Get("_Value_a");
            Assert.IsNotNull(initialValue);
            Assert.AreEqual(0, initialValue.Value);
            var initialStock = Machine.Variables.Get("_Stock_a");
            Assert.IsNotNull(initialStock);
            Assert.AreEqual(0, initialStock.Value);
            Machine.Process();
            var variable = Machine.Variables.Get("_Value_a");
            Assert.IsNotNull(variable);
            Assert.AreNotEqual(initialValue, variable.Value);
            variable = Machine.Variables.Get("_Stock_a");
            Assert.IsNotNull(variable);
            Assert.AreNotEqual(initialStock, variable.Value);
        }
    }
}