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
using System.Text.RegularExpressions;
using Symu.SysDyn.Parser;
using Symu.SysDyn.Simulation;

#endregion

namespace Symu.SysDyn.Equations
{
    /// <summary>
    ///     A built_in function defined by string
    /// </summary>
    public class BuiltInFunction
    {
        public BuiltInFunction(string function)
        {
            Function = function ?? throw new ArgumentNullException(nameof(function));
            Name = StringUtils.CleanName(function.Split('(')[0]);
            Parameters = GetParametersOfFunction(function);
        }

        /// <summary>
        ///     The entire function included brackets and parameters
        /// </summary>
        public string Function { get; }

        /// <summary>
        ///     The function name
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     The list of parameters
        /// </summary>
        public IEnumerable<string> Parameters { get; }

        /// <summary>
        ///     Get the list of parameters of a function
        /// </summary>
        /// <param name="input"></param>
        /// <returns>input = "function(param1, param2)" - return {param1, param2}</returns>
        public static List<string> GetParametersOfFunction(string input)
        {
            // Get function name
            var func = Regex.Match(input, @"\b[^()]+\((.*)\)$");

            return string.IsNullOrEmpty(func.Groups[1].Value) ? 
                new List<string>() : 
                CleanParameters(func.Groups[1].Value.Split(',').ToList());
        }

        public static List<string> CleanParameters(List<string> parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            return parameters.Select(CleanParameter).ToList();
        }

        public static string CleanParameter(string parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            parameter = parameter.ToLowerInvariant();

            // Can have multiple blanks
            while (parameter.Contains(' '))
            {
                parameter = parameter.Replace(StringUtils.Blank, string.Empty);
            }

            return parameter;
        }

        /// <summary>
        ///     Prepare the function for the Equation.Prepare()
        /// </summary>
        /// <param name="word"></param>
        /// <param name="sim"></param>
        /// <returns></returns>
        public virtual string Prepare(string word, SimSpecs sim)
        {
            return word;
        }
    }
}