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
using System.Text.RegularExpressions;
using NCalc2;
using Symu.SysDyn.Equations;

#endregion

namespace Symu.SysDyn.Functions
{
    /// <summary>
    ///     IF condition THEN expression ELSE expression
    ///     all non-zero values are true, while zero is false.
    ///     Generally, condition is an expression involving the logical, relational, and equality operators.
    /// </summary>
    public class IfThenElse : BuiltInFunction
    {
        public IfThenElse(string model, string function)
        {
            OriginalFunction = function ?? throw new ArgumentNullException(nameof(function));
            Name = "If";
            Model = model;
            Parse(model, ref function, out var parameters, out var args);
            Parameters = parameters;
            Args = args;
            Expression = new Expression(function);
        }

        public override IBuiltInFunction Clone()
        {
            var clone = new IfThenElse(Model, OriginalFunction);
            CopyTo(clone);
            return clone;
        }

        /// <summary>
        ///     Parse the string function to extract the if-then-else conditions in the parameters
        /// </summary>
        /// <param name="model"></param>
        /// <param name="input"></param>
        /// <param name="parameters"></param>
        /// <param name="args"></param>
        public static void Parse(string model, ref string input, out List<IEquation> parameters, out List<float> args)
        {
            parameters = new List<IEquation>();
            args = new List<float>();

            var result = MatchRegex(input);
            if (!result.Success)
            {
                return;
            }

            var equation = "if(";
            var condition = EquationFactory.CreateInstance(model, result.Groups[1].Value, out var value);
            equation = UpdateEquation(parameters, args, condition, value, equation);

            equation += ",";
            var thenExpression = EquationFactory.CreateInstance(model, result.Groups[2].Value, out value);
            equation = UpdateEquation(parameters, args, thenExpression, value, equation);

            equation += ",";
            var elseExpression = EquationFactory.CreateInstance(model, result.Groups[3].Value, out value);
            equation = UpdateEquation(parameters, args, elseExpression, value, equation);
            input = equation + ")";
        }

        private static string UpdateEquation(ICollection<IEquation> parameters, ICollection<float> args,
            IEquation elseExpression, float value, string equation)
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
            var regex = new Regex(@"IF\s*(.*)\s*THEN\s*(.*)\s*ELSE\s*(.*)", RegexOptions.IgnoreCase);
            return regex.Match(input);
        }

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