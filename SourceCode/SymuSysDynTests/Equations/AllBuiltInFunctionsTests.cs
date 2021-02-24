#region Licence

// Description: SymuBiz - SymuSysDynTests
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Symu.SysDyn.Core.Equations;

#endregion

namespace SymuSysDynTests.Equations
{
    [TestClass]
    public class AllBuiltInFunctionsTests
    {
        [TestMethod]
        public async Task FactoryTest()
        {
            var factory = await EquationFactory.CreateInstance(string.Empty, "TIME");
            Assert.IsInstanceOfType(factory.Equation, typeof(ComplexEquation));
            Assert.AreEqual("Time0", factory.Equation.InitializedEquation);
            Assert.AreEqual(0, factory.Equation.Variables.Count);
        }

        [TestMethod]
        public async Task DtTest()
        {
            var factory = await EquationFactory.CreateInstance(string.Empty, "DT");
            Assert.IsInstanceOfType(factory.Equation, typeof(ComplexEquation));
            Assert.AreEqual("Dt0", factory.Equation.InitializedEquation);
            Assert.AreEqual(0, factory.Equation.Variables.Count);
        }

        [TestMethod]
        public async Task StepTest()
        {
            var factory = await EquationFactory.CreateInstance(string.Empty, "STEP(Height,StartTime)");
            Assert.IsInstanceOfType(factory.Equation, typeof(ComplexEquation));
            Assert.AreEqual("Step0", factory.Equation.InitializedEquation);
            Assert.AreEqual(2, factory.Equation.Variables.Count);
            Assert.AreEqual("_Height", factory.Equation.Variables[0]);
            Assert.AreEqual("_Starttime", factory.Equation.Variables[1]);
        }

        [TestMethod]
        public async Task PulseTest()
        {
            var factory = await EquationFactory.CreateInstance(string.Empty, "PULSE(Magnitude,FirstTime, Interval)");
            Assert.IsInstanceOfType(factory.Equation, typeof(ComplexEquation));
            Assert.AreEqual("Pulse0", factory.Equation.InitializedEquation);
            Assert.AreEqual(3, factory.Equation.Variables.Count);
            Assert.AreEqual("_Magnitude", factory.Equation.Variables[0]);
            Assert.AreEqual("_Firsttime", factory.Equation.Variables[1]);
            Assert.AreEqual("_Interval", factory.Equation.Variables[2]);
        }

        [TestMethod]
        public async Task RampTest()
        {
            //Time is a reserved word
            var factory = await EquationFactory.CreateInstance(string.Empty, "RAMP(StartTime,Slope)");
            Assert.IsInstanceOfType(factory.Equation, typeof(ComplexEquation));
            Assert.AreEqual("Ramp0", factory.Equation.InitializedEquation);
            Assert.AreEqual(2, factory.Equation.Variables.Count);
            Assert.AreEqual("_Starttime", factory.Equation.Variables[0]);
            Assert.AreEqual("_Slope", factory.Equation.Variables[1]);
        }

        [TestMethod]
        public async Task NormalTest()
        {
            var factory = await EquationFactory.CreateInstance(string.Empty, "Normal(Mean,StandardDeviation)");
            Assert.IsInstanceOfType(factory.Equation, typeof(ComplexEquation));
            Assert.AreEqual("Normal0", factory.Equation.InitializedEquation);
            Assert.AreEqual(2, factory.Equation.Variables.Count);
            Assert.AreEqual("_Mean", factory.Equation.Variables[0]);
            Assert.AreEqual("_Standarddeviation", factory.Equation.Variables[1]);
        }

        [TestMethod]
        public async Task NormalTest1()
        {
            var factory = await EquationFactory.CreateInstance(string.Empty, "Normal(Mean,StandardDeviation,Seed)");
            Assert.IsInstanceOfType(factory.Equation, typeof(ComplexEquation));
            Assert.AreEqual("Normal0", factory.Equation.InitializedEquation);
            Assert.AreEqual(3, factory.Equation.Variables.Count);
            Assert.AreEqual("_Mean", factory.Equation.Variables[0]);
            Assert.AreEqual("_Standarddeviation", factory.Equation.Variables[1]);
            Assert.AreEqual("_Seed", factory.Equation.Variables[2]);
        }

        [TestMethod]
        public async Task IfThenElseTest()
        {
            var factory =
                await EquationFactory.CreateInstance(string.Empty, "If condition then expression1 else expression2");
            Assert.IsInstanceOfType(factory.Equation, typeof(ComplexEquation));
            Assert.AreEqual("If0", factory.Equation.InitializedEquation);
            Assert.AreEqual(3, factory.Equation.Variables.Count);
            Assert.AreEqual("_Condition", factory.Equation.Variables[0]);
            Assert.AreEqual("_Expression1", factory.Equation.Variables[1]);
            Assert.AreEqual("_Expression2", factory.Equation.Variables[2]);
        }

        [TestMethod]
        public async Task Smth1Test()
        {
            var factory = await EquationFactory.CreateInstance(string.Empty, "SMTH1(Input, Averaging)");
            Assert.IsInstanceOfType(factory.Equation, typeof(ComplexEquation));
            Assert.AreEqual("Smth10", factory.Equation.InitializedEquation);
            Assert.AreEqual(2, factory.Equation.Variables.Count);
            Assert.AreEqual("_Input", factory.Equation.Variables[0]);
            Assert.AreEqual("_Averaging", factory.Equation.Variables[1]);
        }

