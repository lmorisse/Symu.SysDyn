#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2021 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Core.Parser;

#endregion

namespace Symu.SysDyn.Tests.Parser
{
    [TestClass]
    public class StringUtilsTests
    {
        [TestMethod]
        public void CleanNameTest()
        {
            Assert.AreEqual("Test_avec_plein_de_problemes",
                StringUtils.CleanName("\rtest  Avec_____plein\nde\\nProbleme's\r"));
        }

        [TestMethod]
        public void CleanNamesTest()
        {
            var names = new List<string> {"test  Avec_____plein\nde_Problemes"};
            names = StringUtils.CleanNames(names);
            Assert.AreEqual("Test_avec_plein_de_problemes", names[0]);
        }

        [TestMethod]
        public void CleanFullNameTest()
        {
            Assert.AreEqual("_Variable", StringUtils.CleanFullName(".variable"));
            Assert.AreEqual("Model_Variable", StringUtils.CleanFullName("model.variable"));
            Assert.AreEqual("Model_Variable", StringUtils.CleanFullName("mOdel.vAriable"));
        }

        /// <summary>
        ///     Non passing tests
        /// </summary>
        [DataRow(null)]
        [DataRow("")]
        [TestMethod]
        public void GetStringInBracesTest(string input)
        {
            Assert.IsNull(StringUtils.GetStringInBraces(input));
        }

        /// <summary>
        ///     Non passing tests
        /// </summary>
        [TestMethod]
        public void GetStringInBracesTest1()
        {
            Assert.AreEqual(string.Empty, StringUtils.GetStringInBraces("test"));
        }

        /// <summary>
        ///     Passing test
        /// </summary>
        [TestMethod]
        public void GetStringInBracesTest2()
        {
            var test = StringUtils.GetStringInBraces("test1 {test2}");
            Assert.IsNotNull(test);
            Assert.AreEqual("test2", test);
        }
    }
}