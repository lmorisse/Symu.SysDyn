#region Licence

// Description: SymuBiz - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NCalcAsync;
using Symu.SysDyn.Core.Equations;

#endregion

namespace Symu.SysDyn.Core.Functions
{
    /// <summary>
    ///     IF condition THEN expression ELSE expression
    ///     all non-zero values are true, while zero is false.
    ///     Generally, condition is an expression involving the logical, relational, and equality operators.
    /// </summary>
    public class IfThenElse : BuiltInFunction
    {
        public static async Task<IfThenElse> CreateIfThenElse(string model, string function)
        {
            var ifThenElse = new IfThenElse
            {
                OriginalFunction = function ?? throw new ArgumentNullException(nameof(function)),
                Name = "If",
                Model = model
            };
            function = await ifThenElse.Parse(model, function);
            ifThenElse.Expression = new Expression(function);
            return ifThenElse;
        }

        public override async Task<IBuiltInFunction> Clone()
        {
            var clone = await CreateIfThenElse(Model, OriginalFunction);
            CopyTo(clone);
            return clone;
        }

        /// <summary>
        ///     Parse the string function to extract the if-then-else conditions in the parameters
        /// </summary>
        /// <param name="model"></param>
        /// <param name="input"></param>
        public async Task<string> Parse(string model, string input)
        {
            Parameters = new List<IEquation>();
            Args = new List<float>();

            var result = MatchRegex(input);
            if (!result.Success)
            {
                return input;
            }

            var equation = "if(";
            var condition = await EquationFactory.CreateInstance(model, PrepareExpression(result.Groups[1].Value));
            equation = UpdateEquation(Parameters, Args, condition.Equation, condition.Value, equation);

            equation += ",";
            var thenExpression = await EquationFactory.CreateInstance(model, PrepareExpression(result.Groups[2].Value));
            equation = UpdateEquation(Parameters, Args, thenExpression.Equation, thenExpression.Value, equation);

            equation += ",";
            var elseExpression = await EquationFactory.CreateInstance(model, PrepareExpression(result.Groups[3].Value));
            equation = UpdateEquation(Parameters, Args, elseExpression.Equation, elseExpression.Value, equation);
            return equation + ")";
        }

        private static string PrepareExpression(string value)
        {
            value = value.Trim();
            if (value.StartsWith("(") && value.EndsWith(")"))
            {
                value = value.Remove(0, 1);
                return value.Remove(value.Length - 1, 1);
            }

            return value;
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