#region Licence

// Description: SymuBiz - SymuSysDynTests
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
            var equation = EquationFactory.CreateInstance(string.Empty, "TIME", out _);
            Assert.IsInstanceOfType(equation, typeof(ComplexEquation));
            Assert.AreEqual("Time0", equation.InitializedEquation);
            Assert.AreEqual(0, equation.Variables.Count);
        }

        [TestMethod]
        public void DtTest()
        {
            var equation = EquationFactory.CreateInstance(string.Empty, "DT", out _);
            Assert.IsInstanceOfType(equation, typeof(ComplexEquation));
            Assert.AreEqual("Dt0", equation.InitializedEquation);
            Assert.AreEqual(0, equation.Variables.Count);
        }

        [TestMethod]
        public void StepTest()
        {
            var equation = EquationFactory.CreateInstance(string.Empty, "STEP(Height,StartTime)", out _);
            Assert.IsInstanceOfType(equation, typeof(ComplexEquation));
            Assert.AreEqual("Step0", equation.InitializedEquation);
            Assert.AreEqual(2, equation.Variables.Count);
            Assert.AreEqual("_Height", equation.Variables[0]);
            Assert.AreEqual("_Starttime", equation.Variables[1]);
        }

        [TestMethod]
        public void PulseTest()
        {
            var equation = EquationFactory.CreateInstance(string.Empty, "PULSE(Magnitude,FirstTime, Interval)", out _);
            Assert.IsInstanceOfType(equation, typeof(ComplexEquation));
            Assert.AreEqual("Pulse0", equation.InitializedEquation);
            Assert.AreEqual(3, equation.Variables.Count);
            Assert.AreEqual("_Magnitude", equation.Variables[0]);
            Assert.AreEqual("_Firsttime", equation.Variables[1]);
            Assert.AreEqual("_Interval", equation.Variables[2]);
        }

        [TestMethod]
        public void RampTest()
        {
            //Time is a reserved word
            var equation = EquationFactory.CreateInstance(string.Empty, "RAMP(StartTime,Slope)", out _);
            Assert.IsInstanceOfType(equation, typeof(ComplexEquation));
            Assert.AreEqual("Ramp0", equation.InitializedEquation);
            Assert.AreEqual(2, equation.Variables.Count);
            Assert.AreEqual("_Starttime", equation.Variables[0]);
            Assert.AreEqual("_Slope", equation.Variables[1]);
        }

        [TestMethod]
        public void NormalTest()
        {
            var equation = EquationFactory.CreateInstance(string.Empty, "Normal(Mean,StandardDeviation)", out _);
            Assert.IsInstanceOfType(equation, typeof(ComplexEquation));
            Assert.AreEqual("Normal0", equation.InitializedEquation);
            Assert.AreEqual(2, equation.Variables.Count);
            Assert.AreEqual("_Mean", equation.Variables[0]);
            Assert.AreEqual("_Standarddeviation", equation.Variables[1]);
        }

        [TestMethod]
        public void NormalTest1()
        {
            var equation = EquationFactory.CreateInstance(string.Empty, "Normal(Mean,StandardDeviation,Seed)", out _);
            Assert.IsInstanceOfType(equation, typeof(ComplexEquation));
            Assert.AreEqual("Normal0", equation.InitializedEquation);
            Assert.AreEqual(3, equation.Variables.Count);
            Assert.AreEqual("_Mean", equation.Variables[0]);
            Assert.AreEqual("_Standarddeviation", equation.Variables[1]);
            Assert.AreEqual("_Seed", equation.Variables[2]);
        }

        [TestMethod]
        public void IfThenElseTest()
        {
            var equation =
                EquationFactory.CreateInstance(string.Empty, "If condition then expression1 else expression2", out _);
            Assert.IsInstanceOfType(equation, typeof(ComplexEquation));
            Assert.AreEqual("If0", equation.InitializedEquation);
            Assert.AreEqual(3, equation.Variables.Count);
            Assert.AreEqual("_Condition", equation.Variables[0]);
            Assert.AreEqual("_Expression1", equation.Variables[1]);
            Assert.AreEqual("_Expression2", equation.Variables[2]);
        }

        [TestMethod]
        public void Smth1Test()
        {
            var equation = EquationFactory.CreateInstance(string.Empty, "SMTH1(Input, Averaging)", out _);
            Assert.IsInstanceOfType(equation, typeof(ComplexEquation));
            Assert.AreEqual("Smth10", equation.InitializedEquation);
            Assert.AreEqual(2, equation.Variables.Count);
            Assert.AreEqual("_Input", equation.Variables[0]);
            Assert.AreEqual("_Averaging", equation.Variables[1]);
        }

        [TestMethod]
        public void Smth1Test1()
        {
            var equation = EquationFactory.CreateInstance(string.Empty, "SMTH1(Input, Averaging, Initial)", out _);
            Assert.IsInstanceOfType(equation, typeof(ComplexEquation));
            Assert.AreEqual("Smth10", equation.InitializedEquation);
            Assert.AreEqual(3, equation.Variables.Count);
            Assert.AreEqual("_Input", equation.Variables[0]);
            Assert.AreEqual("_Averaging", equation.Variables[1]);
            Assert.AreEqual("_Initial", equation.Variables[2]);
        }

        [TestMethod]
        public void Smth3Test()
        {
            var equation = EquationFactory.CreateInstance(string.Empty, "SMTH3(Input, Averaging)", out _);
            Assert.IsInstanceOfType(equation, typeof(ComplexEquation));
            Assert.AreEqual("Smth30", equation.InitializedEquation);
            Assert.AreEqual(2, equation.Variables.Count);
            Assert.AreEqual("_Input", equation.Variables[0]);
            Assert.AreEqual("_Averaging", equation.Variables[1]);
        }

        [TestMethod]
        public void Smth3Test1()
        {
            var equation = EquationFactory.CreateInstance(string.Empty, "SMTH3(Input, Averaging, Initial)", out _);
            Assert.IsInstanceOfType(equation, typeof(ComplexEquation));
            Assert.AreEqual("Smth30", equation.InitializedEquation);
            Assert.AreEqual(3, equation.Variables.Count);
            Assert.AreEqual("_Input", equation.Variables[0]);
            Assert.AreEqual("_Averaging", equation.Variables[1]);
            Assert.AreEqual("_Initial", equation.Variables[2]);
        }

        [TestMethod]
        public void SmthNTest()
        {
            var equation = EquationFactory.CreateInstance(string.Empty, "SMTHN(Input, Averaging, 2)", out _);
            Assert.IsInstanceOfType(equation, typeof(ComplexEquation));
            Assert.AreEqual("Smthn0", equation.InitializedEquation);
            Assert.AreEqual(2, equation.Variables.Count);
            Assert.AreEqual("_Input", equation.Variables[0]);
            Assert.AreEqual("_Averaging", equation.Variables[1]);
        }

        [TestMethod]
        public void SmthNTest1()
        {
            var equation = EquationFactory.CreateInstance(string.Empty, "SMTHN(Input, Averaging, 3, Initial)", out _);
            Assert.IsInstanceOfType(equation, typeof(ComplexEquation));
            Assert.AreEqual("Smthn0", equation.InitializedEquation);
            Assert.AreEqual(3, equation.Variables.Count);
            Assert.AreEqual("_Input", equation.Variables[0]);
            Assert.AreEqual("_Averaging", equation.Variables[1]);
            Assert.AreEqual("_Initial", equation.Variables[2]);
        }

        [TestMethod]
        public void AbsTest()
        {
            var equation = EquationFactory.CreateInstance(string.Empty, "Abs(variable)", out _);
            Assert.IsInstanceOfType(equation, typeof(ComplexEquation));
            Assert.AreEqual("Abs0", equation.InitializedEquation);
            Assert.AreEqual(1, equation.Variables.Count);
            Assert.AreEqual("_Variable", equation.Variables[0]);
        }

        [TestMethod]
        public void MinTest()
        {
            var equation = EquationFactory.CreateInstance(string.Empty, "Min(variable1, variable2)", out _);
            Assert.IsInstanceOfType(equation, typeof(ComplexEquation));
            Assert.AreEqual("Min0", equation.InitializedEquation);
            Assert.AreEqual(2, equation.Variables.Count);
            Assert.AreEqual("_Variable1", equation.Variables[0]);
            Assert.AreEqual("_Variable2", equation.Variables[1]);
        }

        [TestMethod]
        public void MaxTest()
        {
            var equation = EquationFactory.CreateInstance(string.Empty, "Max(variable1, variable2)", out _);
            Assert.IsInstanceOfType(equation, typeof(ComplexEquation));
            Assert.AreEqual("Max0", equation.InitializedEquation);
            Assert.AreEqual(2, equation.Variables.Count);
            Assert.AreEqual("_Variable1", equation.Variables[0]);
            Assert.AreEqual("_Variable2", equation.Variables[1]);
        }

        [TestMethod]
        public void MaxTest1()
        {
            var equation = EquationFactory.CreateInstance(string.Empty, "Max (variable1, variable2)", out _);
            Assert.IsInstanceOfType(equation, typeof(ComplexEquation));
            Assert.AreEqual("Max0", equation.InitializedEquation);
            Assert.AreEqual(2, equation.Variables.Count);
            Assert.AreEqual("_Variable1", equation.Variables[0]);
            Assert.AreEqual("_Variable2", equation.Variables[1]);
        }

        [TestMethod]
        public void PowTest()
        {
            var equation = EquationFactory.CreateInstance(string.Empty, "Pow(variable1, variable2)", out _);
            Assert.IsInstanceOfType(equation, typeof(ComplexEquation));
            Assert.AreEqual("Pow0", equation.InitializedEquation);
            Assert.AreEqual(2, equation.Variables.Count);
            Assert.AreEqual("_Variable1", equation.Variables[0]);
            Assert.AreEqual("_Variable2", equation.Variables[1]);
        }
    }
}