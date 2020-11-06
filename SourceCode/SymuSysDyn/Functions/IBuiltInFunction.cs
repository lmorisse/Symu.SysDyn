#region Licence

// Description: SymuBiz - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Collections.Generic;
using Symu.SysDyn.Engine;
using Symu.SysDyn.Equations;
using Symu.SysDyn.Models;

#endregion

namespace Symu.SysDyn.Functions
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
        string OriginalFunction { get; }

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

        IBuiltInFunction Clone();

        /// <summary>
        ///     Prepare the function for the Equation.Prepare()
        /// </summary>
        /// <param name="selfVariable"></param>
        /// <param name="variables"></param>
        /// <param name="sim"></param>
        /// <returns></returns>
        float Prepare(IVariable selfVariable, VariableCollection variables, SimSpecs sim);

        float InitialValue(SimSpecs sim);
        bool TryEvaluate(IVariable variable, VariableCollection variables, SimSpecs sim, out float result);
        void Replace(string child, string value, SimSpecs sim);
    }
}