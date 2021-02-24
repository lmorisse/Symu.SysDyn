#region Licence

// Description: SymuBiz - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Core.Functions;

#endregion

namespace SymuSysDynTests.Functions
{
    [TestClass]
    public class FunctionUtilsTests
    {
        [TestMethod]
        public void ParseStringFunctionsTest()
        {
            const string test =
                "xxx func0(a)+b + func1(a,b+c) - func2(a*b,func3(a+b,c)) * func4(e)+func5((f))+func6(func7(g,h)+func8(i,(a)=>a+2)) yyy";
            var result = FunctionUtils.ParseStringFunctions(test);
            Assert.AreEqual(6, result.Count);
            Assert.AreEqual("func0(a)", result[0]);
            Assert.AreEqual("func1(a,b+c)", result[1]);
            Assert.AreEqual("func2(a*b,func3(a+b,c))", result[2]);
            Assert.AreEqual("func4(e)", result[3]);
            Assert.AreEqual("func5((f))", result[4]);
            Assert.AreEqual("func6(func7(g,h)+func8(i,(a)=>a+2))", result[5]);
        }

        [TestMethod]
        public void ParseStringFunctionsTest1()
        {
            const string test =
                "Func1(param1,param2)+someStuffAfterFunction + DT + TIME + STEP( 1 , 2)-Normal(1,2)*RAMP(2,1)";
            var result = FunctionUtils.ParseStringFunctions(test);
            Assert.AreEqual(6, result.Count);
            Assert.AreEqual("Func1(param1,param2)", result[0]);
            Assert.AreEqual("DT", result[1]);
            Assert.AreEqual("TIME", result[2]);
            Assert.AreEqual("STEP( 1 , 2)", result[3]);
            Assert.AreEqual("Normal(1,2)", result[4]);
            Assert.AreEqual("RAMP(2,1)", result[5]);
        }

        [TestMethod]
        public void ParseStringFunctionsTest1Bis()
        {
            const string test = "TIME";
            var result = FunctionUtils.ParseStringFunctions(test);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("TIME", result[0]);
        }

        [TestMethod]
        public void ParseStringFunctionsTest1Ter()
        {
            const string test = "DT";
            var result = FunctionUtils.ParseStringFunctions(test);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("DT", result[0]);
        }


        /// <summary>
        ///     Non passing test
        /// </summary>
        [TestMethod]
        public void ParseStringFunctionsTest2()
        {
            const string test = "someStuffBeforeFunction + ( param1 ) + ((param2) + (param3)) + xxx";
            var results = FunctionUtils.ParseStringFunctions(test);
            Assert.AreEqual(0, results.Count);
        }

        /// <summary>
        ///     passing test : Is variable false then true
        /// </summary>
        [TestMethod]
        public void ParseStringFunctionsTest3()
        {
            const string test = "Dt* (variable1 - variable2)-Time+SET(3,5)";
            var results = FunctionUtils.ParseStringFunctions(test);
            Assert.AreEqual(3, results.Count);
            Assert.AreEqual("Dt", results[0]);
            Assert.AreEqual("Time", results[1]);
            Assert.AreEqual("SET(3,5)", results[2]);
        }



        /// <summary>
        ///     Passing tests
        /// </summary>
        [TestMethod]
        public async Task GetFunctionsTest()
        {
            const string test =
                "Func1((param1),(param2))+DT + TIME + PULSE( 1 , 2)-Normal(1,2)*Func2(2,1)-ExternalUpdate+2";
            var results = await FunctionUtils.ParseFunctions(string.Empty, test);
            // Result is 8 because Func1((param1),(param2)) appears twice, once with a false result : Func1((param1))
            // To correct
            Assert.AreEqual(7, results.Count);
            Assert.AreEqual("Func1", results[0].Name);
            Assert.AreEqual("Dt", results[1].Name);
            Assert.AreEqual("Time", results[2].Name);
            Assert.AreEqual("Pulse", results[3].Name);
            Assert.AreEqual("Normal", results[4].Name);
            Assert.AreEqual("Func2", results[5].Name);
            Assert.AreEqual("Externalupdate", results[6].Name);
        }

