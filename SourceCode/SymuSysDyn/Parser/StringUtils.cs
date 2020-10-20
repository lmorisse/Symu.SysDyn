#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Symu.SysDyn.Equations;

#endregion

namespace Symu.SysDyn.Parser
{
    //todo Maybe try a framework like https://github.com/IronyProject to have a real grammar more than regex
    public static class StringUtils
    {
        public const string Comma = ",";
        public const string LParenthesis = "(";
        public const string RParenthesis = ")";
        public const string Blank = " ";

        #region Functions and parameters
        /// <summary>
        /// Extract functions from a string as a list of BuiltIn functions
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //TODO use NCalc2 or Antlr methods?
        public static IEnumerable<BuiltInFunction> GetStringFunctions(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }
            var builtInFunctions = new List<BuiltInFunction>();

            var regex = new Regex(@"([a-zA-Z0-9]*)\s*\([^()]*\)");
            var functions = regex.Matches(input);

            if (functions.Count > 0)
            {
                foreach (var func in functions)
                {
                    var function = func.ToString().Trim();

                    var name = CleanName(function.Split('(')[0]);

                    if (string.IsNullOrEmpty(name))
                    {
                        continue;
                    }

                    switch (name)
                    {
                        case "If":
                        case "Then":
                        case "Else":
                            break;
                        case Step.Value:
                            builtInFunctions.Add(new Step(function));
                            break;
                        default:
                            builtInFunctions.Add(new BuiltInFunction(function));
                            break;
                    }
                }
            }

            if (IfThenElse.IsContainedIn(input))
            {
                builtInFunctions.Add(new IfThenElse(input));
            }

            if (Dt.IsContainedIn(input))
            {
                builtInFunctions.Add(new Dt());
            }

            if (Time.IsContainedIn(input))
            {
                builtInFunctions.Add(new Time());
            }

            return builtInFunctions;
        }

        #endregion

        #region Names

        public static List<string> CleanNames(List<string> names)
        {
            if (names == null)
            {
                throw new ArgumentNullException(nameof(names));
            }

            return names.Select(CleanName).ToList();
        }

        public static string CleanName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            // useful for function's parameters
            name = name.Trim();
            name = name.ToLowerInvariant();

            // Can have multiple blanks
            while (name.Contains(' '))
            {
                name = name.Replace(' ', '_');
            }

            name = name.Replace("\n", "_");
            name = name.Replace("\\n", "_");
            name = name.Replace("\r", "");
            // Can have multiple underscores
            while (name.Contains("__"))
            {
                name = name.Replace("__", "_");
            }

            return FirstCharToUpper(name);
        }

        private static string FirstCharToUpper(string input)
        {
            switch (input)
            {
                case null: throw new ArgumentNullException(nameof(input));
                case "": return string.Empty;
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }

        /// <summary>
        ///     Get the string in braces
        /// </summary>
        /// <param name="input"></param>
        /// <returns>input = "xxx {stringInBraces} yyy" - return stringInBraces</returns>
        public static string GetStringInBraces(string input)
        {
            return string.IsNullOrEmpty(input) ? null : Regex.Match(input, @"\{([^)]*)\}").Groups[1].Value;
        }

        #endregion
    }
}