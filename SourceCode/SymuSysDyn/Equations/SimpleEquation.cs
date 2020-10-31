#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;

using NCalc2;

using Symu.SysDyn.Functions;
using Symu.SysDyn.Model;
using Symu.SysDyn.Simulation;

#endregion

namespace Symu.SysDyn.Equations
{
    /// <summary>
    /// IEquation with parameters without functions
    /// </summary>
    public class SimpleEquation : IEquation
    {
        public string OriginalEquation { get; protected set; }
        public string InitializedEquation { get; set; }
        /// <summary>
        ///     Range of the output of the equation provide by the variable
        /// </summary>
        protected Range _range { get; set; }

        public List<string> Variables { get; protected set; }
        /// <summary>
        /// List of words that constitute the initialized equation
        /// It is necessary for the replace method
        /// </summary>
        protected List<string> _words { get; set; }
        protected Expression _expression;

        public SimpleEquation(string originalEquation, string initializedEquation, List<string> variables, List<string> words, Range range) :
            this(originalEquation, initializedEquation, variables, words)
        {
            _range = range;
        }

        public SimpleEquation(string originalEquation, string initializedEquation, List<string> variables, List<string> words)
        {
            OriginalEquation = originalEquation;
            InitializedEquation = initializedEquation;
            Variables = variables;
            _expression = new Expression(InitializedEquation);
            _words = words;
        }

        public virtual IEquation Clone()
        {
            return new SimpleEquation(OriginalEquation, InitializedEquation, Variables, _words, _range);
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return InitializedEquation;
        }
        public virtual bool CanBeOptimized(string variableName)
        {
            var itself = _words.Count == 1 && Variables.Count == 1 && Variables[0] == variableName;
            return !Variables.Any() || itself;
        }

        /// <summary>
        ///     Takes equation and the current variable values returns the result of the equation as the float
        /// </summary>
        /// <param name="selfVariable"></param>
        /// <param name="variables"></param>
        /// <param name="sim"></param>
        /// <returns></returns>
        public float Evaluate(Variable selfVariable, Variables variables, SimSpecs sim)
        {
            try
            {
                Prepare(selfVariable, variables, sim);
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
        public virtual void Prepare(Variable selfVariable, Variables variables, SimSpecs sim)
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
                _expression.Parameters[variable] = output;
            }
        }

        public float InitialValue()
        {
            var value = Convert.ToSingle(_expression.Evaluate());
            if (float.IsNaN(value) || float.IsInfinity(value))
            {
                value = 0;
            }

            return value;
        }


        public virtual void Replace(string child, string value)
        {
            while (_words.FindIndex(ind => ind.Equals(child)) >= 0)
            {
                _words[_words.FindIndex(ind => ind.Equals(child))] = value;
            }

            InitializedEquation = string.Join(string.Empty, _words);
            Variables.Remove(child);
            _expression = new Expression(InitializedEquation);
        }
    }
}