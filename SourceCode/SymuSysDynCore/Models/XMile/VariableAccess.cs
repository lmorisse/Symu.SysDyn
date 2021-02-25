#region Licence

// Description: SymuSysDyn - SymuSysDynCore
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

#endregion

namespace Symu.SysDyn.Core.Models.XMile
{
    /// <summary>
    ///     Access XMIle attribute values
    /// </summary>
    public enum VariableAccess
    {
        None,

        /// <summary>
        ///     If a variable has an output access, the variable can be used in another module
        /// </summary>
        Input,

        /// <summary>
        ///     If a variable has an input access, the variable is imported from another module
        /// </summary>
        Output
    }
}