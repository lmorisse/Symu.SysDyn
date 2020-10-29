#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using NCalc2;
using Symu.SysDyn.Functions;
using Symu.SysDyn.Model;
using Symu.SysDyn.Simulation;

#endregion

namespace Symu.SysDyn.Equations
{
    /// <summary>
    /// IEquation with parameters and functions
    /// </summary>
    public class ComplexEquation : SimpleEquation
    {
        /// <summary>
        ///     List of all the nested functions used in the equation
        /// </summary>
        public List<BuiltInFunction> Functions { get; }

        public ComplexEquation(string originalEquation, string initializedEquation, List<BuiltInFunction> functions, List<string> variables, List<string> words, Range range) : 
            this(originalEquation, initializedEquation, functions, variables, words)
        {
            _range = range;
        }

        public ComplexEquation(string originalEquation, string initializedEquation, List<BuiltInFunction> functions, List<string> variables, List<string> words)
            : base(originalEquation, initializedEquation, variables, words)
        {
            Functions = functions;
        }

        public override IEquation Clone()
        {
            return new ComplexEquation(OriginalEquation, InitializedEquation, Functions, Variables, _words, _range);
        }

        public override bool CanBeOptimized(string variableName)
        {
            return !Functions.Any() && base.CanBeOptimized(variableName);
        }
        /// <summary>
        ///     Prepare equation to be computed
        ///     Replace all variables of the equation by its actual value
        /// </summary>
        /// <param name="variables"></param>
        /// <param name="sim"></param>
        /// <returns></returns>
        public override void Prepare(Variables variables, SimSpecs sim)
        {
            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }

            base.Prepare(variables, sim);

            // Built-in functions
            foreach (var function in Functions)
            {
                _expression.Parameters[function.IndexName] = function.Prepare(variables, sim);
            }
        }


        public override void Replace(string child, string value)
        {
            //Replace functions
            foreach (var function in Functions.ToImmutableList())
            {
                function.Replace(child, value);

                if (function.Parameters.Any(x => x != null))
                {
                    continue;
                }

                try
                {
                    while (_words.FindIndex(ind => ind.Equals(function.IndexName)) >= 0)
                    {
                        _words[_words.FindIndex(ind => ind.Equals(function.IndexName))] = function.InitialValue().ToString(CultureInfo.InvariantCulture);
                    }
                    Functions.Remove(function);
                }
                catch 
                {
                }

            }
            
            while (_words.FindIndex(ind => ind.Equals(child)) >=0)
            {
                _words[_words.FindIndex(ind => ind.Equals(child))] = value;
            }

            InitializedEquation = string.Join(string.Empty, _words);
            Variables.Remove(child);
            _expression = new Expression(InitializedEquation);
        }
    }
}