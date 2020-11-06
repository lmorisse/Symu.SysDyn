#region Licence

// Description: SymuBiz - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

#endregion

namespace Symu.SysDyn.Parser
{
    public static class StringUtils
    {
        public static string FullName(string model, string name)
        {
            return model + "_" + name;
        }

        public static string ConnectName(string model, string name)
        {
            return model + "." + name;
        }

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
            name = name.Replace("'", "");
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
        ///     For Connect, replace a model.variable name into Model_Variable
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string CleanFullName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            var splits = name.Split('.');
            var model = FirstCharToUpper(splits[0].ToLowerInvariant());
            var variable = FirstCharToUpper(splits[1].ToLowerInvariant());
            return FullName(model, variable);
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