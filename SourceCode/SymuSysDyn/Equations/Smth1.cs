#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Globalization;
using System.Linq;

using Symu.SysDyn.Simulation;

#endregion

namespace Symu.SysDyn.Equations
{
    /// <summary>
    /// The smth1, smth3 and smthn functions perform a first-, third- and nth-order respectively exponential smooth of input, using an exponential averaging time of averaging,
    /// and an optional initial value initial for the smooth.smth3 does this by setting up a cascade of three first-order exponential smooths, each with an averaging time of averaging/3.
    /// The other functions behave analogously.They return the value of the final smooth in the cascade.
    /// If you do not specify an initial value initial, they assume the value to be the initial value of input.
    /// </summary>
    public class Smth1 : BuiltInFunction
    {
        protected SmthMachine SmthMachine { get; set; }
        
        public Smth1(string function) : base(function)
        {
            Input = Parameters[0].OriginalEquation;
            Averaging = Parameters[1].OriginalEquation;
            if (Parameters.Count == 3)
            {
                Initial = Parameters[2].OriginalEquation;
            }

            SmthMachine = new SmthMachine(Input, Averaging, 1, Initial);
        }
        public string Input { get; protected set; }
        public string Averaging { get; protected set; }
        public string Initial { get; protected set; } = "0";

        public override float Evaluate(SimSpecs sim)
        {
            if (sim == null)
            {
                throw new ArgumentNullException(nameof(sim));
            }

            return SmthMachine.Evaluate(sim.Time);
        }
    }
}