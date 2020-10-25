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
using Symu.SysDyn.Model;
using Symu.SysDyn.Simulation;

#endregion

namespace Symu.SysDyn.Functions
{
    /// <summary>
    ///     Normal built in function
    ///     Sample a value from a Normal distribution
    ///     Parameters: 2 or 3: (mean, standard deviation[, seed]); 0 ≤ seed < 232
    /// If seed is provided, the sequence of numbers will always be identical
    /// </summary>
    /// <example>NORMAL(100, 5) samples from N(100, 5)</example>
    public class Normal : BuiltInFunction
    {
        public const string Value = "Normal";

        public Normal(string function) : base(function)
        {
        }

        public string Mean => GetParam(0);

        public string StandardDeviation => GetParam(1);
        public string Seed => Parameters.Count == 3 ? GetParam(2) : string.Empty;

        public override float Evaluate(Variables variables, SimSpecs sim)
        {
            var mean = GetValue(0,variables, sim);

            var standardDeviation = GetValue(1, variables, sim);

            if (string.IsNullOrEmpty(Seed))
            {
                return Common.Math.ProbabilityDistributions.Normal.Sample(mean, standardDeviation);
            }
            // with seed parameter
            var seed = Convert.ToInt32(GetValue(2, variables, sim));

            return Common.Math.ProbabilityDistributions.Normal.Sample(mean, standardDeviation, seed);
        }

    }
}