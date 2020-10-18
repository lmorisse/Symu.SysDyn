#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NCalc2;
using Symu.SysDyn.Model;
using Symu.SysDyn.Parser;
using Symu.SysDyn.Simulation;

#endregion

namespace Symu.SysDyn.Equations
{
    public class ManagedEquation
    {
        private readonly string _equation;

        /// <summary>
        ///     Range of the output of the equation provide by the variable
        /// </summary>
        private readonly Range _range;

        /// <summary>
        ///     List of all the built_in functions used in the equation
        /// </summary>
        private List<BuiltInFunction> _builtInFunctions;

        public ManagedEquation(string equation, Range range) : this(equation)
        {
            _range = range;
        }

        public ManagedEquation(string equation)
        {
            _equation = Initialize(equation);
        }

        /// <summary>
        ///     List of all the words in the equation set in Initialize
        /// </summary>
        public List<string> Words { get; set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return _equation;
        }

        /// <summary>
        ///     Takes equation and the current variable values returns the result of the equation as the float
        /// </summary>
        /// <param name="variables"></param>
        /// <param name="sim"></param>
        /// <returns></returns>
        public float Compute(Variables variables, SimSpecs sim)
        {
            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }

            try
            {
                var explicitEquation = Prepare(variables, sim);
                if (string.IsNullOrEmpty(explicitEquation))
                {
                    return 0;
                }

                var e = new Expression(explicitEquation);
                return Convert.ToSingle(e.Evaluate());
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(" the internal details for this exception are as follows: \r\n" +
                                            ex.Message);
            }

            //todo Trace.WriteLineIf(World.LoggingSwitch.TraceVerbose, equation + "was calculate");
        }

        /// <summary>
        ///     Prepare equation to be computed
        ///     Replace all variables of the equation by its actual value
        /// </summary>
        /// <param name="variables"></param>
        /// <param name="sim"></param>
        /// <returns></returns>
        private string Prepare(Variables variables, SimSpecs sim)
        {
            var words = Words.ToArray();
            for (var i = 0; i < words.Length; i++)
            {
                var word = words[i];

                if (EquationUtils.Operators.Contains(word))
                {
                    continue;
                }

                for (var j = 0; j < _builtInFunctions.Count; j++)
                {
                    var function = _builtInFunctions[j];
                    if (word != function.Name + j)
                    {
                        continue;
                    }

                    words[i] = function.Prepare(word, sim);
                }

                var target = variables[word];
                if (target == null)
                {
                    continue;
                }

                var output = target.Value.ToString(CultureInfo.InvariantCulture);
                if (_range != null)
                {
                    words[i] = _range.GetOutputInsideRange(output).ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    words[i] = output;
                }
            }

            return string.Join(StringUtils.Blank, words);
        }

        /// <summary>
        ///     Clean equation to be able to be computed
        ///     Done once at the creation of the variable
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string Initialize(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            // Remove string in Braces which are units not equation
            var index = input.IndexOf('{');
            if (index > 0)
            {
                input = input.Remove(index);
            }

            _builtInFunctions = StringUtils.GetStringFunctions(input).ToList();
            for (var i = 0; i < _builtInFunctions.Count; i++)
            {
                var function = _builtInFunctions[i];
                input = input.Replace(function.Function, function.Name + i);
            }

            input = input.Replace(EquationUtils.Plus, StringUtils.Blank + EquationUtils.Plus + StringUtils.Blank);
            input = input.Replace(EquationUtils.Minus, StringUtils.Blank + EquationUtils.Minus + StringUtils.Blank);
            input = input.Replace(EquationUtils.Multiplication,
                StringUtils.Blank + EquationUtils.Multiplication + StringUtils.Blank);
            input = input.Replace(EquationUtils.Division,
                StringUtils.Blank + EquationUtils.Division + StringUtils.Blank);
            input = input.Replace(StringUtils.LParenthesis,
                StringUtils.Blank + StringUtils.LParenthesis + StringUtils.Blank);
            input = input.Replace(StringUtils.RParenthesis,
                StringUtils.Blank + StringUtils.RParenthesis + StringUtils.Blank);
            input = input.Replace("  ", StringUtils.Blank);
            input = input.TrimEnd(' ');

            Words = input.Split(' ').ToList();

            for (var i = 0; i < Words.Count; i++)
            {
                var word = Words[i];
                if (EquationUtils.Operators.Contains(word))
                {
                    continue;
                }

                Words[i] = StringUtils.CleanName(word);
            }

            return string.Join(StringUtils.Blank, Words);
        }

        public IEnumerable<string> GetVariables()
        {
            var variables = new List<string>();
            foreach (var word in Words)
            {
                if (word.Length <= 1)
                {
                    continue;
                }

                if (float.TryParse(word, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
                {
                    continue;
                }

                if (_builtInFunctions.Exists(x => word.StartsWith(x.Name)))
                {
                    continue;
                }

                variables.Add(word);
            }

            return variables;
        }
    }
}