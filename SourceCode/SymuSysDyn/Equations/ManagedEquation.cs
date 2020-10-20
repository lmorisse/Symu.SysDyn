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
        public string OriginalEquation { get; }
        public string InitializedEquation { get; }
        private readonly Expression _expression;

        /// <summary>
        ///     Range of the output of the equation provide by the variable
        /// </summary>
        private readonly Range _range;

        /// <summary>
        ///     List of all the built_in functions used in the equation
        /// </summary>
        private List<BuiltInFunction> _builtInFunctions;
        public List<string> Variables { get; } = new List<string>();

        public ManagedEquation(string equation, Range range) : this(equation)
        {
            _range = range;
        }

        public ManagedEquation(string equation)
        {
            OriginalEquation = equation;
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
            foreach (var function in _builtInFunctions)
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

            // Remove string in Braces which are units not equation
            var index = input.IndexOf('{');
            if (index > 0)
            {
                input = input.Remove(index);
            }
            input = input.Trim();
            _builtInFunctions = StringUtils.GetStringFunctions(input).ToList();
            for (var i = 0; i < _builtInFunctions.Count; i++)
            {
                var function = _builtInFunctions[i];
                function.IndexName = function.Name + i;
                input = input.Replace(function.OriginalFunction, function.IndexName);
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

            var words = input.Split(' ').ToList();
            words.RemoveAll(string.IsNullOrEmpty);

            for (var i = 0; i < words.Count; i++)
            {
                var word = words[i];
                if (EquationUtils.Operators.Contains(word))
                {
                    continue;
                }

                words[i] = StringUtils.CleanName(word);
            }
            SetVariables(words);
            return words.Aggregate(string.Empty, (current, word) => current + word);
        }

        private void SetVariables (IEnumerable<string> words)
        {
            foreach (var word in words)
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

                Variables.Add(word);
            }
        }
    }
}