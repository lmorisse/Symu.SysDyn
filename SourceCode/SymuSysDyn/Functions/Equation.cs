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
using System.Text.RegularExpressions;
using NCalc2;
using Symu.SysDyn.Functions;
using Symu.SysDyn.Model;
using Symu.SysDyn.Parser;
using Symu.SysDyn.Simulation;

#endregion

namespace Symu.SysDyn.Functions
{
    public class Equation : IEquation
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

            if (float.TryParse(eqn, out var floatEqn))
            {
                return new ConstantEquation(floatEqn);
            }

            return new Equation(eqn, range);
        }
        public string OriginalEquation { get; }
        public string InitializedEquation { get; }
        private readonly Expression _expression;

        /// <summary>
        ///     Range of the output of the equation provide by the variable
        /// </summary>
        private readonly Range _range;

        /// <summary>
        ///     List of all the nested functions used in the equation
        /// </summary>
        private List<BuiltInFunction> _functions = new List<BuiltInFunction>();
        public List<string> Variables { get; private set; } = new List<string>();

        public Equation(string equation, Range range) : this(equation)
        {
            _range = range;
        }

        public Equation(string equation)
        {
            if (equation == null)
            {
                throw new ArgumentNullException(nameof(equation));
            }

            OriginalEquation = equation.Trim();
            InitializedEquation = Initialize(equation);
            _expression = new Expression(InitializedEquation);
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return InitializedEquation;
        }

        /// <summary>
        ///     Takes equation and the current variable values returns the result of the equation as the float
        /// </summary>
        /// <param name="variables"></param>
        /// <param name="sim"></param>
        /// <returns></returns>
        public float Evaluate(Variables variables, SimSpecs sim)
        {
            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }

            if (_expression == null)
            {
                return 0;
            }

            try
            {
                Prepare(variables, sim);
                return Convert.ToSingle(_expression.Evaluate());
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
        public void Prepare(Variables variables, SimSpecs sim)
        {
            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }
            // Variables
            foreach (var variable in Variables)
            {
                if (!variables.Exists(variable))
                {
                    //In case of SmthMachine with parameters
                    continue;
                }

                var output = variables[variable].Value;
                if (_range != null)
                {
                    _expression.Parameters[variable] = _range.GetOutputInsideRange(output);
                }
                else
                {
                    _expression.Parameters[variable] = output;
                }
            }
            // Built-in functions
            foreach (var function in _functions)
            {
                _expression.Parameters[function.IndexName] = function.Prepare(variables, sim);
            }
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
            // Case of a numeric
            if (float.TryParse(input, out _))
            {
                return input.Trim();
            }

            // Remove string in Braces which are units not equation
            var index = input.IndexOf('{');
            if (index > 0)
            {
                input = input.Remove(index);
            }
            input = input.Trim();
            _functions = StringFunction.GetFunctions(input).ToList();
            for (var i = 0; i < _functions.Count; i++)
            {
                var function = _functions[i];
                function.IndexName = function.Name + i;
                input = input.Replace(function.OriginalFunction, function.IndexName);
            }
            // split equation in words
            var regexWords = new Regex(@"[0-9]*\.?\,?[0-9]+|[-^+*\/()<>=]|\w+");
            var matches = regexWords.Matches(input);
            var words = new List<string>();
            foreach (var match in matches)
            {
                var word = StringUtils.CleanName(match.ToString());
                words.Add(word);
                SetVariables(word);
            }

            Variables = Variables.Distinct().ToList();
            return words.Aggregate(string.Empty, (current, word) => current + word);
        }

        private void SetVariables (string word)
        {
            if (word.Length <= 1)
            {
                return;
            }

            if (float.TryParse(word, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
            {
                return;
            }

            var function = _functions.Find(x => word == x.IndexName);
            if (function != null)
            {
                //Get the variables of the function
                foreach (var equation in function.Parameters)
                {
                    Variables.AddRange(equation.Variables);
                }
            }
            else
            {
                Variables.Add(word);
            }
        }
    }
}