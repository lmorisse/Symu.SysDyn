#region Licence

// Description: SymuSysDyn - SymuSysDynCore
// Website: https://symu.org
// Copyright: (c) 2021 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NCalcAsync;
using NCalcAsync.Domain;
using Symu.SysDyn.Core.Engine;
using Symu.SysDyn.Core.Models.XMile;

#endregion

namespace Symu.SysDyn.Core.Equations
{
    /// <summary>
    /// </summary>
    public class Equation //: IEquation
    {
        /// <summary>
        ///     True if Expression does not contains Non optimizable functions (typically time dependent) such as Step, Ramp, ...
        ///     Initialized by CompiledEquation
        /// </summary>
        private bool _canBeOptimized;

        public Equation(string eqn)
        {
            Expression = new Expression(eqn, EvaluateOptions.IgnoreCase);
        }

        public Expression Expression { get; set; }
        public string OriginalEquation => Expression.OriginalExpression;
        public List<string> Variables { get; set; } = new List<string>();

        // todo Change name => Initialize
        public async Task<float> InitialValue(string model)
        {
            if (Expression.HasErrors())
            {
                return 0;
            }

            // Expression has been compiled with the first Evaluation
            // So ParsedExpression is initialized, we can extract all the parameters
            var compiledExpression = CompiledExpression.Get(Expression.ParsedExpression, model);
            _canBeOptimized = compiledExpression.CanBeOptimized;
            Variables.AddRange(compiledExpression.Parameters);
            if (Variables.Any(x => !x.EndsWith("_Time") && !x.EndsWith("_Dt")))
            {
                return 0;
            }

            var value = Convert.ToSingle(await Expression.EvaluateAsync(0, 0, 1));
            if (float.IsNaN(value) || float.IsInfinity(value))
            {
                return 0;
            }

            return value;
        }

        public async Task<float> Evaluate(IVariable selfVariable, VariableCollection variables, SimSpecs sim)
        {
            try
            {
                Prepare(selfVariable, variables, sim);
                return Convert.ToSingle(await Expression.EvaluateAsync(sim?.Time, sim?.Step, sim?.DeltaTime));
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(" the internal details for this exception are as follows: \r\n" +
                                            ex.Message);
            }

            //todo Trace.WriteLineIf(World.LoggingSwitch.TraceVerbose, equation + "was calculate");
        }

        public void Prepare(IVariable selfVariable, VariableCollection variables, SimSpecs sim)
        {
            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }

            foreach (var variable in Variables.Where(variables.Exists))
            {
                Expression.Parameters[variable] = variables[variable].Value;
            }
        }

        public void Replace(string child, string value)
        {
            if (!Variables.Contains(child) || Expression.ParsedExpression == null)
            {
                return;
            }

            var replacedExpression =
                CompiledExpression.ReplaceLogicalExpression(Expression.ParsedExpression, string.Empty, child, value);
            Variables.Remove(child);
            if (string.IsNullOrEmpty(replacedExpression))
            {
                Expression = null;
                return;
            }

            Expression = new Expression(replacedExpression, EvaluateOptions.IgnoreCase);
            Expression.HasErrors(); // use to compile expression and get the ParsedExpression
            //return Expression.HasErrors();
            //todo use this return to check error
        }

        public async Task<Equation> Clone(string model)
        {
            var equation = new Equation(Expression.OriginalExpression);
            if (Expression.HasErrors())
            {
                //todo Error management
                // This information should be thrown to StateMachine
                return null;
            }

            equation._canBeOptimized = _canBeOptimized;
            if (Variables.Any())
            {
                equation.Variables.AddRange(Variables);
            }
            else
            {
                // To initialize Expression.LastValue
                await Expression.EvaluateAsync(0, 0, 1);
            }

            return equation;
        }

        public virtual bool CanBeOptimized()
        {
            return _canBeOptimized && !Variables.Any();
        }

        public virtual bool CanBeOptimized(string variableName)
        {
            var itself = Variables.Count == 1 && Variables[0] == variableName &&
                         Expression.ParsedExpression is Identifier;
            // Remove Stock without inflow and outflow
            return _canBeOptimized && !Variables.Any() || itself;
        }
    }
}