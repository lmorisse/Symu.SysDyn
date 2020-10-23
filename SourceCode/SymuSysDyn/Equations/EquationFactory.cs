#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Symu.SysDyn.Functions;
using Symu.SysDyn.Model;
using Symu.SysDyn.Parser;

namespace Symu.SysDyn.Equations
{
    public static class EquationFactory
    {
        public static IEquation CreateInstance(string eqn)
        {
            return CreateInstance(eqn, null);
        }

        public static IEquation CreateInstance(string eqn, Range range)
        {
            if (string.IsNullOrEmpty(eqn))
            {
                return null;
            }

            // Remove string in Braces which are units, not equation
            var index = eqn.IndexOf('{');
            if (index > 0)
            {
                eqn = eqn.Remove(index);
            }
            eqn = eqn.Trim();

            if (float.TryParse(eqn, out var floatEqn))
            {
                return new ConstantEquation(floatEqn);
            }

            var initializedEquation = Initialize(eqn, out var functions, out var variables, out var words);
            float sumEval=0;
            foreach (var function in functions.ToImmutableList())
            {
                var success = function.TryEvaluate(null, null, out var eval);
                if (!success)
                {
                    continue;
                }

                // Function can be replaced by a constant
                initializedEquation = initializedEquation.Replace(function.IndexName,
                    eval.ToString(CultureInfo.InvariantCulture));
                functions.Remove(function);
                sumEval += eval;
            }

            if (functions.Any() )
            {
                return new ComplexEquation(eqn, initializedEquation, functions, variables, range);
            }
            if (variables.Any())
            {
                return new SimpleEquation(eqn, initializedEquation, variables, words, range);
            }
            return new ConstantEquation(sumEval);
        }

        /// <summary>
        ///     Clean equation to be able to be computed
        ///     Done once at the creation of the variable
        /// </summary>
        /// <param name="originalEquation"></param>
        /// <param name="functions"></param>
        /// <param name="variables"></param>
        /// <param name="words"></param>
        /// <returns>Initialized equation</returns>
        public static string Initialize(string originalEquation, out List<BuiltInFunction> functions, out List<string> variables, out List<string> words)
        {
            if (originalEquation == null)
            {
                throw new ArgumentNullException(nameof(originalEquation));
            }

            functions = FunctionUtils.ParseFunctions(originalEquation).ToList();
            for (var i = 0; i < functions.Count; i++)
            {
                var function = functions[i];
                function.IndexName = function.Name + i;
                function.Name = StringUtils.CleanName(function.Name);
                originalEquation = originalEquation.Replace(function.OriginalFunction, function.IndexName);
            }
            // split equation in words
            var regexWords = new Regex(@"[0-9]*\.?\,?[0-9]+|[-^+*\/()<>=]|\w+");
            var matches = regexWords.Matches(originalEquation);
            words = new List<string>();
            variables = new List<string>();
            foreach (var match in matches)
            {
                var word = StringUtils.CleanName(match.ToString());
                words.Add(word);
                variables.AddRange(SetVariables(word, functions));
            }

            variables = variables.Distinct().ToList();
            return words.Aggregate(string.Empty, (current, word) => current + word);
        }

        private static IEnumerable<string> SetVariables(string word, List<BuiltInFunction> functions)
        {
            var variables = new List<string>();
            if (word.Length <= 1)
            {
                return variables;
            }

            if (float.TryParse(word, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
            {
                return variables;
            }

            var function = functions.Find(x => word == x.IndexName);
            if (function != null)
            {
                //Get the variables of the function
                foreach (var equation in function.Parameters)
                {
                    variables.AddRange(equation.Variables);
                }
            }
            else
            {
                variables.Add(word);
            }

            return variables;
        }
    }
}