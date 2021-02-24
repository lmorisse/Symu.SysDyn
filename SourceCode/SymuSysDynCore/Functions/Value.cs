#region Licence

// Description: SymuBiz - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Threading.Tasks;
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

        public static async Task<Value> CreateValue(string model, string function) 
        {
            return await CreateBuiltInFunction<Value>(model, function) ;
        }

        public string VariableId => GetParam(0);

        public override async Task<IBuiltInFunction> Clone()
        {
            var clone = await CreateValue(Model, OriginalFunction);
            CopyTo(clone);
            return clone;
        }

        public override async Task<float> Evaluate(IVariable selfVariable, VariableCollection variables, SimSpecs sim)
        {
            if (sim == null)
            {
                throw new ArgumentNullException(nameof(sim));
            }

            _value ??= await GetValue(0, selfVariable, variables, sim);

            return _value.Value;
        }

        public override async Task<TryReplaceStruct> TryReplace(SimSpecs sim)
        {
            return new TryReplaceStruct(false,0);
        }
    }
}