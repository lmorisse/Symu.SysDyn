using Microsoft.VisualStudio.TestTools.UnitTesting;

using Symu.SysDyn.Core.Equations;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Symu.SysDyn.Core.Equations;

namespace Symu.SysDyn.Core.Equations.Tests
{
    [TestClass()]
    public class CompiledExpressionTests
    {
        /// <summary>
        /// Nested Functions
        /// </summary>
        [TestMethod]
        public void GetCompiledExpressionTest()
        {
            var compile = CompiledExpression.GetCompiledExpression("Sqrt(sqrt(4))");
            Assert.AreEqual(0, compile.Parameters.Count);
            Assert.AreEqual(2, compile.Functions.Count);
            Assert.AreEqual("Sqrt", compile.Functions[0]);
            Assert.AreEqual("sqrt", compile.Functions[1]);

        }
        /// <summary>
        /// Functions and parameters
        /// </summary>
        [TestMethod]
        public void ExploratoryTest()
        {
            var compile = CompiledExpression.GetCompiledExpression("(variable1 + (sqrt(4)*(Variable2+5)))");
            Assert.AreEqual(2, compile.Parameters.Count);
            Assert.AreEqual("variable1", compile.Parameters[0]);
            Assert.AreEqual("Variable2", compile.Parameters[1]);
            Assert.AreEqual(1, compile.Functions.Count);
            Assert.AreEqual("sqrt", compile.Functions[0]);

        }
    }
}