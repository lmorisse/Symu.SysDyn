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
using Symu.SysDyn.Functions;
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
            Units = Units.CreateInstanceFromEquation(eqn);
            Equation = EquationFactory.CreateInstance(eqn, null, out var value);
            Value = value;
            Initialize();
            SetChildren();
        }

        public Variable(string name, string eqn, GraphicalFunction graph, Range range, Range scale) : this(name)
        {
            _graphicalFunction = graph;
            Range = range;
            Scale = scale;
            Units = Units.CreateInstanceFromEquation(eqn);
            // intentionally after Range assignment
            Equation = EquationFactory.CreateInstance(eqn, range, out var value);
            Value = value;
            Initialize();
            SetChildren();
        }

        private float _value;
        public float Value
        {
            get => _value;
            set => _value = Scale.GetOutputInsideRange(value);
        }

        public IEquation Equation { get; set; }

        private GraphicalFunction _graphicalFunction;

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
        public List<string> Children { get; private set; }

        /// <summary>
        ///     Find all children of a variable
        /// </summary>
        /// <returns></returns>
        protected void SetChildren()
        {
            Children = Equation?.Variables.Where(word => !word.Equals(Name)).ToList() ?? new List<string>();
        }

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

            var eval = Equation.Evaluate(variables, simulation);
            Value = _graphicalFunction?.GetOutputWithBounds(eval) ?? eval;
            Updated = true;
        }

        #region Xml attributes

        public string Name { get; }

        public Units Units { get; set; }

        /// <summary>
        ///     Input range
        /// </summary>
        public Range Range { get; set; } = new Range(false);

        /// <summary>
        ///     Output scale
        /// </summary>
        public Range Scale { get; set; } = new Range(false);

        public bool TryOptimize(bool setInitialValue)
        {
            if (Equation == null)
            {
                return true;
            }
            // No variables or itself
            var canBeOptimized = !Children.Any() 
                                 && (!Equation.Variables.Any() || (Equation.Variables.Count==1 && Equation.Variables[0] == Name));
            if (canBeOptimized && Equation is ComplexEquation complexEquation)
            {
                canBeOptimized = !complexEquation.Functions.Any();
            }
            if (canBeOptimized)
            {
                if (setInitialValue)
                {
                    try
                    {
                        Value = Equation.InitialValue();
                    }
                    catch
                    {
                        // Should be removed
                    }
                }

                Equation = null;
            }

            return canBeOptimized || Equation==null;
        }

        #endregion

        public void Initialize()
        {
            Updated = Equation == null;
        }

        public Variable Clone()
        {
            var clone = new Variable(Name)
            {
                _graphicalFunction = _graphicalFunction,
                Range = Range,
                Scale = Scale,
                Units = Units,
                Equation = Equation,
                Value = Value,
                Children = new List<string>()
            };
            clone.Children.AddRange(Children);
            return clone;
        }
    }
}