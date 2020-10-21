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
using System.Text.RegularExpressions;
using Symu.SysDyn.Parser;

namespace Symu.SysDyn.Equations
{
    //todo Maybe try a framework like https://github.com/IronyProject to have a real grammar more than regex
    public static class StringFunction
    {

        /// <summary>
        /// Extract functions from a string as a list of BuiltIn functions
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static IEnumerable<BuiltInFunction> GetStringFunctions(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }
            var builtInFunctions = new List<BuiltInFunction>();

            #region functions with ()
            var regex = new Regex(@"([a-zA-Z0-9]+)\s*\([^()]*\)");
            var functions = regex.Matches(input);

            if (functions.Count > 0)
            {
                foreach (var func in functions)
                {
                    var function = func.ToString().Trim();

                    var name = StringUtils.CleanName(function.Split('(')[0]);

                    if (string.IsNullOrEmpty(name))
                    {
                        continue;
                    }

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
                        case Smth1.Value:
                            builtInFunctions.Add(new Smth1(function));
                            break;
                        case Smth3.Value:
                            builtInFunctions.Add(new Smth3(function));
                            break;
                        case SmthN.Value:
                            builtInFunctions.Add(new SmthN(function));
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

            if (Dt.IsContainedIn(input, out var dt))
            {
                builtInFunctions.Add(new Dt(dt));
            }

            if (Time.IsContainedIn(input, out var time))
            {
                builtInFunctions.Add(new Time(time));
            }
            #endregion

            return builtInFunctions;
        }
    }
}