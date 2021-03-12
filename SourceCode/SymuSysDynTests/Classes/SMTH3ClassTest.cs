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

#endregion

namespace Symu.SysDyn.Tests.Classes
{
    [TestClass]
    public abstract class Smth3ClassTest
    {
        protected const string TestFile =
           ClassPath.classpath + "SMTH3.xmile";

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