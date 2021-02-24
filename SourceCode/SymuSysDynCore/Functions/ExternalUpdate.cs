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
    ///     ExternalUpdate built in function
    ///     Used when a variable is updated at each step via an external device
    ///     The variable has no equation so it could be removed by the optimizer
    ///     Using this function will avoid this pitfall
    ///     Parameter : InitialValue (Optional)
    /// </summary>
    /// <example>new Variable("example", "ExternalUpdate(1)")</example>
    public class ExternalUpdate : BuiltInFunction
    {
        public const string Label = "Externalupdate";

        public static async Task<ExternalUpdate> CreateExternalUpdate(string model, string function) 
        {
            return await CreateBuiltInFunction<ExternalUpdate>(model, function) ;
        }

        public override async Task<IBuiltInFunction> Clone()
        {
            var clone = await CreateExternalUpdate(Model, OriginalFunction);
            CopyTo(clone);
            return clone;
        }

        public override async Task<float> Evaluate(IVariable variable, VariableCollection variables, SimSpecs sim)
        {
            if (variable == null)
            {
                throw new ArgumentNullException(nameof(variable));
            }
            if (sim == null)
            {
                throw new ArgumentNullException(nameof(sim));
            }

            if (sim.Step == 0)
            {
                return Args.Count == 1 ? Args[0] : variable.Value;
            }
            return variable.Value;
        }

        public override async Task<TryReplaceStruct> TryReplace(SimSpecs sim)
        {
            return new TryReplaceStruct(false, 0);
        }
    }
}