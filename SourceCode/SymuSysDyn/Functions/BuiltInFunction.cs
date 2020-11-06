#region Licence

// Description: SymuBiz - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NCalc2;
using Symu.SysDyn.Engine;
using Symu.SysDyn.Equations;
using Symu.SysDyn.Models;

#endregion

namespace Symu.SysDyn.Functions
{
    /// <summary>
    ///     Default implementation of IBuiltInFunction
    ///     A built in function defined by string
    ///     Works with nested functions, i.e. if the parameters of the function are functions
    /// </summary>
    /// <remarks>
    ///     When adding a new buildIn function,
    ///     create an inherited class of BuiltInFunction
    ///     add it in FunctionFactory
    ///     add a unit test class for the function
    ///     add a unit test in AllBuiltInFUnctionsTests to have the list of all available functions
    /// </remarks>
    /// <remarks>https://www.simulistics.com/help/equations/builtin.htm</remarks>
    public class BuiltInFunction : IBuiltInFunction
    {
        public BuiltInFunction()
        {
        }

        public BuiltInFunction(string model, string function)
        {
            OriginalFunction = function?.Trim() ?? throw new ArgumentNullException(nameof(function));
            FunctionUtils.ParseParameters(model, ref function, out var name, out var parameters, out var args);
            Name = name;
            Model = model ?? throw new ArgumentNullException(nameof(model));
            Parameters = parameters;
            Args = args;
            InitializedFunction = function;
            Expression = new Expression(function);
        }

        public string InitializedFunction { get; set; }

        /// <summary>
        ///     The entire cleaned function ready to be evaluated
        /// </summary>
        public Expression Expression { get; set; }

        /// <summary>
        ///     The model name
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        ///     List of arguments that are constants
        ///     If it is an IEquation, the value is stored in Parameters
        /// </summary>
        public List<float> Args { get; protected set; }

        #region IBuiltInFunction Members

        /// <summary>
        ///     The entire function included brackets and parameters
        /// </summary>
        public string OriginalFunction { get; protected set; }

        /// <summary>
        ///     The function name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     The function name indexed
        /// </summary>
        public string IndexName { get; set; }

        /// <summary>
        ///     List of arguments that are IEquation or null if it is a constant
        ///     If it is a constant, the value is stored in Args
        /// </summary>
        public List<IEquation> Parameters { get; protected set; }

        public virtual IBuiltInFunction Clone()
        {
            var clone = new BuiltInFunction(Model, OriginalFunction);
            CopyTo(clone);
            return clone;
        }

        /// <summary>
        ///     Prepare the function for the Equation.Prepare()
        /// </summary>
        /// <param name="selfVariable"></param>
        /// <param name="variables"></param>
        /// <param name="sim"></param>
        /// <returns></returns>
        public float Prepare(IVariable selfVariable, VariableCollection variables, SimSpecs sim)
        {
            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }

            var prepareParams = new List<string>();
            foreach (var parameter in Parameters.Where(x => x != null))
            {
                parameter.Prepare(selfVariable, variables, sim);
                prepareParams.AddRange(parameter.Variables);
            }

            foreach (var name in prepareParams.Distinct().ToList())
            {
                var parameter = variables.Get(name);
                if (parameter != null)
                {
                    Expression.Parameters[name] = parameter.Value;
                }
            }

            return Evaluate(selfVariable, variables, sim);
        }

        public virtual float InitialValue(SimSpecs sim)
        {
            return Convert.ToSingle(Expression.Evaluate());
        }

        public bool TryEvaluate(IVariable variable, VariableCollection variables, SimSpecs sim, out float result)
        {
            try
            {
                result = Evaluate(variable, variables, sim);
                return true;
            }
            catch
            {
                result = 0;
                return false;
            }
        }

        public void Replace(string child, string value, SimSpecs sim)
        {
            var replace = false;

            for (var index = 0; index < Parameters.Count; index++)
            {
                var parameter = Parameters[index];
                if (parameter == null)
                {
                    continue;
                }

                parameter.Replace(child, value, sim);
                if (!parameter.CanBeOptimized(child))
                {
                    continue;
                }

                Args[index] = parameter.InitialValue();
                Parameters[index] = null;
                replace = true;
            }

            if (child == Dt.Value && Name == child)
            {
                Expression = new Expression(value);
                return;
            }

            if (!replace)
            {
                return;
            }

            InitializedFunction = FunctionUtils.GetInitializedFunction(Name, Parameters, Args);
            Expression = new Expression(InitializedFunction);
        }

        #endregion

        public virtual float Evaluate(IVariable selfVariable, VariableCollection variables, SimSpecs sim)
        {
            return InitialValue(sim);
        }

        protected void CopyTo(IBuiltInFunction copy)
        {
            if (copy == null)
            {
                throw new ArgumentNullException(nameof(copy));
            }

            copy.IndexName = IndexName;
        }

        protected float GetValue(int index, IVariable variable, VariableCollection variables, SimSpecs sim)
        {
            return Parameters[index] != null ? Parameters[index].Evaluate(variable, variables, sim) : Args[index];
        }

        protected string GetParam(int index)
        {
            return Parameters[index] != null
                ? Parameters[index].InitializedEquation
                : Args[index].ToString(CultureInfo.InvariantCulture);
        }
    }
}