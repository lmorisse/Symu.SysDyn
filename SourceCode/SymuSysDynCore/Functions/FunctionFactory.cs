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
using System.Threading.Tasks;
using Symu.SysDyn.Core.Parser;

#endregion

namespace Symu.SysDyn.Core.Functions
{
    public static class FunctionFactory
    {
        public static async Task<BuiltInFunction> CreateInstance(string model, string function)
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
                //case Step.Label:
                //    return new Step(model, function);
                //case Pulse.Label:
                //    return await Pulse.CreatePulse(model, function);
                //case Normal.Label:
                //    return await Normal.CreateNormal(model, function);
                //case Ramp.Label:
                //    return await Ramp.CreateRamp(model, function);
                //case Smth1.Label:
                //    return await Smth1.CreateSmth1(model, function);
                //case Smth3.Label:
                //    return await Smth3.CreateSmth3(model, function);
                //case SmthN.Label:
                //    return await SmthN.CreateSmthN(model, function);
                //case Dt.Label:
                //    return await Dt.CreateDt(model, function);
                //case Time.Label:
                //    return await Time.CreateTime(model, function);
                //case Value.Label:
                //    return await Value.CreateValue(model, function);
                //case ExternalUpdate.Label:
                //    return await ExternalUpdate.CreateExternalUpdate(model, function);
                default:
                    return await BuiltInFunction.CreateBuiltInFunction<BuiltInFunction>(model, function);
            }

            return null;
        }
    }
}