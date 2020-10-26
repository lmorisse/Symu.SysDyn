﻿#region Licence

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
using NCalc2;
using Symu.SysDyn.Functions;
using Symu.SysDyn.Model;
using Symu.SysDyn.Parser;
using Symu.SysDyn.Simulation;

namespace Symu.SysDyn.Equations
{
    public static class EquationFactory
    {
        public static IEquation CreateInstance(string eqn, out float value)
        {
            return CreateInstance(eqn, null, out value);
        }

        public static IEquation CreateInstance(string eqn, Range range, out float value)
        {
            value = 0;
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
            if (float.TryParse(eqn, NumberStyles.Any, CultureInfo.InvariantCulture, out var floatEqn))
            //if (float.TryParse(eqn, out var floatEqn))
            {
                value = floatEqn;
                return null;
            }

            try
            {
                // Test literal such as "1/10"
                var expression =new Expression(eqn);
                var eval = expression.Evaluate();
                value = Convert.ToSingle(eval);
                return null;
            }
            catch
            {
                // not an constant
            }
           Initialize(eqn, out var functions, out var variables, out var words);
            float sumEval=0;

            for (var i = 0; i < words.Count; i++)
            {
                var word = words[i];
                if (word.Length <= 1)
                {
                    continue;
                }

                float eval = 0;
                var function = functions.Find(x => x.IndexName == word);
                if (function != null)
                {
                    var success = function.TryEvaluate(null, null, out eval);
                    if (!success)
                    {
                        continue;
                    }

                    functions.Remove(function);
                }
                else
                {
                    var expression = new Expression(word);
                    try
                    {
                        // Test literal such as ".01"
                        eval = Convert.ToSingle(expression.Evaluate());
                    }
                    catch
                    {
                        continue;
                    }
                }

                // Variable can be replaced by a constant
                words[i] = eval.ToString(CultureInfo.InvariantCulture);
                sumEval += eval;
            }
            var initializedEquation = string.Join(string.Empty, words);

            // Equation with functions or only variables with brackets
            if (functions.Any() || words.Contains("("))
            {
                var complexEquation = new ComplexEquation(eqn, initializedEquation, functions, variables, words, range);
                try
                {
                    value = complexEquation.InitialValue();
                    return null;
                }
                catch
                {
                    return complexEquation;
                }
            }
            // Only variables without brackets
            if (variables.Any())
            {
                return new SimpleEquation(eqn, initializedEquation, variables, words, range);
            }
            // Only constants
            value = sumEval;
            return null;
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
            var regexWords = new Regex(@"[a-zA-Z0-9_]*\.?\,?[a-zA-Z0-9_]+|[-^+*\/()<>=]|\w+");//@"[0-9]*\.?\,?[0-9]+|[-^+*\/()<>=]|\w+");
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
            return string.Join(string.Empty, words);
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
                foreach (var equation in function.Parameters.Where(x => x != null))
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