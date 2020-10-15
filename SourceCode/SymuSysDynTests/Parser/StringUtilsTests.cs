using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Parser;


namespace SymuSysDynTests.Parser
{
    [TestClass()]
    public class StringUtilsTests
    {


        [TestMethod()]
        public void CleanNameTest()
        {
            Assert.AreEqual("Test_avec_plein_de_problemes", StringUtils.CleanName("test  Avec_____plein/nde_Problemes"));
        }

        [TestMethod()]
        public void CleanNamesTest()
        {
            var names = new List<string> { "test  Avec_____plein/nde_Problemes" };
            names = StringUtils.CleanNames(names);
            Assert.AreEqual("Test_avec_plein_de_problemes", names[0]);
        }
        /// <summary>
        /// Non passing tests
        /// </summary>
        [DataRow(null)]
        [DataRow("")]
        [TestMethod()]
        public void CreateInstanceFromEquationTest(string input)
        {
            Assert.IsNull(StringUtils.GetStringInBraces(input));
        }
        /// <summary>
        /// Non passing tests
        /// </summary>
        [TestMethod()]
        public void CreateInstanceFromEquationTest1()
        {
            Assert.AreEqual(string.Empty, StringUtils.GetStringInBraces("test"));
        }
        /// <summary>
        /// Passing test
        /// </summary>
        [TestMethod()]
        public void CreateInstanceFromEquationTest2()
        {
            var test = StringUtils.GetStringInBraces("test1 {test2}");
            Assert.IsNotNull(test);
            Assert.AreEqual("test2", test);
        }
    }
}