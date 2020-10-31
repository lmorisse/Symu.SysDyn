#region Licence

// Description: SymuSysDyn - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Equations;

#endregion

namespace SymuSysDynTests.Equations
{
    [TestClass]
    public class AllBuiltInFunctionsTests
    {
        [TestMethod]
        public void TimeTest()
        {
            var equation = EquationFactory.CreateInstance("TIME", out _);
            Assert.IsInstanceOfType(equation, typeof(ComplexEquation));
            Assert.AreEqual("Time0", equation.InitializedEquation);
            Assert.AreEqual(0, equation.Variables.Count);
        }

        [TestMethod]
        public void DtTest()
        {
            var equation = EquationFactory.CreateInstance("DT", out _);
            Assert.IsInstanceOfType(equation, typeof(ComplexEquation));
            Assert.AreEqual("Dt0", equation.InitializedEquation);
            Assert.AreEqual(0, equation.Variables.Count);
        }

        [TestMethod]
        public void StepTest()
        {
            var equation = EquationFactory.CreateInstance("STEP(Height,StartTime)", out _);
            Assert.IsInstanceOfType(equation, typeof(ComplexEquation));
            Assert.AreEqual("Step0", equation.InitializedEquation);
            Assert.AreEqual(2, equation.Variables.Count);
            Assert.AreEqual("Height", equation.Variables[0]);
            Assert.AreEqual("Starttime", equation.Variables[1]);
        }

        [TestMethod]
        public void RampTest()
        {
            //Time is a reserved word
            var equation = EquationFactory.CreateInstance("RAMP(StartTime,Slope)", out _);
            Assert.IsInstanceOfType(equation, typeof(ComplexEquation));
            Assert.AreEqual("Ramp0", equation.InitializedEquation);
            Assert.AreEqual(2, equation.Variables.Count);
            Assert.AreEqual("Starttime", equation.Variables[0]);
            Assert.AreEqual("Slope", equation.Variables[1]);
        }

        [TestMethod]
        public void NormalTest()
        {
            var equation = EquationFactory.CreateInstance("Normal(Mean,StandardDeviation)", out _);
            Assert.IsInstanceOfType(equation, typeof(ComplexEquation));
            Assert.AreEqual("Normal0", equation.InitializedEquation);
            Assert.AreEqual(2, equation.Variables.Count);
            Assert.AreEqual("Mean", equation.Variables[0]);
            Assert.AreEqual("Standarddeviation", equation.Variables[1]);
        }

        [TestMethod]
        public void NormalTest1()
        {
            var equation = EquationFactory.CreateInstance("Normal(Mean,StandardDeviation,Seed)", out _);
            Assert.IsInstanceOfType(equation, typeof(ComplexEquation));
            Assert.AreEqual("Normal0", equation.InitializedEquation);
            Assert.AreEqual(3, equation.Variables.Count);
            Assert.AreEqual("Mean", equation.Variables[0]);
            Assert.AreEqual("Standarddeviation", equation.Variables[1]);
            Assert.AreEqual("Seed", equation.Variables[2]);
        }

        [TestMethod]
        public void IfThenElseTest()
        {
            var equation = EquationFactory.CreateInstance("If condition then expression1 else expression2", out _);
            Assert.IsInstanceOfType(equation, typeof(ComplexEquation));
            Assert.AreEqual("If0", equation.InitializedEquation);
            Assert.AreEqual(3, equation.Variables.Count);
            Assert.AreEqual("Condition", equation.Variables[0]);
            Assert.AreEqual("Expression1", equation.Variables[1]);
            Assert.AreEqual("Expression2", equation.Variables[2]);
        }

        [TestMethod]
        public void Smth1Test()
        {
            var equation = EquationFactory.CreateInstance("SMTH1(Input, Averaging)", out _);
            Assert.IsInstanceOfType(equation, typeof(ComplexEquation));
            Assert.AreEqual("Smth10", equation.InitializedEquation);
            Assert.AreEqual(2, equation.Variables.Count);
            Assert.AreEqual("Input", equation.Variables[0]);
            Assert.AreEqual("Averaging", equation.Variables[1]);
        }

