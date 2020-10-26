#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using Symu.SysDyn.Model;
using Symu.SysDyn.Simulation;

#endregion

namespace Symu.SysDyn.Equations
{
    /// <summary>
    /// IEquation that has parameters but no functions
    /// </summary>
    public class SimpleEquation : IEquation
    {

        public string OriginalEquation { get; }
        public string InitializedEquation { get; set; }
        /// <summary>
        /// Result of the Prepare method
        /// Simplified version of Expression
        /// </summary>
        private float _eval;

        private readonly List<string> _words;

        /// <summary>
        ///     Range of the output of the equation provide by the variable
        /// </summary>
        protected Range Range { get; set; }

        public List<string> Variables { get; } 

        public SimpleEquation(string originalEquation, string initializedEquation, List<string> variables, List<string> words, Range range) : 
            this(originalEquation, initializedEquation, variables, words)
        {
            Range = range;
        }

        public SimpleEquation(string originalEquation, string initializedEquation, List<string> variables, List<string> words)
        {
            OriginalEquation = originalEquation;
            InitializedEquation = initializedEquation;
            Variables = variables;
            _words = words;
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
        public virtual float Evaluate(Variables variables, SimSpecs sim)
        {
            Prepare(variables, sim);
            return _eval;
        }

        public float InitialValue()
        {
            Prepare(new Variables(), null);
            return _eval;
        }

        public void Replace(string child, string value)
        {
            //var index = _words.FindIndex(ind => ind.Equals(child));
            //if (index < 0)
            //{
            //    return;
            //}
            //_words[index] = value;
            while (_words.FindIndex(ind => ind.Equals(child)) >= 0)
            {
                _words[_words.FindIndex(ind => ind.Equals(child))] = value;
            }
            InitializedEquation = string.Join(string.Empty,_words);
            Variables.Remove(child);
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
            _eval = 0;
            var @operator=string.Empty;
            foreach (var word in _words)
            {
                switch (word)
                {
                    case "+":
                    case "-":
                    case "*":
                    case "/":
                        @operator = word;
                        break;
                    default:
                        var output = 0F;
                        var variable = variables.Get(word);
                        if (variable != null)
                        {
                            output = Range?.GetOutputInsideRange(variable.Value) ?? variable.Value;
                        }
                        else if (float.TryParse(word, out var parse))
                        {
                            output = parse;
                        }
                        switch (@operator)
                        {
                            case "+":
                                _eval += output;
                                break;
                            case "-":
                                _eval -= output;
                                break;
                            case "/":
                                _eval /= output;
                                break;
                            case "*":
                                _eval *= output;
                                break;
                            default:
                                _eval = output;
                                break;
                        }

                        break;
                }
            }
        }
    }
}