using Symu.SysDyn.Core.Engine;
using Symu.SysDyn.Core.Models.XMile;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Symu.SysDyn.Tests.Classes
{
    public class FunctionClassTest
    {
        protected const string TestFile =
            @"C:\Users\laure\Dropbox\Symu\SourceCode\Symu.SysDyn\Github\SourceCode\SymuSysDynTests\Templates\Function_capitalization.xmile";

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
    }
}
