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
            Mean = Parameters[0].Variables.Any() ? Parameters[0].Variables.First() : Parameters[0].OriginalEquation.Trim();
            StandardDeviation = Parameters[1].Variables.Any() ? Parameters[1].Variables.First() : Parameters[1].OriginalEquation.Trim();

            if (Parameters.Count == 3)
            {
                Seed = Parameters[2].Variables.Any()
                    ? Parameters[2].Variables.First()
                    : Parameters[2].OriginalEquation.Trim();
            }
            else
            {
                Seed = string.Empty;
            }
        }

        public string Mean { get; }
        public string StandardDeviation { get; }
        public string Seed { get; }

        public override float Evaluate(Variables variables, SimSpecs sim)
        {
            if (sim == null)
            {
                throw new ArgumentNullException(nameof(sim));
            }
            //mean can be either a literal or a numeric
            var mean = Parameters[0].Variables.Any() ? Convert.ToSingle(Expression.Parameters[Mean]) : Convert.ToSingle(Mean, CultureInfo.InvariantCulture);

            var standardDeviation = Parameters[1].Variables.Any() ? Convert.ToSingle(Expression.Parameters[StandardDeviation]) : Convert.ToSingle(StandardDeviation, CultureInfo.InvariantCulture);

            if (string.IsNullOrEmpty(Seed))
            {
                return Common.Math.ProbabilityDistributions.Normal.Sample(mean, standardDeviation);
            }
            // with seed parameter
            var seed = Parameters[2].Variables.Any() ? Convert.ToInt32(Expression.Parameters[Seed]) : Convert.ToInt32(Seed, CultureInfo.InvariantCulture);

            return Common.Math.ProbabilityDistributions.Normal.Sample(mean, standardDeviation, seed);
        }
    }
}