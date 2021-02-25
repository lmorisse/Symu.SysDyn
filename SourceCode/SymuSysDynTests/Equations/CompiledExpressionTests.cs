#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2021 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;
using NCalcAsync;
using Symu.SysDyn.Core.Equations;

#endregion

namespace Symu.SysDyn.Tests.Equations
{
    [TestClass]
    public class CompiledExpressionTests
    {
        #region Get

        /// <summary>
        ///     Nested Functions
        /// </summary>
        [DataRow("Sqrt(sqrt(4))", true)]
        [DataRow("STEP(3, 5)", false)]
        [TestMethod]
        public void GetTest(string function, bool optimized)
        {
            var compile = CompiledExpression.Get(function, string.Empty);
            Assert.AreEqual(0, compile.Parameters.Count);
            Assert.AreEqual(optimized, compile.CanBeOptimized);
        }

        /// <summary>
        ///     With parameters
        /// </summary>
        [TestMethod]
        public void GetTest2()
        {
            var compile = CompiledExpression.Get("(variable1 + (sqrt(4)*(Variable2+5)))", string.Empty);
            Assert.AreEqual(2, compile.Parameters.Count);
            Assert.AreEqual("_Variable1", compile.Parameters[0]);
            Assert.AreEqual("_Variable2", compile.Parameters[1]);
            Assert.IsTrue(compile.CanBeOptimized);
        }

        #endregion

        #region Replace

        [TestMethod]
        public void ReplaceTest()
        {
            var e = new Expression("(variable1 + (variable1_1*(Variable2+5)))");
            e.HasErrors(); // To set the parsedEquation
            var replacedEquation =
                CompiledExpression.ReplaceLogicalExpression(e.ParsedExpression, string.Empty, "variable1", "1");
            Assert.AreEqual("(1+(variable1_1*(Variable2+5)))", replacedEquation);
        }

        /// <summary>
        ///     Using Function
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public void ReplaceTest2()
        {
            var e = new Expression("Normal(variable1,variable2)");
            e.HasErrors(); // To set the parsedEquation
            var replacedEquation =
                CompiledExpression.ReplaceLogicalExpression(e.ParsedExpression, string.Empty, "variable1", "1");
            Assert.AreEqual("Normal(1,variable2)", replacedEquation);
            e = new Expression(replacedEquation);
            e.HasErrors(); // To set the parsedEquation
            replacedEquation =
                CompiledExpression.ReplaceLogicalExpression(e.ParsedExpression, string.Empty, "variable2", "1");
            Assert.AreEqual("Normal(1,1)", replacedEquation);
        }

        #endregion
    }
}