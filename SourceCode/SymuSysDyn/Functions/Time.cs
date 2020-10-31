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
    ///     TIME built in function
    /// </summary>
    public class Time : BuiltInFunction
    {
        public const string Value = "Time";

        public Time(string function) : base(function)
        {
        }

        public override float Evaluate(Variable selfVariable, Variables variables, SimSpecs sim)
        {
            if (sim == null)
            {
                throw new ArgumentNullException(nameof(sim));
            }

            return sim.Time;
        }
    }
}