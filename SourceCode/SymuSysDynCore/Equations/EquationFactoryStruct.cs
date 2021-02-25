#region Licence

// Description: SymuSysDyn - SymuSysDynCore
// Website: https://symu.org
// Copyright: (c) 2021 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

namespace Symu.SysDyn.Core.Equations
{
    public struct EquationFactoryStruct
    {
        public EquationFactoryStruct(Equation equation, float value)
        {
            Equation = equation;
            Value = value;
        }

        public Equation Equation { get; set; }
        public float Value { get; set; }
    }
}