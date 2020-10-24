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
            Equation = EquationFactory.CreateInstance(eqn, null);
            // intentionally after EquationFactory
            SetInitialValue();
            SetChildren();
        }

        public Variable(string name, string eqn, GraphicalFunction graph, Range range, Range scale) : this(name)
        {
            _graphicalFunction = graph;
            Range = range;
            Scale = scale;
            Units = Units.CreateInstanceFromEquation(eqn);
            // intentionally after Range assignment
            Equation = EquationFactory.CreateInstance(eqn, range);
            // intentionally after EquationFactory
            SetInitialValue();
            SetChildren();
        }

        public void SetInitialValue()
        {
            Value = Equation == null ? 0 : SetInitialValue(Equation.OriginalEquation);
            Initialize();
        }

        private float _value;

        public float Value
        {
            get => _value;
            set => _value = Scale.GetOutputInsideRange(value);
        }

        public IEquation Equation { get; set; }

        private readonly GraphicalFunction _graphicalFunction;

        /// <summary>
        ///     The variable has been updated
        /// </summary>
        public bool Updated { get; private set; }

        /// <summary>
        ///     The variable is being updating
        /// </summary>
        public bool Updating { get; set; }

        /// <summary>
        ///     Children are items from Equation that are not numbers, operators, blanks nor itself
        /// </summary>
        public List<string> Children { get; private set; }

        /// <summary>
        ///     Find all children of a variable
        /// </summary>
        /// <returns></returns>
        protected void SetChildren()
        {
            Children = Equation?.Variables.Where(word => !word.Equals(Name)).ToList() ?? new List<string>();
        }

        public float SetInitialValue(string stringEquation)
        {
            switch (Equation)
            {
                case ConstantEquation constant:
                    return constant.Value;
                case ComplexEquation _:
                    try
                    {
                        // Value may be the initial value of the equation
                        var eval = EquationFactory.CreateInstance(stringEquation)?.Evaluate(new Variables(), new SimSpecs()) ?? 0;
                        if (float.IsNaN(eval) || float.IsInfinity(eval))
                        {
                            eval = 0;
                        }
                        return eval;
                    }
                    catch
                    {
                        // The equation contains variables, it can be evaluate
                        return 0;
                    }
                default:
                    // SimpleEquation are made of variables, there is no initial value
                    return 0;
            }
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
            //if (float.IsNaN(eval) || float.IsInfinity(eval))
            //{
            //    //eval = 0;
            //    throw new DivideByZeroException();
            //}
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

        #endregion

        public void Initialize()
        {
            Updated = Equation == null || Equation is ConstantEquation;
        }
    }
}