#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using NCalc2;
using Symu.SysDyn.Model;
using Symu.SysDyn.Parser;

namespace Symu.SysDyn.Simulation
{
    public static class ManagedEquation
    {
        public const string Plus = "+";
        public const string Minus = "-";
        public const string Division = "/";
        public const string Multiplication = "*";
        public const string LParenthesis = "(";
        public const string RParenthesis = ")";
        public const string Blank = " ";

        #region Built In Functions
        public const string Dt = "Dt";
        //TODO implement TIME
        public const string Time = "Time";
        /// <summary>
        /// Built-in functions
        /// </summary>
        public static List<string> Functions { get; } = new List<string> { Dt, Time };
        #endregion
        public static List<string> Operators { get; } = new List<string> {Blank, Plus, Minus, Division,
            Multiplication };

        /// <summary>
        ///     Takes equation and the current variable values returns the result of the equation as the float
        /// </summary>
        /// <param name="variable">The variable that contains the equation to prepare</param>
        /// <param name="variables"></param>
        /// <param name="deltaTime"></param>
        /// <returns></returns>
        public static float Compute(Variable variable, Variables variables, float deltaTime)
        {
            if (variable?.Equation == null)
            {
                throw new ArgumentNullException(nameof(variable));
            }

            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }

            try
            {
                var explicitEquation = Prepare(variable, variables, deltaTime);
                var e = new Expression(explicitEquation);
                return Convert.ToSingle(e.Evaluate());
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(nameof(variable) +
                                            " : the internal details for this exception are as follows: \r\n" +
                                            ex.Message);
            }

            //todo Trace.WriteLineIf(World.LoggingSwitch.TraceVerbose, equation + "was calculate");
        }

        /// <summary>
        /// Prepare equation to be computed
        /// Replace all variables of the equation by its actual value
        /// </summary>
        /// <param name="variable">The variable that contains the equation to prepare</param>
        /// <param name="variables"></param>
        /// <param name="deltaTime"></param>
        /// <returns></returns>
        private static string Prepare(Variable variable, Variables variables, float deltaTime)
        {
            var words = variable.Equation.Split(' ');

            for (var counter = 0; counter < words.Length; counter++)
            {
                var word = words[counter];
                if (Operators.Contains(word))
                {
                    continue;
                }
                if (Functions.Contains(word))
                {
                    switch (word)
                    {
                        case Dt:
                            words[counter] = deltaTime.ToString(CultureInfo.InvariantCulture);
                            break;
                    }

                    continue;
                }

                var target = variables[word];
                if (target == null)
                {
                    continue;
                }

                var input = target.Value.ToString(CultureInfo.InvariantCulture);
                words[counter] = variable.Range.GetOutputInsideRange(input).ToString(CultureInfo.InvariantCulture);
            }

            return string.Join(Blank, words);
        }

        /// <summary>
        /// Clean equation to be able to be computed
        /// Done once at the creation of the variable
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Initialize(string input)
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

            input = input.Replace(Plus, Blank + Plus + Blank);
            input = input.Replace(Minus, Blank + Minus + Blank);
            input = input.Replace(Multiplication, Blank + Multiplication + Blank);
            input = input.Replace(Division, Blank + Division + Blank);
            input = input.Replace(LParenthesis, Blank + LParenthesis + Blank);
            input = input.Replace(RParenthesis, Blank + RParenthesis + Blank);
            input = input.Replace("  ", Blank);
            input = input.TrimEnd(' ');

            var words = input.Split(' ');

            for (var counter = 0; counter < words.Length; counter++)
            {
                var word = words[counter];
                if (Operators.Contains(word))
                {
                    continue;
                }

                words[counter] = StringUtils.CleanName(word);
            }

            return string.Join(Blank, words);
        }
    }
}