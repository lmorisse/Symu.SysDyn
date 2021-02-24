#region Licence

// Description: SymuBiz - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Core.Engine;
using Symu.SysDyn.Core.Models.XMile;
using Symu.SysDyn.Core.Parser;

#endregion

namespace SymuSysDynTests.Classes
{
    [TestClass]
    public abstract class BaseClassTest
    {
        protected const string TestFile =
            @"C:\Users\laure\Dropbox\Symu\SourceCode\Symu.SysDyn\Github\SourceCode\SymuSysDynTests\Templates\Test.xmile";

        protected async Task Initialize()
        {
            Machine = await StateMachine.CreateStateMachine(TestFile);
            XDoc = XDocument.Load(TestFile);
            Ns = XDoc.Root?.Attributes("xmlns").First().Value;
            XElement = XDoc.Root?.Descendants(Ns + "variables").First();
        }

        protected XmlParser Parser { get; } = new XmlParser(TestFile);

        /// <summary>
        ///     It is not the Machine.Models
        /// </summary>
        protected XMileModel Model { get;  } = new XMileModel("1");

        /// <summary>
        ///     It is not the Machine.Models
        /// </summary>
        protected XMileModel RootModel => Machine.Models.RootModel;

        protected XDocument XDoc { get; private set; }
        protected XNamespace Ns { get; private set; }
        protected XElement XElement { get; set; }
        protected StateMachine Machine { get; private set; }
    }
}