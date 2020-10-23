#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using NCalc2;
using Symu.SysDyn.Functions;
using Symu.SysDyn.Model;
using Symu.SysDyn.Simulation;

#endregion

namespace Symu.SysDyn.Equations
{
    /// <summary>
    /// IEquation with parameters and functions
    /// </summary>
    public class ComplexEquation : IEquation
    {
        public string OriginalEquation { get; }
        public string InitializedEquation { get; }
        /// <summary>
        ///     Range of the output of the equation provide by the variable
        /// </summary>
        private readonly Range _range;

        public List<string> Variables { get; }
        private readonly Expression _expression;

        /// <summary>
        ///     List of all the nested functions used in the equation
        /// </summary>
        private readonly List<BuiltInFunction> _functions;

        public ComplexEquation(string originalEquation, string initializedEquation, List<BuiltInFunction> functions, List<string> variables, Range range) : 
            this(originalEquation, initializedEquation, functions, variables)
        {
            _range = range;
        }

        public ComplexEquation(string originalEquation, string initializedEquation, List<BuiltInFunction> functions, List<string> variables)
        {
            OriginalEquation = originalEquation;
            InitializedEquation = initializedEquation;
            Variables = variables;
            _functions = functions;
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
    }
}