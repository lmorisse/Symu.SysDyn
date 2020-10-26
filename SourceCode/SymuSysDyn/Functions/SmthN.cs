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
    public class SmthN : Smth1
    {
        public new const string Value = "SmthN";
        public SmthN(string function) : base(function)
        {
            Order = Convert.ToByte(GetParamFromOriginalEquation(2));
        }
        public new string Initial => Parameters.Count == 4 ? GetParamFromOriginalEquation(3) : Input;
        public override float Evaluate(Variables variables, SimSpecs sim)
        {
            if (sim == null)
            {
                throw new ArgumentNullException(nameof(sim));
            }

            SmthMachine = new SmthMachine(Input, Averaging, Order, Initial); // to have the correct Initial, we can't call Evaluate SMTH1
            SmthMachine.AddVariables(variables);
            return SmthMachine.Evaluate(sim.Time);
        }
    }
}