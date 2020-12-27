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
using Symu.SysDyn.Equations;
using Symu.SysDyn.Parser;

#endregion

namespace Symu.SysDyn.Functions
{
    /// <summary>
    ///     Utils method to parse string for functions and theirs parameters
    /// </summary>
    public static class FunctionUtils
    {
        /// <summary>
        ///     Functions Without Brackets ni lowercase
        /// </summary>
        private static readonly List<string> FunctionsWithoutBrackets =
            new List<string> {"dt", "time", "externalupdate"};

        /// <summary>
        ///     Special names that are not functions as is
        /// </summary>
        private static readonly List<string> NotFunctions = new List<string> {"if("};

        /// <summary>
        ///     Extract functions from a string as a list of BuiltIn functions
        /// </summary>
        /// <param name="model"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static IEnumerable<IBuiltInFunction> ParseFunctions(string model, string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var builtInFunctions = new List<IBuiltInFunction>();

            var functions = ParseStringFunctions(input);

            if (functions != null)
            {
                builtInFunctions.AddRange(functions.Select(function =>
                    FunctionFactory.CreateInstance(model, function)));
            }

            if (IfThenElse.IsContainedIn(input))
            {
                builtInFunctions.Add(new IfThenElse(model, input));
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
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <param name="args"></param>
        /// <returns>input = "function(func(param1, param2), param3)" - return {func(param1, param2), param3}</returns>
        //public static List<IEquation> ParseParameters(ref string function,  out string name)
        public static void ParseParameters(string model, ref string function, out string name,
            out List<IEquation> parameters,
            out List<float> args)
        {
            if (function == null)
            {
                throw new ArgumentNullException(nameof(function));
            }

            parameters = new List<IEquation>();
            args = new List<float>();
            name = StringUtils.CleanName(function.Split('(')[0]);
            const string extractFuncRegex = @"\b[^()]+\((.*)\)$";
            const string extractArgsRegex = @"(?:[^,()]+((?:\((?>[^()]+|\((?<open>)|\)(?<-open>))*\)))*)+";

            var match = Regex.Match(function, extractFuncRegex);

            if (string.IsNullOrEmpty(match.Groups[1].Value))
            {
                return; // parameters;
            }

            var innerArgs = match.Groups[1].Value;
            var matches = Regex.Matches(innerArgs, extractArgsRegex);
            for (var i = 0; i < matches.Count; i++)
            {
                var equation = EquationFactory.CreateInstance(model, matches[i].Value, out var value);
                parameters.Add(equation);
                args.Add(value);
            }

            function = GetInitializedFunction(name, parameters, args);
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