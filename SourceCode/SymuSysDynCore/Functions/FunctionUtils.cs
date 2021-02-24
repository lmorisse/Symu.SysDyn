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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using NCalcAsync;

using Symu.SysDyn.Core.Equations;
using Symu.SysDyn.Core.Parser;

#endregion

namespace Symu.SysDyn.Core.Functions
{
    /// <summary>
    ///     Utils method to parse string for functions and theirs parameters
    /// </summary>
    public static class FunctionUtils
    {
        /// <summary>
        ///     Functions Without Brackets ni lowercase
        /// </summary>
        public static readonly List<string> FunctionsWithoutBrackets =
            new List<string> {"dt", "time", "externalupdate"};

        /// <summary>
        ///     Special names that are not functions as is
        /// </summary>
        public static readonly List<string> NotFunctions = new List<string> {"if("};
        /// <summary>
        ///     Functions in NCalcAsync but Time dependent
        /// </summary>
        public static readonly List<string> FunctionsTimeDependent =
            new List<string> { "step"};

        /// <summary>
        ///     Extract functions from a string as a list of BuiltIn functions
        /// </summary>
        /// <param name="model"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static async Task<List<IBuiltInFunction>> ParseFunctions(string model, string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var builtInFunctions = new List<IBuiltInFunction>();

            var functions = ParseStringFunctions(input);

            if (functions != null)
            {
                foreach (var function in functions)
                {
                    var factory = await FunctionFactory.CreateInstance(model, function);
                    if (factory != null)
                    {
                        builtInFunctions.Add(factory);
                    }
                }
            }

            if (IfThenElse.IsContainedIn(input))
            {
                builtInFunctions.Add(await IfThenElse.CreateIfThenElse(model, input));
            }

            return builtInFunctions;
        }

        /// <summary>
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks>No regex approach for maintainability reason</remarks>
        /// <code>var regex = new Regex(@"([a-zA-Z0-9]+)\s*\([^)]*\)");</code>
        public static List<string> ParseStringFunctions(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            var result = new List<string>();
            // replace functions without brackets 
            //([^a-zA-Z0-9_]*)TIME.*?([^a-zA-Z0-9_] *)
       
                   var name = string.Empty;
            var counter = 0;
            var isFunction = true;
            foreach (var split in input.ToCharArray())
            {
                switch (split)
                {
                    case ' ':
                    case '-':
                    case '+':
                    case '*':
                    case '/':
                        if (counter == 0)
                        {
                            if (FunctionsWithoutBrackets.Contains(name.ToLowerInvariant()))
                            {
                                result.Add(name);
                            }

                            name = string.Empty;
                        }
                        else if (isFunction)
                        {
                            name += split;
                        }
                        else
                        {
                            name = string.Empty;
                        }

                        break;
                    case '(':
                        isFunction = !string.IsNullOrEmpty(name);
                        if (isFunction)
                        {
                            name += split;
                        }

                        counter++;
                        break;
                    case ')':
                        counter--;
                        if (isFunction)
                        {
                            name += split;
                            if (counter == 0 && !NotFunctions.Exists(x => name.ToLowerInvariant().StartsWith(x)))
                            {
                                result.Add(name);
                                name = string.Empty;
                            }
                        }
                        else if (counter == 0)
                        {
                            isFunction = true;
                        }

                        break;
                    default:
                        if (isFunction)
                        {
                            name += split;
                        }

                        break;
                }
            }

            // Case of a single BuiltInFunction without brackets
            if (FunctionsWithoutBrackets.Contains(name.ToLowerInvariant()))
            {
                result.Add(name);
            }

            return result;
        }

        /// <summary>
        ///     Get the list of parameters/args of a function with nested functions
        /// </summary>
        /// <param name="model"></param>
        /// <param name="function"></param>
        /// <returns>input = "function(func(param1, param2), param3)" - return {func(param1, param2), param3}</returns>
        public static async Task<T> ParseParameters<T>(string model, string function) where T : IBuiltInFunction, new ()
        {
            if (string.IsNullOrEmpty(function))
            {
                throw new ArgumentNullException(nameof(function));
            }

            var builtInFunction = new T {Name = StringUtils.CleanName(function.Split('(')[0])};
            const string extractFuncRegex = @"\b[^()]+\((.*)\)$";
            const string extractArgsRegex = @"(?:[^,()]+((?:\((?>[^()]+|\((?<open>)|\)(?<-open>))*\)))*)+";

            var match = Regex.Match(function, extractFuncRegex);

            if (string.IsNullOrEmpty(match.Groups[1].Value))
            {
                return builtInFunction; // parameters;
            }

            var innerArgs = match.Groups[1].Value;
            var matches = Regex.Matches(innerArgs, extractArgsRegex);
            for (var i = 0; i < matches.Count; i++)
            {
                var factory = await EquationFactory.CreateInstance(model, matches[i].Value);
                builtInFunction.Parameters.Add(factory.Equation);
                builtInFunction.Args.Add(factory.Value);
            }

            function = GetInitializedFunction(builtInFunction.Name, builtInFunction.Parameters, builtInFunction.Args);


            builtInFunction.InitializedFunction = function;
            builtInFunction.Expression = new Expression(function);

            builtInFunction.Model = model ?? throw new ArgumentNullException(nameof(model));
            return builtInFunction;
        }

        public static string GetInitializedFunction(string name, List<IEquation> parameters, List<float> args)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            //todo refactor in directly in NCalc2 Fork
            if (name == "If")
            {
                name = "if";
            }

            var initializedFunction = name + "(";
            for (var i = 0; i < parameters.Count; i++)
            {
                if (parameters[i] != null)
                {
                    initializedFunction += parameters[i];
                }
                else
                {
                    initializedFunction += args[i].ToString(CultureInfo.InvariantCulture);
                }

                if (i < parameters.Count - 1)
                {
                    initializedFunction += ",";
                }
            }

            initializedFunction += ")";
            return initializedFunction;
        }
    }
}