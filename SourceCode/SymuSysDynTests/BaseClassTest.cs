#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Model;
using Symu.SysDyn.Parser;
using Symu.SysDyn.Simulation;

#endregion

namespace SymuSysDynTests
{
    [TestClass]
    public abstract class BaseClassTest
    {
        protected const string TestFile =
            @"C:\Users\laure\Dropbox\Symu\SourceCode\Symu.SysDyn\Github\SourceCode\SymuSysDynTests\Templates\Test.xmile";

        protected BaseClassTest()
        {
            Machine = new StateMachine(TestFile);
            XDoc = XDocument.Load(TestFile);
            Ns = XDoc.Root?.Attributes("xmlns").First().Value;
            XElement = XDoc.Root?.Descendants(Ns + "variables").First();
        }

        protected XmlParser Parser { get; } = new XmlParser(TestFile);
        protected Variables Variables { get; } = new Variables();
        protected XDocument XDoc { get; }
        protected XNamespace Ns { get; }
        protected XElement XElement { get; set; }
        protected StateMachine Machine { get; }
    }
}