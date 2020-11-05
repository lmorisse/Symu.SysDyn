#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NCalc2;
using Symu.SysDyn.Engine;
using Symu.SysDyn.Functions;
using Symu.SysDyn.Models;

#endregion

namespace Symu.SysDyn.Equations
{
    /// <summary>
    ///     IEquation with parameters and functions
    /// </summary>
    public class ComplexEquation : SimpleEquation
    {
        public ComplexEquation(string originalEquation, string initializedEquation, IEnumerable<IBuiltInFunction> functions,
            List<string> variables, List<string> words, Range range) :
            this(originalEquation, initializedEquation, functions, variables, words)
        {
            Range = range;
        }

        public ComplexEquation(string originalEquation, string initializedEquation,
            IEnumerable<IBuiltInFunction> functions, List<string> variables, List<string> words)
            : base(originalEquation, initializedEquation, variables, words)
        {
            Functions = new BuiltInFunctionCollection(functions);
        }

        /// <summary>
        ///     ComplexEquation inherits from SimpleEquation and adds a list of nested functions
        /// </summary>
        public BuiltInFunctionCollection Functions { get; }

        public override IEquation Clone()
        {
            return new ComplexEquation(OriginalEquation, InitializedEquation, Functions.Clone(), Variables.ToList(), Words.ToList(),
                Range);
        }

        public override bool CanBeOptimized(string variableName)
        {
            return !Functions.Any() && base.CanBeOptimized(variableName);
        }

        /// <summary>
        ///     Prepare equation to be computed
        ///     Replace all variables of the equation by its actual value
        /// </summary>
        /// <param name="selfVariable"></param>
        /// <param name="variables"></param>
        /// <param name="sim"></param>
        /// <returns></returns>
        public override void Prepare(IVariable selfVariable, VariableCollection variables, SimSpecs sim)
        {
            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }

            base.Prepare(selfVariable, variables, sim);

            // Built-in functions
            foreach (var function in Functions)
            {
                Expression.Parameters[function.IndexName] = function.Prepare(selfVariable, variables, sim);
            }
        }

        public override void Replace(string child, string value, SimSpecs sim)
        {
            //Replace functions
            foreach (var function in Functions.ToImmutableList())
            {
                function.Replace(child, value, sim);

                if (function.Parameters.Any(x => x != null))
                {
                    continue;
                }

                try
                {
                    while (Words.FindIndex(ind => ind.Equals(function.IndexName)) >= 0)
                    {
                        Words[Words.FindIndex(ind => ind.Equals(function.IndexName))] =
                            function.InitialValue(sim).ToString(CultureInfo.InvariantCulture);
                    }

                    Functions.Remove(function);
                }
                catch
                {
                }
            }

            while (Words.FindIndex(ind => ind.Equals(child)) >= 0)
            {
                Words[Words.FindIndex(ind => ind.Equals(child))] = value;
            }

            InitializedEquation = string.Join(string.Empty, Words);
            Variables.Remove(child);
            Expression = new Expression(InitializedEquation);
        }
    }
}