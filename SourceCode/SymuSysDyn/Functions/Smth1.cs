#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Globalization;
using Symu.SysDyn.Equations;
using Symu.SysDyn.Model;
using Symu.SysDyn.Simulation;

#endregion

namespace Symu.SysDyn.Functions
{
    /// <summary>
    /// The smth1, smth3 and smthn functions perform a first-, third- and nth-order respectively exponential smooth of input, using an exponential averaging time of averaging,
    /// and an optional initial value initial for the smooth.smth3 does this by setting up a cascade of three first-order exponential smooths, each with an averaging time of averaging/3.
    /// The other functions behave analogously.They return the value of the final smooth in the cascade.
    /// If you do not specify an initial value initial, they assume the value to be the initial value of input.
    /// </summary>
    public class Smth1 : BuiltInFunction
    {
        public const string Value = "Smth1";
        protected SmthMachine SmthMachine { get; set; }

        public Smth1(string function) : base(function)
        {
            Input = GetParamFromOriginalEquation(0);
            Averaging = GetParamFromOriginalEquation(1);
            Initial = Parameters.Count == 3 ? GetParamFromOriginalEquation(2) : Input;
            Order = 1;
        }
        protected string GetParamFromOriginalEquation(int index)
        {
            return Parameters[index] != null ? Parameters[index].OriginalEquation : Args[index].ToString(CultureInfo.InvariantCulture);
        }

        public string Input { get; protected set; }
        public string Averaging { get; protected set; }
        public string Initial { get; protected set; }
        /// <summary>
        /// Nth order smooth
        /// </summary>
        public byte Order { get; protected set; }

        public override float Evaluate(Variables variables, SimSpecs sim)
        {
            if (sim == null)
            {
                throw new ArgumentNullException(nameof(sim));
            }

            SmthMachine = new SmthMachine(Input, Averaging, Order, Initial);
            SmthMachine.AddVariables(variables);
            return SmthMachine.Evaluate(sim.Time);
        }
    }
}