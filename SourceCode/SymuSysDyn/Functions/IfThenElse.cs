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
using Symu.SysDyn.Equations;

#endregion

namespace Symu.SysDyn.Functions
{
    /// <summary>
    ///     IF condition THEN expression ELSE expression
    ///      all non-zero values are true, while zero is false.
    /// Generally, condition is an expression involving the logical, relational, and equality operators.

    /// </summary>
    public class IfThenElse : BuiltInFunction
    {

        public IfThenElse(string function) 
        {
            OriginalFunction = function ?? throw new ArgumentNullException(nameof(function));
            Name = "If";
            Parse(ref function, out var parameters, out var args);
            Parameters = parameters;
            Args = args;
            Expression = new Expression(function);
        }

        /// <summary>
        /// Parse the string function to extract the if-then-else conditions in the parameters
        /// </summary>
        /// <param name="input"></param>
        /// <param name="parameters"></param>
        /// <param name="args"></param>
        public static void Parse(ref string input, out List<IEquation> parameters, out List<float> args)
        {
            parameters = new List<IEquation>();
            args = new List<float>();

            var result = MatchRegex(input);
            if (!result.Success)
            {
                return;
            }

            var equation = "if(";
            var condition = EquationFactory.CreateInstance(result.Groups[1].Value, out var value);
            equation = UpdateEquation(parameters, args, condition, value, equation);

            equation += ",";
            var thenExpression = EquationFactory.CreateInstance(result.Groups[2].Value, out value);
            equation = UpdateEquation(parameters, args, thenExpression, value, equation);

            equation += ",";
            var elseExpression = EquationFactory.CreateInstance(result.Groups[3].Value, out value);
            equation = UpdateEquation(parameters, args, elseExpression, value, equation);
            input = equation + ")";
        }

        private static string UpdateEquation(ICollection<IEquation> parameters, ICollection<float> args, IEquation elseExpression, float value, string equation)
        {
            parameters.Add(elseExpression);
            args.Add(value);
            if (elseExpression != null)
            {
                equation += elseExpression;
            }
            else
            {
                equation += value.ToString(CultureInfo.InvariantCulture);
            }

            return equation;
        }

        private static Match MatchRegex(string input)
        {
            var regex = new Regex(@"IF (.*) THEN (.*) ELSE (.*)", RegexOptions.IgnoreCase);
            return regex.Match(input);
        }

        ///// <summary>
        ///// "if(Condition,ThenExpression,ElseExpression)"
        ///// </summary>
        ///// <returns></returns>
        //public string SetEquation()
        //{
        //    var condition = Parameters.ElementAt(0);
        //    var thenExpression = Parameters.ElementAt(1);
        //    var elseExpression = Parameters.ElementAt(2);
        //    return "if(" + condition + "," + thenExpression + "," + elseExpression + ")";
        //}

        /// <summary>
        ///     Check if it is a IfThenElse function
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsContainedIn(string input)
        {
            return MatchRegex(input).Success;
        }
    }
}