        [TestMethod]
        public void Smth1Test1()
        {
            var equation = EquationFactory.CreateInstance("SMTH1(Input, Averaging, Initial)", out _);
            Assert.IsInstanceOfType(equation, typeof(ComplexEquation));
            Assert.AreEqual("Smth10", equation.InitializedEquation);
            Assert.AreEqual(3, equation.Variables.Count);
            Assert.AreEqual("Input", equation.Variables[0]);
            Assert.AreEqual("Averaging", equation.Variables[1]);
            Assert.AreEqual("Initial", equation.Variables[2]);
        }

        [TestMethod]
        public void Smth3Test()
        {
            var equation = EquationFactory.CreateInstance("SMTH3(Input, Averaging)", out _);
            Assert.IsInstanceOfType(equation, typeof(ComplexEquation));
            Assert.AreEqual("Smth30", equation.InitializedEquation);
            Assert.AreEqual(2, equation.Variables.Count);
            Assert.AreEqual("Input", equation.Variables[0]);
            Assert.AreEqual("Averaging", equation.Variables[1]);
        }

        [TestMethod]
        public void Smth3Test1()
        {
            var equation = EquationFactory.CreateInstance("SMTH3(Input, Averaging, Initial)", out _);
            Assert.IsInstanceOfType(equation, typeof(ComplexEquation));
            Assert.AreEqual("Smth30", equation.InitializedEquation);
            Assert.AreEqual(3, equation.Variables.Count);
            Assert.AreEqual("Input", equation.Variables[0]);
            Assert.AreEqual("Averaging", equation.Variables[1]);
            Assert.AreEqual("Initial", equation.Variables[2]);
        }

        [TestMethod]
        public void SmthNTest()
        {
            var equation = EquationFactory.CreateInstance("SMTHN(Input, Averaging, 2)", out _);
            Assert.IsInstanceOfType(equation, typeof(ComplexEquation));
            Assert.AreEqual("Smthn0", equation.InitializedEquation);
            Assert.AreEqual(2, equation.Variables.Count);
            Assert.AreEqual("Input", equation.Variables[0]);
            Assert.AreEqual("Averaging", equation.Variables[1]);
        }

        [TestMethod]
        public void SmthNTest1()
        {
            var equation = EquationFactory.CreateInstance("SMTHN(Input, Averaging, 3, Initial)", out _);
            Assert.IsInstanceOfType(equation, typeof(ComplexEquation));
            Assert.AreEqual("Smthn0", equation.InitializedEquation);
            Assert.AreEqual(3, equation.Variables.Count);
            Assert.AreEqual("Input", equation.Variables[0]);
            Assert.AreEqual("Averaging", equation.Variables[1]);
            Assert.AreEqual("Initial", equation.Variables[2]);
        }

        [TestMethod]
        public void AbsTest()
        {
            var equation = EquationFactory.CreateInstance("Abs(variable)", out _);
            Assert.IsInstanceOfType(equation, typeof(ComplexEquation));
            Assert.AreEqual("Abs0", equation.InitializedEquation);
            Assert.AreEqual(1, equation.Variables.Count);
            Assert.AreEqual("Variable", equation.Variables[0]);
        }

        [TestMethod]
        public void MinTest()
        {
            var equation = EquationFactory.CreateInstance("Min(variable1, variable2)", out _);
            Assert.IsInstanceOfType(equation, typeof(ComplexEquation));
            Assert.AreEqual("Min0", equation.InitializedEquation);
            Assert.AreEqual(2, equation.Variables.Count);
            Assert.AreEqual("Variable1", equation.Variables[0]);
            Assert.AreEqual("Variable2", equation.Variables[1]);
        }

        [TestMethod]
        public void MaxTest()
        {
            var equation = EquationFactory.CreateInstance("Max(variable1, variable2)", out _);
            Assert.IsInstanceOfType(equation, typeof(ComplexEquation));
            Assert.AreEqual("Max0", equation.InitializedEquation);
            Assert.AreEqual(2, equation.Variables.Count);
            Assert.AreEqual("Variable1", equation.Variables[0]);
            Assert.AreEqual("Variable2", equation.Variables[1]);
        }

        [TestMethod]
        public void PowTest()
        {
            var equation = EquationFactory.CreateInstance("Pow(variable1, variable2)", out _);
            Assert.IsInstanceOfType(equation, typeof(ComplexEquation));
            Assert.AreEqual("Pow0", equation.InitializedEquation);
            Assert.AreEqual(2, equation.Variables.Count);
            Assert.AreEqual("Variable1", equation.Variables[0]);
            Assert.AreEqual("Variable2", equation.Variables[1]);
        }
    }
}