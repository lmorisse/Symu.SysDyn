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
using System.Threading.Tasks;
using NCalcAsync;
using Symu.SysDyn.Core.Engine;
using Symu.SysDyn.Core.Equations;
using Symu.SysDyn.Core.Models.XMile;

#endregion

namespace Symu.SysDyn.Core.Functions
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

        public static async Task<T> CreateBuiltInFunction<T>(string model, string function) where T : IBuiltInFunction, new()
        {
            var originalFunction = function?.Trim() ?? throw new ArgumentNullException(nameof(function));
            var builtInFunction =  await FunctionUtils.ParseParameters<T>(model, function);
            builtInFunction.OriginalFunction = originalFunction;
            return builtInFunction;
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
        public List<float> Args { get; set; } = new List<float>();

        #region IBuiltInFunction Members

        /// <summary>
        ///     The entire function included brackets and parameters
        /// </summary>
        public string OriginalFunction { get; set; }

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
        public List<IEquation> Parameters { get; protected set; } = new List<IEquation>();

        public virtual async Task<IBuiltInFunction> Clone() 
        {
            var clone = await CreateBuiltInFunction<BuiltInFunction>(Model, OriginalFunction);
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
        public async Task<float> Prepare(IVariable selfVariable, VariableCollection variables, SimSpecs sim)
        {
            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }

            var prepareParams = new List<string>();
            foreach (var parameter in Parameters.Where(x => x != null))
            {
                await parameter.Prepare(selfVariable, variables, sim);
                prepareParams.AddRange(parameter.Variables);
                // Get Parameters of the functions of the parameter
                // Example : Time - 5 => Store Time value
                foreach (var expressionParameter in parameter.Expression.Parameters)
                {

                    Expression.Parameters[expressionParameter.Key] = expressionParameter.Value;
                }
            }

            foreach (var name in prepareParams.Distinct().ToList())
            {
                var parameter = variables.Get(name);
                if (parameter != null)
                {
                    Expression.Parameters[name] = parameter.Value;
                }
            }

            return await Evaluate(selfVariable, variables, sim);
        }

        public virtual async Task<TryReplaceStruct> TryReplace(SimSpecs sim)
        {
            try
            {
                var result = Convert.ToSingle(await Expression.EvaluateAsync(sim?.Time, sim?.Step, sim?.DeltaTime));
                return new TryReplaceStruct(true, result);
            }
            catch
            {
                return new TryReplaceStruct(false, 0);
            }
        }

        public async Task<TryReplaceStruct>  TryEvaluate(IVariable variable, VariableCollection variables, SimSpecs sim)
        {
            try
            {
                var result = await Evaluate(variable, variables, sim);
                return new TryReplaceStruct(true, result);
            }
            catch
            {
                return new TryReplaceStruct(false, 0);
            }
        }

        public async Task Replace(string child, string value, SimSpecs sim)
        {
            var replace = false;

            for (var index = 0; index < Parameters.Count; index++)
            {
                var parameter = Parameters[index];
                if (parameter == null)
                {
                    continue;
                }

                await parameter.Replace(child, value, sim);
                if (!parameter.CanBeOptimized(child))
                {
                    continue;
                }

                Args[index] = await parameter.InitialValue();
                Parameters[index] = null;
                replace = true;
            }

            //if (child == Dt.Label && Name == child)
            //{
            //    Expression = new Expression(value);
            //    return;
            //}

            if (!replace)
            {
                return;
            }

            InitializedFunction = FunctionUtils.GetInitializedFunction(Name, Parameters, Args);
            Expression = new Expression(InitializedFunction);
        }

        #endregion

        public virtual async Task<float> Evaluate(IVariable selfVariable, VariableCollection variables, SimSpecs sim)
        {
            try
            {
                return Convert.ToSingle(await Expression.EvaluateAsync(sim?.Time, sim?.Step, sim?.DeltaTime));
            }
            catch
            {
                throw;
            } 
        }

        protected void CopyTo(IBuiltInFunction copy)
        {
            if (copy == null)
            {
                throw new ArgumentNullException(nameof(copy));
            }

            copy.IndexName = IndexName;
        }

        protected async Task<float> GetValue(int index, IVariable variable, VariableCollection variables, SimSpecs sim)
        {
            return Parameters[index] != null ? await Parameters[index].Evaluate(variable, variables, sim) : Args[index];
        }

        protected string GetParam(int index)
        {
            return Parameters[index] != null
                ? Parameters[index].InitializedEquation
                : Args[index].ToString(CultureInfo.InvariantCulture);
        }
    }
}