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
    ///     ExternalUpdate built in function
    ///     Used when a variable is updated at each step via an external device
    ///     The variable has no equation so it could be removed by the optimizer
    ///     Using this function will avoid this pitfall
    ///     Parameter : InitialValue (Optional)
    /// </summary>
    /// <example>new Variable("example", "ExternalUpdate(1)")</example>
    public class ExternalUpdate : BuiltInFunction
    {
        public const string Value = "Externalupdate";

        public ExternalUpdate(string model, string function) : base(model, function)
        {
        }

        public override IBuiltInFunction Clone()
        {
            var clone = new ExternalUpdate(Model, OriginalFunction);
            CopyTo(clone);
            return clone;
        }

        public override float Evaluate(IVariable variable, VariableCollection variables, SimSpecs sim)
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
        public override bool TryReplace(SimSpecs sim, out float result)
        {
            result = 0;
            return false;
            //return Args.Count == 1 ? Args[0] : 0;
        }
    }
}