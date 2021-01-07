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
using Symu.SysDyn.Core.Parser;

#endregion

namespace Symu.SysDyn.Core.Functions
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
                case Step.Label:
                    return new Step(model, function);
                case Pulse.Label:
                    return new Pulse(model, function);
                case Normal.Label:
                    return new Normal(model, function);
                case Ramp.Label:
                    return new Ramp(model, function);
                case Smth1.Label:
                    return new Smth1(model, function);
                case Smth3.Label:
                    return new Smth3(model, function);
                case SmthN.Label:
                    return new SmthN(model, function);
                case Dt.Label:
                    return new Dt(model, function);
                case Time.Label:
                    return new Time(model, function);
                case Value.Label:
                    return new Value(model, function);
                case ExternalUpdate.Label:
                    return new ExternalUpdate(model, function);
                default:
                    return new BuiltInFunction(model, function);
            }

            return null;
        }
    }
}