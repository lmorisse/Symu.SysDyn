#region Licence

// Description: SymuSysDyn - SymuSysDynCore
// Website: https://symu.org
// Copyright: (c) 2021 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Text.RegularExpressions;

#endregion

namespace Symu.SysDyn.Core.Equations
{
    /// <summary>
    ///     IF condition THEN expression ELSE expression
    ///     all non-zero values are true, while zero is false.
    ///     Generally, condition is an expression involving the logical, relational, and equality operators.
    /// </summary>
    public static class IfThenElse
    {
        public static string Parse(string input)
        {
            if (!input.StartsWith("if", true, null))
            {
                return input;
            }

            var regex = new Regex(@"IF\s*(.*)\s*THEN\s*(.*)\s*ELSE\s*(.*)", RegexOptions.IgnoreCase);
            var result = regex.Match(input);
            if (result.Groups.Count < 3)
            {
                //todo throw new exception + error management
                return string.Empty;
            }

            return !result.Success
                ? input
                : $"if({result.Groups[1].Value.Trim()},{result.Groups[2].Value.Trim()},{result.Groups[3].Value.Trim()})";
        }
    }
}