#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Text.RegularExpressions;
using Symu.SysDyn.Model;
using Symu.SysDyn.Simulation;

#endregion

namespace Symu.SysDyn.Functions
{
    /// <summary>
    ///     ExternalUpdate built in function
    ///     Used when a variable is updated at each step via an external device
    ///     The variable has no equation so it could be removed by the optimizer
    ///     Using this function will avoid this pitfall
    /// </summary>
    /// <example>new Variable("example", "ExternalUpdate")</example>
    public class ExternalUpdate : BuiltInFunction
    {
        public const string Value = "Externalupdate";

        public ExternalUpdate(string function) : base(function)
        {
        }
        public override IBuiltInFunction Clone()
        {
            var clone = new ExternalUpdate(OriginalFunction);
            CopyTo(clone);
            return clone;
        }

        public override float Evaluate(Variable variable, Variables variables, SimSpecs sim)
        {
            if (variable == null)
            {
                throw new ArgumentNullException(nameof(variable));
            }

            return variable.Value;
        }
    }
}