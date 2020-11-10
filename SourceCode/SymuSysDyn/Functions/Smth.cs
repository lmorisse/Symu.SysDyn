#region Licence

// Description: SymuBiz - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Globalization;
using System.Linq;
using Symu.SysDyn.Engine;
using Symu.SysDyn.Models;
using Symu.SysDyn.Models.XMile;

#endregion

namespace Symu.SysDyn.Functions
{
    /// <summary>
    ///     The smth1, smth3 and smthn functions perform a first-, third- and nth-order respectively exponential smooth of
    ///     input, using an exponential averaging time of averaging,
    ///     and an optional initial value initial for the smooth.smth3 does this by setting up a cascade of three first-order
    ///     exponential smooths, each with an averaging time of averaging/3.
    ///     The other functions behave analogously.They return the value of the final smooth in the cascade.
    ///     If you do not specify an initial value initial, they assume the value to be the initial value of input.
    /// </summary>
    public abstract class Smth : BuiltInFunction
    {
        private bool _initialized;

        private byte _order;
        private float[] _previousValues;

        protected Smth(string model, string function) : base(model, function)
        {
        }

        protected Smth(string model, string function, byte order) : base(model, function)
        {
            Order = order;
            InitialIndex = Parameters.Count == 3 ? 2 : 0;
        }

        protected int InitialIndex { get; set; }
        public string Input => GetParamFromOriginalEquation(0);
        public string Averaging => GetParamFromOriginalEquation(1);
        public string Initial => GetParamFromOriginalEquation(InitialIndex);

        /// <summary>
        ///     Nth order smooth
        /// </summary>
        public byte Order
        {
            get => _order;
            protected set
            {
                _order = value;
                _previousValues = new float[value];
            }
        }

        protected string GetParamFromOriginalEquation(int index)
        {
            return Parameters[index] != null
                ? Parameters[index].OriginalEquation
                : Args[index].ToString(CultureInfo.InvariantCulture);
        }

        public override float Evaluate(IVariable selfVariable, VariableCollection variables, SimSpecs sim)
        {
            if (sim == null)
            {
                throw new ArgumentNullException(nameof(sim));
            }

            if (!_initialized)
            {
                _previousValues[0] = GetValue(InitialIndex, selfVariable, variables, sim);

                for (var i = 1; i < Order; i++)
                {
                    _previousValues[i] = _previousValues[0];
                }

                _initialized = true;
                return _previousValues.Last();
            }

            var input = GetValue(0, selfVariable, variables, sim);
            var averaging = GetValue(1, selfVariable, variables, sim);

            for (var i = 0; i < Order; i++)
            {
                _previousValues[i] += sim.DeltaTime * (input - _previousValues[i]) * Order / averaging;
                input = _previousValues[i];
            }

            return _previousValues.Last();
        }
    }
}