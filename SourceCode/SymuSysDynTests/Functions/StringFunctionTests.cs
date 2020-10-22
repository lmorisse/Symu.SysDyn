using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Functions;

namespace SymuSysDynTests.Functions
{
    [TestClass()]
    public class StringFunctionTests
    {
        /// <summary>
        ///     Passing tests
        /// </summary>
        [TestMethod]
        public void GetStringFunctionsTest()
        {
            const string test =
                "someStuffBeforeFunction Func1(param1,param2)+someStuffAfterFunction + DT + TIME + STEP( 1 , 2)-Normal(1,2)*RAMP(2,1)";
            var results = StringFunction.GetStringFunctions(test).ToList();
            Assert.AreEqual(6, results.Count);
            Assert.AreEqual("Func1", results[0].Name);
            Assert.AreEqual("Step", results[1].Name);
            Assert.IsTrue(results[1] is Step);
            Assert.IsTrue(results[2] is Normal);
            Assert.AreEqual("Normal", results[2].Name);
            Assert.IsTrue(results[3] is Ramp);
            Assert.AreEqual("Ramp", results[3].Name);
            Assert.IsTrue(results[4] is Dt);
            Assert.IsTrue(results[5] is Time);
        }

        /// <summary>
        ///     Non passing test
        /// </summary>
        [TestMethod]
        public void GetStringFunctionsTest1()
        {
            const string test = "someStuffBeforeFunction + ( param1 ) + ( param2 + param3 ) + xxx";
            var results = StringFunction.GetStringFunctions(test).ToList();
            Assert.AreEqual(0, results.Count);
        }


        /// <summary>
        ///     If then else test
        /// </summary>
        [TestMethod]
        public void GetStringFunctionsTest21()
        {
            const string test = "If x1 then x2 else x3";
            var results = StringFunction.GetStringFunctions(test).ToList();
            Assert.AreEqual(1, results.Count);
            Assert.IsTrue(results[0] is IfThenElse);
        }
    }
}