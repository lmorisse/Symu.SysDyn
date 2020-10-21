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
using Symu.SysDyn.Model;

#endregion

namespace Symu.SysDyn.Parser
{
    //todo Maybe try a framework like https://github.com/IronyProject to have a real grammar more than regex
    public static class StringUtils
    {

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