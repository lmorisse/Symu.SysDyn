#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Linq;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Engine;
using Symu.SysDyn.Models;
using Symu.SysDyn.Parser;

#endregion

namespace SymuSysDynTests
{
    [TestClass]
    public abstract class Smth3ClassTest
    {
        protected const string TestFile =
            @"C:\Users\laure\Dropbox\Symu\SourceCode\Symu.SysDyn\Github\SourceCode\SymuSysDynTests\Templates\SMTH3.xmile";

        protected Smth3ClassTest()
        {
            Machine = new StateMachine(TestFile);
            XDoc = XDocument.Load(TestFile);
            Ns = XDoc.Root?.Attributes("xmlns").First().Value;
            XElement = XDoc.Root?.Descendants(Ns + "variables").First();
        }

        /// <summary>
        ///     It is not the Machine.Variables
        /// </summary>
        protected Model Variables { get; } = new Model("1");

        protected XDocument XDoc { get; }
        protected XNamespace Ns { get; }
        protected XElement XElement { get; set; }
        protected StateMachine Machine { get; }
    }
}