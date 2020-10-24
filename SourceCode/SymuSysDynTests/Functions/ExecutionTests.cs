using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NCalc2;

namespace SymuSysDynTests.Functions
{
    /// <summary>
    /// Use to explore the capabilities of NCalc2
    /// </summary>
    [TestClass()]
    public class ExecutionTests
    {
        [TestMethod]
        public void NestedFunctionTest()
        {
            var e = new Expression("Pow(Abs(X1),X2)") {Parameters = {["X1"] = 1, ["X2"] = 1}};

            Assert.AreEqual(1F, Convert.ToUInt16(e.Evaluate()));
        }
        [TestMethod]
        public void NoSeparatorTest()
        {
            var e = new Expression("Abs(5)+Pow(2,2)-5");
            Assert.AreEqual(4F, Convert.ToUInt16(e.Evaluate()));
        }
        [TestMethod]
        public void ParametersTest()
        {
            var e = new Expression("Abs(Param1)") {Parameters = {["Param1"] = -2}};
            Assert.AreEqual(2F, Convert.ToUInt16(e.Evaluate()));
        }


        #region BuilIn functions


        [TestMethod]
        public void IfThenElseTest()
        {
            var e = new Expression("if(1<1,1,2)");
            Assert.AreEqual(2, Convert.ToUInt16(e.Evaluate()));
            // 0 is false
            e = new Expression("if(0,1,2)");
            Assert.AreEqual(2, Convert.ToUInt16(e.Evaluate()));
            // other is true 
            e = new Expression("if(1,1,2)");
            Assert.AreEqual(1, Convert.ToUInt16(e.Evaluate()));
        }
        #endregion


    }
}