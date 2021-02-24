﻿#region Licence

// Description: SymuBiz - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NCalcAsync;

#endregion

namespace SymuSysDynTests.Functions
{
    /// <summary>
    ///     Use to explore the capabilities of NCalc2
    /// </summary>
    [TestClass]
    public class ExecutionTests
    {
        [TestMethod]
        public async Task NestedFunctionTest()
        {
            var e = new Expression("Pow(Abs(X1),X2)") {Parameters = {["X1"] = 1, ["X2"] = 1}};

            Assert.AreEqual(1F, Convert.ToUInt16(await e.EvaluateAsync()));
        }

        [TestMethod]
        public async Task NoSeparatorTest()
        {
            var e = new Expression("Abs(5)+Pow(2,2)-5");
            Assert.AreEqual(4F, Convert.ToUInt16(await e.EvaluateAsync()));
        }

        [TestMethod]
        public async Task ParametersTest()
        {
            var e = new Expression("Abs(Param1)") {Parameters = {["Param1"] = -2}};
            Assert.AreEqual(2F, Convert.ToUInt16(await e.EvaluateAsync()));
        }


        #region BuilIn functions

        [TestMethod]
        public async Task IfThenElseTest()
        {
            var e = new Expression("if(1<1,1,2)");
            Assert.AreEqual(2, Convert.ToUInt16(await e.EvaluateAsync()));
            // 0 is false
            e = new Expression("if(0,1,2)");
            Assert.AreEqual(2, Convert.ToUInt16(await e.EvaluateAsync()));
            // other is true 
            e = new Expression("if(1,1,2)");
            Assert.AreEqual(1, Convert.ToUInt16(await e.EvaluateAsync()));
        }

        #endregion
    }
}