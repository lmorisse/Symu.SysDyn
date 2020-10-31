#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

#endregion

#region using directives

#endregion

#region using directives

using System;
using Symu.SysDyn.Parser;

#endregion

namespace Symu.SysDyn.Functions
{
    public static class FunctionFactory
    {
        public static BuiltInFunction CreateInstance(string function)
        {
            if (function == null)
            {
                throw new ArgumentNullException(nameof(function));
            }

            var name = StringUtils.CleanName(function.Split('(')[0]);
            switch (name)
            {
                // May be followed by () and then considered as a default builtin function
                case "If":
                case "Then":
                case "Else":
                    break;
                case Step.Value:
                    return new Step(function);
                case Normal.Value:
                    return new Normal(function);
                case Ramp.Value:
                    return new Ramp(function);
                case Smth1.Value:
                    return new Smth1(function);
                case Smth3.Value:
                    return new Smth3(function);
                case SmthN.Value:
                    return new SmthN(function);
                case Dt.Value:
                    return new Dt(function);
                case Time.Value:
                    return new Time(function);
                case ExternalUpdate.Value:
                    return new ExternalUpdate(function);
                default:
                    return new BuiltInFunction(function);
            }

            return null;
        }
    }
}