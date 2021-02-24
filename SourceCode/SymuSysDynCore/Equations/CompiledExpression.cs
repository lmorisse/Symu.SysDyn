#region Licence

// Description: SymuSysDyn - SymuSysDynCore
// Website: https://symu.org
// Copyright: (c) 2021 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NCalcAsync;
using NCalcAsync.Domain;

namespace Symu.SysDyn.Core.Equations
{
    /// <summary>
    /// Use NCalc to extract parameters and functions from an expression (or equation string)
    /// without creating the expression yet
    /// </summary>
    public class CompiledExpression
    {
        public List<string> Parameters { get; set; } = new List<string>();//Identifier.Name
        public List<string> Functions { get; set; } = new List<string>();
        public static CompiledExpression GetCompiledExpression(string expression)
        {
            var compiled = new CompiledExpression();
            var logicalExpression = Expression.Compile(expression, true);
            ExtractLogicalExpression(logicalExpression, compiled);
            return compiled;
        }

        private static void ExtractLogicalExpression(LogicalExpression logicalExpression, CompiledExpression compiled)
        {
            switch (logicalExpression)
            {
                case TernaryExpression ternary:
                    ExtractLogicalExpression(ternary.LeftExpression, compiled);
                    ExtractLogicalExpression(ternary.MiddleExpression, compiled);
                    ExtractLogicalExpression(ternary.RightExpression, compiled);
                    break;
                case BinaryExpression binary:
                    ExtractLogicalExpression(binary.LeftExpression, compiled);
                    ExtractLogicalExpression(binary.RightExpression, compiled);
                    break;
                case UnaryExpression unary:
                    ExtractLogicalExpression(unary.Expression, compiled);
                    break;
                case Function function:
                    compiled.Functions.Add(function.Identifier.Name);
                    foreach (var expression in function.Expressions)
                    {
                        ExtractLogicalExpression(expression, compiled);
                    }
                    break;
                case ValueExpression value:
                    //compiled.Parameters.Add(value.Value);
                    throw new NotImplementedException();
                case Identifier identifier:
                    compiled.Parameters.Add(identifier.Name);
                    break;
            }
        }
    }
}