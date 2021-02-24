#region Licence

// Description: SymuBiz - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Collections.Generic;
using System.Threading.Tasks;
using NCalcAsync;
using Symu.SysDyn.Core.Engine;
using Symu.SysDyn.Core.Equations;
using Symu.SysDyn.Core.Models.XMile;

#endregion

namespace Symu.SysDyn.Core.Functions
{
    /// <summary>
    ///     Interface for built in function defined by string
    ///     Works with nested functions, i.e. if the parameters of the function are functions
    /// </summary>
    public interface IBuiltInFunction
    {
        /// <summary>
        ///     The entire function included brackets and parameters
        /// </summary>
        string OriginalFunction { get; set; }

        /// <summary>
        ///     The function name
        /// </summary>
        string Name { get; set; }

        /// <summary>
        ///     The function name indexed
        /// </summary>
        string IndexName { get; set; }

        /// <summary>
        ///     List of arguments that are IEquation or null if it is a constant
        ///     If it is a constant, the value is stored in Args
        /// </summary>
        List<IEquation> Parameters { get; }

        Task<IBuiltInFunction> Clone();

        /// <summary>
        ///     Prepare the function for the Equation.Prepare()
        /// </summary>
        /// <param name="selfVariable"></param>
        /// <param name="variables"></param>
        /// <param name="sim"></param>
        /// <returns></returns>
        Task<float> Prepare(IVariable selfVariable, VariableCollection variables, SimSpecs sim);

        Task<TryReplaceStruct> TryReplace(SimSpecs sim);
        Task<TryReplaceStruct> TryEvaluate(IVariable variable, VariableCollection variables, SimSpecs sim);
        Task Replace(string child, string value, SimSpecs sim);
        List<float> Args { get; set; }
        string InitializedFunction { get; set; }
        Expression Expression { get; set; }
        string Model { get; set; }
    }
}