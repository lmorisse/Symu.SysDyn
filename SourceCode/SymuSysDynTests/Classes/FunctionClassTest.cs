using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Core.Engine;
using Symu.SysDyn.Core.Models.XMile;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Symu.SysDyn.Tests.Classes
{
    [TestClass]
    public class FunctionClassTest
    {
        protected const string TestFile =
            ClassPath.classpath + "Function_capitalization.xmile";

        /// <summary>
        ///     It is not the Machine.Variables
        /// </summary>
        protected XMileModel Variables { get; } = new XMileModel("1");

        protected XDocument XDoc { get; private set; }
        protected XNamespace Ns { get; private set; }
        protected XElement XElement { get; set; }
        protected StateMachine Machine { get; private set; }

        protected async Task Initialize()
        {
            Machine = await StateMachine.CreateStateMachine(TestFile);
            XDoc = XDocument.Load(TestFile);
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
    }
}
