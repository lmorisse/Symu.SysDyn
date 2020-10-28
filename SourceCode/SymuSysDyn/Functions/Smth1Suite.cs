#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Globalization;
using NCalc2;
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
    public class Smth1Suite : BuiltInFunction
    {
        public const string Value = "Smth1suite";

        private float _previousValue;

        private bool _initialized;
        public Smth1Suite(string function) : base(function)
        {
            Order = 1;
        }
        protected string GetParamFromOriginalEquation(int index)
        {
            return Parameters[index] != null ? Parameters[index].OriginalEquation : Args[index].ToString(CultureInfo.InvariantCulture);
        }
        public string Input => GetParamFromOriginalEquation(0);
        public string Averaging => GetParamFromOriginalEquation(1);
        public string Initial => Parameters.Count == 3 ? GetParamFromOriginalEquation(2) : Input;
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

            if (!_initialized)
            {
                _previousValue = Parameters.Count == 3 ? GetValue(2, variables, sim) : GetValue(0, variables, sim);
                _initialized = true;
                return _previousValue;
            }

            var input = GetValue(0, variables, sim);
            var averaging = GetValue(1, variables, sim);

            _previousValue += sim.DeltaTime * (input - _previousValue) / averaging;
            return _previousValue;
        }
    }
}