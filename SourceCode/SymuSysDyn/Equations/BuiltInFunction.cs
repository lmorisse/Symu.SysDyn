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
using Symu.SysDyn.Model;
using Symu.SysDyn.Parser;
using Symu.SysDyn.Simulation;

#endregion

namespace Symu.SysDyn.Equations
{
    /// <summary>
    ///     A built in function defined by string
    ///     Works with nested functions, i.e. if the parameters of the function are functions
    /// </summary>
    /// <remarks>https://www.simulistics.com/help/equations/builtin.htm</remarks>
    public class BuiltInFunction
    {        
        public BuiltInFunction() { }

        public BuiltInFunction(string function)
        {
            OriginalFunction = function ?? throw new ArgumentNullException(nameof(function));
            Name = StringUtils.CleanName(function.Split('(')[0]);
            Parameters = GetParameters(function);
            Expression = new Expression(SetCleanedFunction());
        }

        public string SetCleanedFunction()
        {
            var cleanedFunction = Name + StringUtils.LParenthesis;
            for (var i = 0; i < Parameters.Count; i++)
            {
                cleanedFunction += Parameters[i];
                if (i < Parameters.Count - 1)
                {
                    cleanedFunction += StringUtils.Comma;
                }
            }
            cleanedFunction += StringUtils.RParenthesis;
            return cleanedFunction;
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
        public string Name { get; protected set; }
        /// <summary>
        ///     The function name indexed 
        /// </summary>
        public string IndexName { get; set; }

        public List<Equation> Parameters { get; protected set; }

        /// <summary>
        ///     Get the list of parameters of a function with nested functions
        /// </summary>
        /// <param name="input"></param>
        /// <returns>input = "function(func(param1, param2), param3)" - return {func(param1, param2), param3}</returns>
        public static List<Equation> GetParameters(string input)
        {
            var result = new List<Equation>();
            const string extractFuncRegex = @"\b[^()]+\((.*)\)$";
            const string extractArgsRegex = @"(?:[^,()]+((?:\((?>[^()]+|\((?<open>)|\)(?<-open>))*\)))*)+";

            var parameters = Regex.Match(input, extractFuncRegex);

            if (string.IsNullOrEmpty(parameters.Groups[1].Value))
            {
                return result;
            }

            var innerArgs = parameters.Groups[1].Value;
            var matches = Regex.Matches(innerArgs, extractArgsRegex);
            for (var i = 0; i < matches.Count; i++)
            {
                result.Add(new Equation(matches[i].Value));
            }

            return result;
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
            foreach (var parameter in Parameters)
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
    }
}