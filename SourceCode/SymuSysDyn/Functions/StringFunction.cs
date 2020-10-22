#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

#endregion

#region using directives

#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using Symu.SysDyn.Parser;

namespace Symu.SysDyn.Functions
{
    //todo Maybe try a framework like https://github.com/IronyProject to have a real grammar more than regex
    public static class StringFunction
    {


        /// <summary>
        /// Extract functions from a string as a list of BuiltIn functions
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static IEnumerable<BuiltInFunction> GetFunctions(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }
            var builtInFunctions = new List<BuiltInFunction>();

            #region functions with ()
            var functions = GetStringFunctions(input);

            if (functions != null)
            {
                foreach (var function in functions)
                {
                    var name = StringUtils.CleanName(function.Split('(')[0]);
                    switch (name)
                    {
                        // May be followed by () and then considered as a default builtin function
                        case "If": 
                        case "Then":
                        case "Else":
                            break;
                        case Step.Value:
                            builtInFunctions.Add(new Step(function));
                            break;
                        case Normal.Value:
                            builtInFunctions.Add(new Normal(function));
                            break;
                        case Ramp.Value:
                            builtInFunctions.Add(new Ramp(function));
                            break;
                        case Smth1.Value:
                            builtInFunctions.Add(new Smth1(function));
                            break;
                        case Smth3.Value:
                            builtInFunctions.Add(new Smth3(function));
                            break;
                        case SmthN.Value:
                            builtInFunctions.Add(new SmthN(function));
                            break;
                        case Dt.Value:
                            builtInFunctions.Add(new Dt(function));
                            break;
                        case Time.Value:
                            builtInFunctions.Add(new Time(function));
                            break;
                        default:
                            builtInFunctions.Add(new BuiltInFunction(function));
                            break;
                    }
                }
            }
            #endregion

            #region functions without ()
            if (IfThenElse.IsContainedIn(input))
            {
                builtInFunctions.Add(new IfThenElse(input));
            }

            //if (Dt.IsContainedIn(input, out var dt))
            //{
            //    builtInFunctions.Add(new Dt(dt));
            //}

            //if (Time.IsContainedIn(input, out var time))
            //{
            //    builtInFunctions.Add(new Time(time));
            //}
            #endregion

            return builtInFunctions;
        }
        /// <summary>
        /// Functions Without Brackets ni lowercase
        /// </summary>
        private static readonly List<string> FunctionsWithoutBrackets= new List<string> {"dt","time"};
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <remarks>No regex approach for maintainability reason</remarks>
        /// <code>var regex = new Regex(@"([a-zA-Z0-9]+)\s*\([^)]*\)");</code>
        public static List<string> GetStringFunctions(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }
            var result = new List<string>();
            
            var name = string.Empty;
            var counter = 0;
            var isFunction=true;
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
                            if (counter == 0)
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
            return result;
        }

        /// <summary>
        ///     Get the list of parameters of a function with nested functions
        /// </summary>
        /// <param name="input"></param>
        /// <returns>input = "function(func(param1, param2), param3)" - return {func(param1, param2), param3}</returns>
        public static List<IEquation> GetParameters(string input)
        {
            var result = new List<IEquation>();
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
                result.Add(Equation.CreateInstance(matches[i].Value));
            }

            return result;
        }
    }
}