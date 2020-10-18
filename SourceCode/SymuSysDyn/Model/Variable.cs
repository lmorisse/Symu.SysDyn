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
using Symu.SysDyn.Equations;
using Symu.SysDyn.Parser;

#endregion

namespace Symu.SysDyn.Model
{
    /// <summary>
    ///     Base class for the variable of the model
    /// </summary>
    public class Variable
    {
        private float _value;

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
            Equation = new ManagedEquation(eqn);
            Units = Units.CreateInstanceFromEquation(eqn);
            SetChildren();
        }

        public Variable(string name, string eqn, GraphicalFunction graph, Range range, Range scale) : this(name)
        {
            Value = CheckInitialValue(eqn);
            Units = Units.CreateInstanceFromEquation(eqn);
            Function = graph;
            Range = range;
            Scale = scale;
            // intentionally after Range assignment
            Equation = new ManagedEquation(eqn, range);
            SetChildren();
        }

        public float Value
        {
            get => _value;
            set => _value = Scale.GetOutputInsideRange(value);
        }

        public ManagedEquation Equation { get; set; }

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
            Children = Equation?.GetVariables()?.Where(word => !word.Equals(Name)).ToList() ?? new List<string>();
        }

        public static float CheckInitialValue(string equation)
        {
            if (string.IsNullOrEmpty(equation))
            {
                return 0;
            }

            // Remove string in Braces which are units not equation
            var index = equation.IndexOf('{');
            if (index > 0)
            {
                equation = equation.Remove(index);
            }

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

        #region Xml attributes

        public string Name { get; }

        public string Eqn { get; set; }
        public Units Units { get; set; }

        /// <summary>
        ///     Input range
        /// </summary>
        public Range Range { get; set; } = new Range(false);

        /// <summary>
        ///     Output scale
        /// </summary>
        public Range Scale { get; set; } = new Range(false);

        #endregion
    }
}