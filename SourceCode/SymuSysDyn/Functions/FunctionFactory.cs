#region Licence

// Description: SymuBiz - SymuSysDyn
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
        public static BuiltInFunction CreateInstance(string model, string function)
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
                    return new Step(model, function);
                case Pulse.Value:
                    return new Pulse(model, function);
                case Normal.Value:
                    return new Normal(model, function);
                case Ramp.Value:
                    return new Ramp(model, function);
                case Smth1.Value:
                    return new Smth1(model, function);
                case Smth3.Value:
                    return new Smth3(model, function);
                case SmthN.Value:
                    return new SmthN(model, function);
                case Dt.Value:
                    return new Dt(model, function);
                case Time.Value:
                    return new Time(model, function);
                case ExternalUpdate.Value:
                    return new ExternalUpdate(model, function);
                default:
                    return new BuiltInFunction(model, function);
            }

            return null;
        }
    }
}