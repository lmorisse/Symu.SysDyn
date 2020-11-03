#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using Symu.SysDyn.Models;
using Symu.SysDyn.Simulation;

#endregion

namespace Symu.SysDyn.Functions
{
    /// <summary>
    ///     TIME built in function
    /// </summary>
    public class Time : BuiltInFunction
    {
        public const string Value = "Time";

        public Time(string model, string function) : base(model, function)
        {
        }

        public override IBuiltInFunction Clone()
        {
            var clone = new Time(Model, OriginalFunction);
            CopyTo(clone);
            return clone;
        }

        public override float Evaluate(IVariable selfVariable, VariableCollection variables, SimSpecs sim)
        {
            if (sim == null)
            {
                throw new ArgumentNullException(nameof(sim));
            }

            return sim.Time;
        }
    }
}