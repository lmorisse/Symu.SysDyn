#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using NCalc2;
using Symu.SysDyn.Engine;
using Symu.SysDyn.Models;

#endregion

namespace Symu.SysDyn.Equations
{
    /// <summary>
    ///     IEquation with parameters without functions
    /// </summary>
    public class SimpleEquation : IEquation
    {
        public SimpleEquation(string originalEquation, string initializedEquation, List<string> variables,
            List<string> words, Range range) :
            this(originalEquation, initializedEquation, variables, words)
        {
            Range = range;
        }

        public SimpleEquation(string originalEquation, string initializedEquation, List<string> variables,
            List<string> words)
        {
            OriginalEquation = originalEquation;
            InitializedEquation = initializedEquation;
            Variables = variables;
            Expression = new Expression(InitializedEquation);
            Words = words;
        }

        /// <summary>
        ///     Range of the output of the equation provide by the variable
        /// </summary>
        protected Range Range { get; set; }

        /// <summary>
        ///     List of words that constitute the initialized equation
        ///     It is necessary for the replace method
        /// </summary>
        protected List<string> Words { get; }

        protected Expression Expression { get; set; }

        #region IEquation Members

        public string OriginalEquation { get; protected set; }
        public string InitializedEquation { get; set; }

        public List<string> Variables { get; }

        public virtual IEquation Clone()
        {
            return new SimpleEquation(OriginalEquation, InitializedEquation, Variables.ToList(), Words.ToList(), Range);
        }

        public virtual bool CanBeOptimized(string variableName)
        {
            var itself = Words.Count == 1 && Variables.Count == 1 && Variables[0] == variableName;
            return !Variables.Any() || itself;
        }

        /// <summary>
        ///     Takes equation and the current variable values returns the result of the equation as the float
        /// </summary>
        /// <param name="selfVariable"></param>
        /// <param name="variables"></param>
        /// <param name="sim"></param>
        /// <returns></returns>
        public float Evaluate(IVariable selfVariable, VariableCollection variables, SimSpecs sim)
        {
            try
            {
                Prepare(selfVariable, variables, sim);
                return Convert.ToSingle(Expression.Evaluate());
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
        /// <param name="selfVariable">The variable parent of the equation</param>
        /// <param name="variables"></param>
        /// <param name="sim"></param>
        /// <returns></returns>
        public virtual void Prepare(IVariable selfVariable, VariableCollection variables, SimSpecs sim)
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

        public float InitialValue()
        {
            var value = Convert.ToSingle(Expression.Evaluate());
            if (float.IsNaN(value) || float.IsInfinity(value))
            {
                value = 0;
            }

            return value;
        }


        public virtual void Replace(string child, string value, SimSpecs sim)
        {
            while (Words.FindIndex(ind => ind.Equals(child)) >= 0)
            {
                Words[Words.FindIndex(ind => ind.Equals(child))] = value;
            }

            InitializedEquation = string.Join(string.Empty, Words);
            Variables.Remove(child);
            Expression = new Expression(InitializedEquation);
        }

        #endregion

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return InitializedEquation;
        }
    }
}