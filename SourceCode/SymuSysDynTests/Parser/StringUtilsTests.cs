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

namespace Symu.SysDyn.Core.Parser.Tests
{
    [TestClass()]
    public class StringUtilsTests
    {
        //Test of the classic special characters
        [TestMethod()]
        public void CleanSpecialCharsTest()
        {
            Assert.AreEqual("Ceciestuntest", StringUtils.CleanSpecialChars("Ceci es/t un/ t()e=s+t&"));

        }

        // Test to see if the result is null if there is only special characters
        [TestMethod()]
        public void CleanSpecialCharsTest2()
        {
            Assert.AreEqual("", StringUtils.CleanSpecialChars("$$$$$$$$$"));

        }
        //Test to see if it's null if the input is null
        [TestMethod()]
        public void CleanSpecialCharsTest3()
        {
            Assert.AreEqual("", StringUtils.CleanSpecialChars(""));

        }
        //Test to see if it doesn't remove the underscore
        [TestMethod()]
        public void CleanSpecialCharsTest4()
        {
            Assert.AreEqual("Ceci_est_un_test", StringUtils.CleanSpecialChars("Ceci_es/t_un/_te=s+t&"));

        }
        //Test to see all special characters
        [TestMethod()]
        public void CleanSpecialCharsTest5()
        {
            Assert.AreEqual("TestavecPleindeChpar", StringUtils.CleanSpecialChars("Test@av-+e/cPle^in/deC..h$par^^"));

        }
    }
}

namespace Symu.SysDyn.Tests.Parser
{
    [TestClass]
    public class StringUtilsTests
    {
        //Test to see if it remove the \r \n \\n 
        [TestMethod]
        public void CleanNameTest()
        {
            Assert.AreEqual("Test_avec_plein_de_problemes",
                StringUtils.CleanName("\rtest  Avec_____plein\nde\\nProbleme's\r"));
        }

        //Test to see if the UpperCase are well processed
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