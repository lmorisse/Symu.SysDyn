#region Licence

// Description: SymuSysDyn - SymuSysDynCore
// Website: https://symu.org
// Copyright: (c) 2021 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NCalcAsync;
using NCalcAsync.Domain;
using Symu.SysDyn.Core.Parser;

#endregion

namespace Symu.SysDyn.Core.Equations
{
    /// <summary>
    ///     Use NCalc to extract parameters and functions from an expression (or equation string)
    ///     without creating the expression yet
    /// </summary>
    public class CompiledExpression
    {
        public static string[] NotOptimizableFunctions = {"pulse", "step", "ramp", "smth1", "smth3", "smthn"};
        public List<string> Parameters { get; set; } = new List<string>(); //Identifier.Name
        public bool CanBeOptimized { get; set; } = true;

        public static string ReplaceLogicalExpression(LogicalExpression logicalExpression, string equation, string name,
            string replacedValue)
        {
            switch (logicalExpression)
            {
                case TernaryExpression ternary:
                    equation = ReplaceLogicalExpression(ternary.LeftExpression, equation, name, replacedValue);
                    equation = ReplaceLogicalExpression(ternary.MiddleExpression, equation, name, replacedValue);
                    equation = ReplaceLogicalExpression(ternary.RightExpression, equation, name, replacedValue);
                    break;
                case BinaryExpression binary:
                    equation += "(";
                    equation = ReplaceLogicalExpression(binary.LeftExpression, equation, name, replacedValue);
                    switch (binary.Type)
                    {
                        case BinaryExpressionType.And:
                            equation += "&&";
                            break;

                        case BinaryExpressionType.Or:
                            equation += "||";
                            break;

                        case BinaryExpressionType.Div:
                            equation += "/";
                            break;

                        case BinaryExpressionType.Equal:
                            equation += "==";
                            break;

                        case BinaryExpressionType.Greater:
                            equation += ">";
                            break;

                        case BinaryExpressionType.GreaterOrEqual:
                            equation += ">=";
                            break;

                        case BinaryExpressionType.Lesser:
                            equation += "<";
                            break;

                        case BinaryExpressionType.LesserOrEqual:
                            equation += "<=";
                            break;

                        case BinaryExpressionType.Minus:
                            equation += "-";
                            break;

                        case BinaryExpressionType.Modulo:
                            equation += "%";
                            break;

                        case BinaryExpressionType.NotEqual:
                            equation += "!=";
                            break;

                        case BinaryExpressionType.Plus:
                            equation += "+";

                            break;

                        case BinaryExpressionType.Times:
                            equation += "*";
                            break;

                        case BinaryExpressionType.BitwiseAnd:
                            equation += "&";
                            break;

                        case BinaryExpressionType.BitwiseOr:
                            equation += "|";
                            break;

                        case BinaryExpressionType.BitwiseXOr:
                            equation += "^";
                            break;

                        case BinaryExpressionType.LeftShift:
                            equation += "<<";
                            break;

                        case BinaryExpressionType.RightShift:
                            equation += ">>";
                            break;
                    }

                    equation = ReplaceLogicalExpression(binary.RightExpression, equation, name, replacedValue);
                    equation += ")";
                    break;
                case UnaryExpression unary:
                    equation = ReplaceLogicalExpression(unary.Expression, equation, name, replacedValue);
                    break;
                case Function function:
                    equation += $"{function.Identifier.Name}(";
                    for (var i = 0; i < function.Expressions.Length; i++)
                    {
                        var expression = function.Expressions[i];
                        equation = ReplaceLogicalExpression(expression, equation, name, replacedValue);
                        if (i < function.Expressions.Length - 1)
                        {
                            equation += ",";
                        }
                    }

                    equation += ")";
                    break;
                case ValueExpression value: //constant value
                    equation += Convert.ToString(value.Value, CultureInfo.InvariantCulture);
                    break;
                case Identifier identifier: //Parameter
                    if (identifier.Name == name)
                    {
                        equation += replacedValue;
                    }
                    else
                    {
                        equation += identifier.Name;
                    }

                    break;
            }

            return equation;
        }


        #region Get

        public static CompiledExpression Get(string equation, string model)
        {
            var logicalExpression = Expression.Compile(equation, true);
            return Get(logicalExpression, model);
        }

        public static CompiledExpression Get(LogicalExpression expression, string model)
        {
            var compiledExpression = new CompiledExpression();
            ExtractLogicalExpression(expression, compiledExpression, model, expression.RetrievedFromCache);
            compiledExpression.Parameters = compiledExpression.Parameters.Distinct().ToList();
            return compiledExpression;
        }

        private static void ExtractLogicalExpression(LogicalExpression logicalExpression,
            CompiledExpression compiledExpression, string model, bool retrievedFromCache)
        {
            switch (logicalExpression)
            {
                case TernaryExpression ternary:
                    ExtractLogicalExpression(ternary.LeftExpression, compiledExpression, model, retrievedFromCache);
                    ExtractLogicalExpression(ternary.MiddleExpression, compiledExpression, model, retrievedFromCache);
                    ExtractLogicalExpression(ternary.RightExpression, compiledExpression, model, retrievedFromCache);
                    break;
                case BinaryExpression binary:
                    ExtractLogicalExpression(binary.LeftExpression, compiledExpression, model, retrievedFromCache);
                    ExtractLogicalExpression(binary.RightExpression, compiledExpression, model, retrievedFromCache);
                    break;
                case UnaryExpression unary:
                    ExtractLogicalExpression(unary.Expression, compiledExpression, model, retrievedFromCache);
                    break;
                case Function function:
                    //parameters.Functions.Add(function.Identifier.Name);
                    if (compiledExpression.CanBeOptimized)
                    {
                        compiledExpression.CanBeOptimized =
                            !NotOptimizableFunctions.Contains(function.Identifier.Name.ToLowerInvariant());
                    }

                    foreach (var expression in function.Expressions)
                    {
                        ExtractLogicalExpression(expression, compiledExpression, model, retrievedFromCache);
                    }

                    break;
                case ValueExpression _: //constant value
                    // Intentionally blank
                    break;
                case Identifier identifier: //Parameter
                    if (!retrievedFromCache)
                    {
                        identifier.Name = StringUtils.CleanFullName(model, identifier.Name);
                    }

                    compiledExpression.Parameters.Add(identifier.Name);
                    break;
            }
        }

        #endregion
    }
}