        [TestMethod]
        public async Task Smth1Test1()
        {
            var factory = await EquationFactory.CreateInstance(string.Empty, "SMTH1(Input, Averaging, Initial)");
            Assert.IsInstanceOfType(factory.Equation, typeof(ComplexEquation));
            Assert.AreEqual("Smth10", factory.Equation.InitializedEquation);
            Assert.AreEqual(3, factory.Equation.Variables.Count);
            Assert.AreEqual("_Input", factory.Equation.Variables[0]);
            Assert.AreEqual("_Averaging", factory.Equation.Variables[1]);
            Assert.AreEqual("_Initial", factory.Equation.Variables[2]);
        }

        [TestMethod]
        public async Task Smth3Test()
        {
            var factory = await EquationFactory.CreateInstance(string.Empty, "SMTH3(Input, Averaging)");
            Assert.IsInstanceOfType(factory.Equation, typeof(ComplexEquation));
            Assert.AreEqual("Smth30", factory.Equation.InitializedEquation);
            Assert.AreEqual(2, factory.Equation.Variables.Count);
            Assert.AreEqual("_Input", factory.Equation.Variables[0]);
            Assert.AreEqual("_Averaging", factory.Equation.Variables[1]);
        }

        [TestMethod]
        public async Task Smth3Test1()
        {
            var factory = await EquationFactory.CreateInstance(string.Empty, "SMTH3(Input, Averaging, Initial)");
            Assert.IsInstanceOfType(factory.Equation, typeof(ComplexEquation));
            Assert.AreEqual("Smth30", factory.Equation.InitializedEquation);
            Assert.AreEqual(3, factory.Equation.Variables.Count);
            Assert.AreEqual("_Input", factory.Equation.Variables[0]);
            Assert.AreEqual("_Averaging", factory.Equation.Variables[1]);
            Assert.AreEqual("_Initial", factory.Equation.Variables[2]);
        }

        [TestMethod]
        public async Task SmthNTest()
        {
            var factory = await EquationFactory.CreateInstance(string.Empty, "SMTHN(Input, Averaging, 2)");
            Assert.IsInstanceOfType(factory.Equation, typeof(ComplexEquation));
            Assert.AreEqual("Smthn0", factory.Equation.InitializedEquation);
            Assert.AreEqual(2, factory.Equation.Variables.Count);
            Assert.AreEqual("_Input", factory.Equation.Variables[0]);
            Assert.AreEqual("_Averaging", factory.Equation.Variables[1]);
        }

        [TestMethod]
        public async Task SmthNTest1()
        {
            var factory = await EquationFactory.CreateInstance(string.Empty, "SMTHN(Input, Averaging, 3, Initial)");
            Assert.IsInstanceOfType(factory.Equation, typeof(ComplexEquation));
            Assert.AreEqual("Smthn0", factory.Equation.InitializedEquation);
            Assert.AreEqual(3, factory.Equation.Variables.Count);
            Assert.AreEqual("_Input", factory.Equation.Variables[0]);
            Assert.AreEqual("_Averaging", factory.Equation.Variables[1]);
            Assert.AreEqual("_Initial", factory.Equation.Variables[2]);
        }

        [TestMethod]
        public async Task AbsTest()
        {
            var factory = await EquationFactory.CreateInstance(string.Empty, "Abs(variable)");
            Assert.IsInstanceOfType(factory.Equation, typeof(ComplexEquation));
            Assert.AreEqual("Abs0", factory.Equation.InitializedEquation);
            Assert.AreEqual(1, factory.Equation.Variables.Count);
            Assert.AreEqual("_Variable", factory.Equation.Variables[0]);
        }

        [TestMethod]
        public async Task MinTest()
        {
            var factory = await EquationFactory.CreateInstance(string.Empty, "Min(variable1, variable2)");
            Assert.IsInstanceOfType(factory.Equation, typeof(ComplexEquation));
            Assert.AreEqual("Min0", factory.Equation.InitializedEquation);
            Assert.AreEqual(2, factory.Equation.Variables.Count);
            Assert.AreEqual("_Variable1", factory.Equation.Variables[0]);
            Assert.AreEqual("_Variable2", factory.Equation.Variables[1]);
        }

        [TestMethod]
        public async Task MaxTest()
        {
            var factory = await EquationFactory.CreateInstance(string.Empty, "Max(variable1, variable2)");
            Assert.IsInstanceOfType(factory.Equation, typeof(ComplexEquation));
            Assert.AreEqual("Max0", factory.Equation.InitializedEquation);
            Assert.AreEqual(2, factory.Equation.Variables.Count);
            Assert.AreEqual("_Variable1", factory.Equation.Variables[0]);
            Assert.AreEqual("_Variable2", factory.Equation.Variables[1]);
        }

        [TestMethod]
        public async Task PowTest()
        {
            var factory = await EquationFactory.CreateInstance(string.Empty, "Pow(variable1, variable2)");
            Assert.IsInstanceOfType(factory.Equation, typeof(ComplexEquation));
            Assert.AreEqual("Pow0", factory.Equation.InitializedEquation);
            Assert.AreEqual(2, factory.Equation.Variables.Count);
            Assert.AreEqual("_Variable1", factory.Equation.Variables[0]);
            Assert.AreEqual("_Variable2", factory.Equation.Variables[1]);
        }
    }
}