        /// <summary>
        ///     Non passing test
        /// </summary>
        [TestMethod]
        public async Task GetFunctionsTest1()
        {
            const string test = "someStuffBeforeFunction + ( param1 ) + ( param2 + param3 ) + xxx";
            var results = await FunctionUtils.ParseFunctions(string.Empty, test);
            Assert.AreEqual(0, results.Count);
        }


        /// <summary>
        ///     If then else test
        /// </summary>
        [TestMethod]
        public async Task GetFunctionsTest21()
        {
            const string test = "If x1 then x2 else x3";
            var results = await FunctionUtils.ParseFunctions(string.Empty, test);
            Assert.AreEqual(1, results.Count);
            Assert.IsTrue(results[0] is IfThenElse);
            Assert.AreEqual(3, results[0].Parameters.Count);
            Assert.AreEqual("_X1", results[0].Parameters[0].ToString());
            Assert.AreEqual("_X2", results[0].Parameters[1].ToString());
            Assert.AreEqual("_X3", results[0].Parameters[2].ToString());
        }

        /// <summary>
        ///     If then else test with brackets
        ///     If() may be identified as a function
        /// </summary>
        [TestMethod]
        public async Task GetFunctionsTest22()
        {
            const string test = "If(x1) then x2 else x3";
            var results = await FunctionUtils.ParseFunctions(string.Empty, test);
            Assert.AreEqual(1, results.Count);
            Assert.IsTrue(results[0] is IfThenElse);
            Assert.AreEqual(3, results[0].Parameters.Count);
            Assert.AreEqual("_X1", results[0].Parameters[0].ToString());
            Assert.AreEqual("_X2", results[0].Parameters[1].ToString());
            Assert.AreEqual("_X3", results[0].Parameters[2].ToString());
        }

        /// <summary>
        ///     If then else test with brackets
        ///     If() may be identified as a function
        /// </summary>
        [TestMethod]
        public async Task GetFunctionsTest23()
        {
            const string test = "If(TIME < 3) then x2 else (TIME-3)";
            var results = await FunctionUtils.ParseFunctions(string.Empty, test);
            Assert.AreEqual(1, results.Count);
            Assert.IsTrue(results[0] is IfThenElse);
            Assert.AreEqual(3, results[0].Parameters.Count);
            Assert.AreEqual("Time0<3", results[0].Parameters[0].ToString());
            Assert.AreEqual("_X2", results[0].Parameters[1].ToString());
            Assert.AreEqual("Time0-3", results[0].Parameters[2].ToString());
        }

        [DataRow("Func", "Func")]
        [DataRow("Func()", "Func")]
        [TestMethod]
        public async Task GetParametersTest(string function, string name)
        {
            var builtInFunction = await FunctionUtils.ParseParameters<BuiltInFunction>(string.Empty, function);
            Assert.AreEqual(0, builtInFunction.Parameters.Count);
            Assert.AreEqual(name, builtInFunction.Name);
        }

        [TestMethod]
        public async Task GetParametersTest2()
        {
            const string function = "Func(param1)";
            var builtInFunction = await FunctionUtils.ParseParameters<BuiltInFunction>(string.Empty, function);
            Assert.AreEqual(1, builtInFunction.Parameters.Count);
            Assert.AreEqual("_Param1", builtInFunction.Parameters[0].Variables.First());
            Assert.AreEqual("Func", builtInFunction.Name);
        }

        [TestMethod]
        public async Task GetParametersTest3()
        {
            const string function = "Func(param1, param2)";
            var builtInFunction = await FunctionUtils.ParseParameters<BuiltInFunction>(string.Empty, function);
            Assert.AreEqual(2, builtInFunction.Parameters.Count);
            Assert.AreEqual("_Param1", builtInFunction.Parameters[0].Variables.First());
            Assert.AreEqual("_Param2", builtInFunction.Parameters[1].Variables.First());
            Assert.AreEqual("Func", builtInFunction.Name);
        }

        [TestMethod]
        public async Task GetParametersTest4()
        {
            const string function = @"someFunc((a),b,func1(a,b+c),func2(a*b,func3(a+b,c)),func4(e)+func5(f),func6(func7(g,h)+func8(i,(a)=>a+2)),g+2)";

            var builtInFunction = await FunctionUtils.ParseParameters<BuiltInFunction>(string.Empty, function);
            Assert.AreEqual(7, builtInFunction.Parameters.Count);
            Assert.AreEqual("Somefunc", builtInFunction.Name);
        }
    }
}