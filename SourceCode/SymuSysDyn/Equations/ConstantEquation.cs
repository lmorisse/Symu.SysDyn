#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using System.Collections.Generic;
using System.Globalization;
using Symu.SysDyn.Model;
using Symu.SysDyn.Simulation;

namespace Symu.SysDyn.Equations
{
    /// <summary>
    /// Optimized IEquation for variable that have an constant equation
    /// </summary>
    public class ConstantEquation : IEquation
    {
        private readonly float _constant;

        public string InitializedEquation => _constant.ToString(CultureInfo.InvariantCulture);
        public string OriginalEquation => _constant.ToString(CultureInfo.InvariantCulture);
        public ConstantEquation(float constant) 
        {
            _constant = constant;
        }

        public List<string> Variables { get; } = new List<string>();

        public float Evaluate(Variables variables, SimSpecs sim)
        {
            return _constant;
        }

        public void Prepare(Variables variables, SimSpecs sim)
        {
        }        
        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return InitializedEquation;
        }
    }
}