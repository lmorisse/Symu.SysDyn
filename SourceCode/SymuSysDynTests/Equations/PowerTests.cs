#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2021 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Tests.Classes;
using Symu.SysDyn.Core.Equations;
using System.Threading.Tasks;
#endregion

namespace Symu.SysDyn.Tests.Equations
{
    [TestClass]
    public class PowerTests : BaseClassTest
    {
        [TestInitialize]
        public async Task InitializeTest()
        {
            await Initialize();
        }

        [DataRow("2^2")]
        [DataRow("(2)^(2)")]
        [DataRow("(2^2)")]
        [TestMethod]
        public void ParsePowerSimple(string function)
        {
            var factory = Power.Parse(function);
            Assert.AreEqual("pow(2,2)", factory);
        }

        [DataRow("3^3^3")]
        [DataRow("(3^3^3)")]
        [DataRow("(3^3)^3")]
        [TestMethod]
        public void ParsePowerMultipleSimple(string function)
        {
            var factory = Power.ParseMultiple(function);
            Assert.AreEqual("pow(pow(3,3),3)", factory);
        }

        [DataRow("(3^3)^(2^2)")]
        [TestMethod]
        public void ParsePowerDoubleSimple(string function)
        {
            var factory = Power.Parse(function);
            Assert.AreEqual("pow(pow(3,3),pow(2,2))", factory);
        }

        [DataRow("(3^3^3)^(2^2^2)")]
        [TestMethod]
        public void ParsePowerDoubleMultiple(string function)
        {
            var factory = Power.Parse(function);
            Assert.AreEqual("pow(pow(pow(3,3),3),pow(pow(2,2),2))", factory);
        }
    }
}
