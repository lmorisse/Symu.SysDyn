#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion
#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using Symu.SysDyn.Engine;
using Symu.SysDyn.Equations;
using Symu.SysDyn.Parser;

#endregion

namespace Symu.SysDyn.Models
{
    /// <summary>
    ///     Default implementation of IVariable
    /// </summary>
    public class Variable : IVariable
    {
        /// <summary>
        /// Constructor for root model
        /// </summary>
        /// <param name="name"></param>
        public Variable(string name): this(name, string.Empty)
        {

        }

        public Variable(string name, string model)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            Model = model;
            Name = StringUtils.CleanName(name);
            FullName = StringUtils.FullName(Model, Name);
        }

        public Variable(string name, string model, string eqn) : this(name, model)
        {
            Units = Units.CreateInstanceFromEquation(eqn);
            Equation = EquationFactory.CreateInstance(Model, eqn, null, out var value);
            Value = value;
            NonNegative = new NonNegative(false);
            Initialize();
            SetChildren();
        }
        public static IVariable CreateInstance(string name, Model model, string eqn)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var variable = new Variable(name, model.Name, eqn);
            model.Variables.Add(variable);
            return variable;
        }

        public Variable(string name, string model, string eqn, GraphicalFunction graph, Range range, Range scale,
            NonNegative nonNegative, VariableAccess access) : this(name, model)
        {
            GraphicalFunction = graph;
            Range = range;
            Scale = scale;
            Units = Units.CreateInstanceFromEquation(eqn);
            Equation = EquationFactory.CreateInstance(Model, eqn, range, out var value);
            NonNegative = nonNegative;
            Access = access;
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
            return FullName;
        }

        public void Update(VariableCollection variables, SimSpecs simulation)
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
            var clone = new Variable(Name, Model);
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
        ///     Except itself
        /// </summary>
        /// <returns></returns>
        protected void SetChildren()
        {
            Children = Equation?.Variables.Where(x => x != FullName).ToList() ?? new List<string>();
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
            copy.NonNegative = NonNegative;
            copy.Access = Access;
            copy.Children = new List<string>();
            copy.Children.AddRange(Children);
        }

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return obj is Variable variable
                   && variable.FullName == FullName;
        }        
        public bool Equals(string fullName)
        {
            return fullName == FullName;
        }

        public bool TryOptimize(bool setInitialValue, SimSpecs sim)
        {
            if (Equation == null)
            {
                return true;
            }

            // No variables or itself
            var canBeOptimized = !Children.Any() && Equation.CanBeOptimized(FullName);

            if (!canBeOptimized)
            {
                return Equation == null;
            }

            if (setInitialValue)
            {
                if (Equation.Variables.Count == 1 && Equation.Variables[0] == FullName)
                {
                    Equation.Replace(FullName, Value.ToString(CultureInfo.InvariantCulture), sim);
                }

                var value = Equation.InitialValue();
                AdjustValue(value);
            }

            Equation = null;

            return true;
        }

        /// <summary>
        /// Model name
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Module and Connect are using FullName = ModelName.VariableName
        /// </summary>
        public string FullName { get; set; }

        #region Xml attributes

        public string Name { get; protected set; }

        public Units Units { get; set; }

        /// <summary>
        ///     Input range
        /// </summary>
        public Range Range { get; set; } = new Range();

        /// <summary>
        ///     Output scale
        /// </summary>
        public Range Scale { get; set; } = new Range();

        public NonNegative NonNegative { get; set; }

        public VariableAccess Access { get; set; }

        #endregion
    }
}