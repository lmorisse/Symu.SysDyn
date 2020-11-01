#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Symu.SysDyn.Equations;
using Symu.SysDyn.Parser;
using Symu.SysDyn.Simulation;

#endregion

namespace Symu.SysDyn.Model
{
    /// <summary>
    ///     Default implementation of IVariable
    /// </summary>
    public class Variable : IVariable
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
            Units = Units.CreateInstanceFromEquation(eqn);
            Equation = EquationFactory.CreateInstance(eqn, null, out var value);
            Value = value;
            NonNegative = new NonNegative(false);
            Initialize();
            SetChildren();
        }
        public static IVariable CreateInstance(Variables variables, string name, string eqn)
        {
            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }

            var variable = new Variable(name, eqn);
            variables.Add(variable);
            return variable;
        }

        public Variable(string name, string eqn, GraphicalFunction graph, Range range, Range scale,
            NonNegative nonNegative) : this(name)
        {
            GraphicalFunction = graph;
            Range = range;
            Scale = scale;
            Units = Units.CreateInstanceFromEquation(eqn);
            Equation = EquationFactory.CreateInstance(eqn, range, out var value);
            NonNegative = nonNegative;
            AdjustValue(value);
            Initialize();
            SetChildren();
        }

        #region IVariable Members

        public float Value { get; set; }

        public IEquation Equation { get; set; }

        public GraphicalFunction GraphicalFunction { get; set; }

        /// <summary>
        ///     The variable has been updated
        /// </summary>
        public bool Updated { get; set; }

        /// <summary>
        ///     The variable is being updating
        /// </summary>
        public bool Updating { get; set; }

        /// <summary>
        ///     Children are Equation's Variables except itself
        /// </summary>
        /// <remarks>Could be a computed property, but for performance, it is setted once</remarks>
        public List<string> Children { get; set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return Name;
        }

        public void Update(Variables variables, SimSpecs simulation)
        {
            if (Updated)
            {
                return;
            }

            var eval = Equation.Evaluate(this, variables, simulation);
            AdjustValue(eval);
            Updated = true;
        }

        public void Initialize()
        {
            Updated = Equation == null;
        }

        public virtual IVariable Clone()
        {
            var clone = new Variable(Name);
            CopyTo(clone);
            return clone;
        }

        #endregion

        /// <summary>
        ///     Adjust Value when a graphical function is defined
        /// </summary>
        /// <param name="value"></param>
        protected void AdjustValue(float value)
        {
            if (float.IsNaN(value) || float.IsInfinity(value))
            {
                throw new ArgumentOutOfRangeException();
            }

            // Graphical function
            Value = GraphicalFunction?.GetOutput(value) ?? value;
            if (NonNegative != null)
            {
                Value = NonNegative.GetOutputInsideRange(Value);
            }
        }

        /// <summary>
        ///     Find all children of a variable
        /// </summary>
        /// <returns></returns>
        protected void SetChildren()
        {
            Children = Equation?.Variables.Where(word => !word.Equals(Name)).ToList() ?? new List<string>();
        }

        protected void CopyTo(IVariable copy)
        {
            if (copy == null)
            {
                throw new ArgumentNullException(nameof(copy));
            }

            copy.GraphicalFunction = GraphicalFunction;
            copy.Range = Range;
            copy.Scale = Scale;
            copy.Units = Units;
            copy.Equation = Equation?.Clone();
            copy.Value = Value;
            copy.Children = new List<string>();
            copy.Children.AddRange(Children);
        }

        #region Xml attributes

        public string Name { get; }

        public Units Units { get; set; }

        /// <summary>
        ///     Input range
        /// </summary>
        public Range Range { get; set; } = new Range();

        /// <summary>
        ///     Output scale
        /// </summary>
        public Range Scale { get; set; } = new Range();

        public NonNegative NonNegative { get; }

        public bool TryOptimize(bool setInitialValue)
        {
            if (Equation == null)
            {
                return true;
            }

            // No variables or itself
            var canBeOptimized = !Children.Any() && Equation.CanBeOptimized(Name);

            if (!canBeOptimized)
            {
                return Equation == null;
            }

            if (setInitialValue)
            {
                if (Equation.Variables.Count == 1 && Equation.Variables[0] == Name)
                {
                    Equation.Replace(Name, Value.ToString(CultureInfo.InvariantCulture));
                }

                var value = Equation.InitialValue();
                AdjustValue(value);
            }

            Equation = null;

            return true;
        }

        #endregion
    }
}