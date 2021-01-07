#region Licence

// Description: SymuBiz - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using Symu.SysDyn.Core.Engine;
using Symu.SysDyn.Core.Models.XMile;

#endregion

namespace Symu.SysDyn.Core.Functions
{
    /// <summary>
    ///     Value built in function
    ///     Set the value of a variable when the function is called. This value won't evolve even if the variable value does.
    ///     Used with functions that are triggered by a starttime such as Step, Ramp, Pulse, ...
    ///     value(variableId)
    ///     Arguments: variableId
    /// </summary>
    /// <example>Value(Time) will have a return value of 1 if called at time 1 or after</example>
    public class Value : BuiltInFunction
    {
        public const string Label = "Value";
        private float? _value;

        public Value(string model, string function) : base(model, function)
        {
        }

        public string VariableId => GetParam(0);

        public override IBuiltInFunction Clone()
        {
            var clone = new Value(Model, OriginalFunction);
            CopyTo(clone);
            return clone;
        }

        public override float Evaluate(IVariable selfVariable, VariableCollection variables, SimSpecs sim)
        {
            if (sim == null)
            {
                throw new ArgumentNullException(nameof(sim));
            }

            _value ??= GetValue(0, selfVariable, variables, sim);

            return _value.Value;
        }

        public override bool TryReplace(SimSpecs sim, out float result)
        {
            result = 0;
            return false;
        }
    }
}