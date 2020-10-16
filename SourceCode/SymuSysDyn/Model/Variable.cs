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
using Symu.SysDyn.Parser;
using Symu.SysDyn.Simulation;

#endregion

namespace Symu.SysDyn.Model
{
    /// <summary>
    ///     Base class for the variable of the model
    /// </summary>
    public class Variable
    {
        public Variable(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = StringUtils.CleanName(name);
        }

        public Variable(string name, string eqn) : this(name)
        {
            Value = CheckInitialValue(eqn);
            Equation = ManagedEquation.Initialize(eqn);
            Units = Units.CreateInstanceFromEquation(eqn);
            SetChildren();
        }

        public Variable(string name, string eqn, GraphicalFunction graph, Range range, Range scale) : this(name, eqn)
        {
            Function = graph;
            Range = range;
            Scale = scale;
        }

        #region Xml attributes

        public string Eqn { get; set; }
        public Units Units { get; set; }
        /// <summary>
        /// Input range
        /// </summary>
        public Range Range{ get; set; } = new Range(false);
        /// <summary>
        /// Output scale
        /// </summary>
        public Range Scale { get; set; } = new Range(false);

        #endregion

        public string Name { get; }

        private float _value;

        public float Value
        {
            get => _value;
            set => _value = Scale.GetOutputInsideRange(value);
        }

        public string Equation { get; set; }

        public GraphicalFunction Function { get; set; }

        public float OldValue { get; set; }

        /// <summary>
        ///     The variable has been updated
        /// </summary>
        public bool Updated { get; set; }

        /// <summary>
        ///     The variable is being updating
        /// </summary>
        public bool Updating { get; set; }

        /// <summary>
        ///     Children are items from Equation that are not numbers, operators, blanks nor itself
        /// </summary>
        public List<string> Children { get; protected set; }

        /// <summary>
        ///     Find all children of a variable
        /// </summary>
        /// <returns></returns>
        protected void SetChildren()
        {
            var words = Equation?.Split(' ', '+', '-', '*', '/', '(', ')');

            Children = words?.Where(word => !Simulation.ManagedEquation.Functions.Contains(word) &&
                                            !Simulation.ManagedEquation.Operators.Contains(word) &&
                                            !float.TryParse(word, NumberStyles.Any, CultureInfo.InvariantCulture,
                                                out _) &&
                                            word.Length > 0
                                            && !word.Equals(Name)).ToList();
        }

        public static float CheckInitialValue(string equation)
        {
            if (!float.TryParse(equation, out var n))
            {
                n = 0;
            }

            return n;
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}