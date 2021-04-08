using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Core.Engine;
using Symu.SysDyn.Core.Models.XMile;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Symu.SysDyn.Tests.Classes
{
    [TestClass]
    public class FunctionClassTest
    {
        protected const string TestFile =
            ClassPath.classpath;

        /// <summary>
        ///     It is not the Machine.Variables
        /// </summary>
        protected XMileModel Variables { get; } = new XMileModel("1");

        protected XDocument XDoc { get; private set; }
        protected XNamespace Ns { get; private set; }
        protected XElement XElement { get; set; }
        protected StateMachine Machine { get; private set; }

        protected async Task Initialize(string path)
        {
            var testFile2 = TestFile + path;
            Machine = await StateMachine.CreateStateMachine(testFile2);
            XDoc = XDocument.Load(testFile2);
            Ns = XDoc.Root?.Attributes("xmlns").First().Value;
            XElement = XDoc.Root?.Descendants(Ns + "variables").First();
        }

        [TestMethod]
        public virtual void StateMachineTest()
        {
            Assert.IsNotNull(Machine.Simulation);
            Assert.IsNotNull(Machine.Results);
            Assert.IsNotNull(Machine.ReferenceVariables);
            Assert.IsNotNull(Machine.Variables);
        }


        [TestMethod]
        protected void TestVariable(string name, float value)
        {
            var variable = Machine.Variables.Get(name);
            Assert.IsNotNull(variable);
            Assert.AreEqual(value, variable.Value);
        }
        [TestMethod]
        protected void TestVariableNotEqual(string name, float value)
        {
            var variable = Machine.Variables.Get(name);
            Assert.IsNotNull(variable);
            Assert.AreNotEqual(value, variable.Value);
        }

        [TestMethod]
        protected void TestVariableFloat(string name, float value, int comma)
        {
            var variable = Machine.Variables.Get(name);
            Assert.IsNotNull(variable);
            Assert.AreEqual(value, (float)Math.Round(variable.Value, comma));
        }
        protected void TestTwoVariables(string name1, string name2)
        {
            var variable1 = Machine.Variables.Get(name1);
            var variable2 = Machine.Variables.Get(name2);
            Assert.IsNotNull(variable1);
            Assert.IsNotNull(variable2);
            Assert.AreEqual(variable1.Value, variable2.Value);
        }
    

        
    }
}
