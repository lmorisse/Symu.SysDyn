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

#endregion

namespace Symu.SysDyn.Models.XMile
{
    /// <summary>
    ///     Interface for the variable of the model
    /// </summary>
    public interface IVariable
    {
        float Value { get; set; }
        IEquation Equation { get; set; }

        /// <summary>
        ///     The variable has been updated
        /// </summary>
        bool Updated { get; set; }

        /// <summary>
        ///     The variable is being updating
        /// </summary>
        bool Updating { get; set; }

        /// <summary>
        ///     Children are Equation's Variables except itself
        /// </summary>
        /// <remarks>Could be a computed property, but for performance, it is setted once</remarks>
        List<string> Children { get; set; }

        GraphicalFunction GraphicalFunction { get; set; }


        string Name { get; }
        string Model { get; }
        string FullName { get; }
        Units Units { get; set; }

        /// <summary>
        ///     Input range
        /// </summary>
        Range Range { get; set; }

        /// <summary>
        ///     Output scale
        /// </summary>
        Range Scale { get; set; }

        NonNegative NonNegative { get; set; }

        VariableAccess Access { get; set; }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        string ToString();

        void Update(VariableCollection variables, SimSpecs simulation);
        bool TryOptimize(bool setInitialValue, SimSpecs sim);
        void Initialize();
        IVariable Clone();
        bool Equals(string fullName);
    }
}