using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Functions;

namespace SymuSysDynTests.Functions
{
    [TestClass()]
    public class IfThenElseTests
    {

        [TestMethod]
        public void ParseTest()
        {
            CheckOkTest(IfThenElse.Parse("IF x1 THEN x2 ELSE x3"));
            CheckOkTest(IfThenElse.Parse("If x1 Then x2 Else x3"));
            CheckOkTest(IfThenElse.Parse("if x1 then x2 else x3"));
            CheckKoTest(IfThenElse.Parse("IF x1 THEN x2"));
            CheckKoTest(IfThenElse.Parse("IF x1 ELSE x3"));
        }

        private static void CheckOkTest(IReadOnlyList<IEquation> parameters)
        {
            Assert.AreEqual(3, parameters.Count);
            Assert.AreEqual("X1", parameters[0].Variables.First());
            Assert.AreEqual("X2", parameters[1].Variables.First());
            Assert.AreEqual("X3", parameters[2].Variables.First());
        }

        private static void CheckKoTest(ICollection parameters)
        {
            Assert.AreEqual(0, parameters.Count);
        }


        [TestMethod]
        public void SetEquationTest()
        {
            var function = new IfThenElse("IF x1 THEN x2 ELSE x3");
            Assert.AreEqual("if(X1,X2,X3)", function.SetEquation());
        }
    }
}