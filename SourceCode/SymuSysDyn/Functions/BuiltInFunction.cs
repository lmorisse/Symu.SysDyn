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
using System.Text.RegularExpressions;
using NCalc2;
using Symu.SysDyn.Equations;
using Symu.SysDyn.Model;
using Symu.SysDyn.Parser;
using Symu.SysDyn.Simulation;

#endregion

namespace Symu.SysDyn.Functions
{
    /// <summary>
    ///     A built in function defined by string
    ///     Works with nested functions, i.e. if the parameters of the function are functions
    /// </summary>
    /// <remarks>When adding a new buildIn function,
    /// create an inherited class of BuiltInFunction
    /// add it in FunctionUtils.ParseStringFunctions
    /// add a unit test class for the function
    /// add a unit test in AllBuiltInFUnctionsTests to have the list of all available functions</remarks>
    /// <remarks>https://www.simulistics.com/help/equations/builtin.htm</remarks>
    public class BuiltInFunction
    {        
        public BuiltInFunction() { }

        public BuiltInFunction(string function)
        {
            OriginalFunction = function?.Trim() ?? throw new ArgumentNullException(nameof(function));
            FunctionUtils.ParseParameters(ref function, out var name, out var parameters, out var args);
            Name = name;
            Parameters = parameters;
            Args = args;
            Expression = new Expression(function);
        }

        /// <summary>
        ///     The entire function included brackets and parameters
        /// </summary>
        public string OriginalFunction { get; protected set; }

        /// <summary>
        ///     The entire cleaned function ready to be evaluated
        /// </summary>
        public Expression Expression { get; set; }

        /// <summary>
        ///     The function name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        ///     The function name indexed 
        /// </summary>
        public string IndexName { get; set; }
        /// <summary>
        /// List of arguments that are IEquation or null if it is a constant
        /// If it is a constant, the value is stored in Args
        /// </summary>
        public List<IEquation> Parameters { get; protected set; }
        /// <summary>
        /// List of arguments that are constants
        /// If it is an IEquation, the value is stored in Parameters
        /// </summary>
        public List<float> Args { get; protected set; }

        protected float GetValue(int index, Variables variables, SimSpecs sim)
        {
            return Parameters[index] != null ? Parameters[index].Evaluate(variables, sim) : Args[index];
        }

        protected string GetParam(int index)
        {
            return Parameters[index] != null ? Parameters[index].InitializedEquation : Args[index].ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     Prepare the function for the Equation.Prepare()
        /// </summary>
        /// <param name="variables"></param>
        /// <param name="sim"></param>
        /// <returns></returns>
        public float Prepare(Variables variables, SimSpecs sim)
        {
            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }

            var prepareParams = new List<string>();
            foreach (var parameter in Parameters.Where(x => x != null))
            {
                parameter.Prepare(variables, sim);
                prepareParams.AddRange(parameter.Variables);
            }

            foreach (var name in prepareParams.Distinct().ToList())
            {
                var variable = variables.Get(name);
                if (variable != null)
                {
                    Expression.Parameters[name] = variable.Value;
                }
            }

            return Evaluate(variables, sim);
        }

        public virtual float Evaluate(Variables variables, SimSpecs sim)
        {
            return Convert.ToSingle(Expression.Evaluate());
        }

        public bool TryEvaluate(Variables variables, SimSpecs sim, out float result)
        {
            try
            {
                result = Evaluate(variables, sim);
                return true;
            }
            catch
            {
                result = 0;
                return false;
            }
        }
    }
}