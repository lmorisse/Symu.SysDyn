#region Licence

// Description: SymuSysDyn - SymuSysDynCore
// Website: https://symu.org
// Copyright: (c) 2021 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

namespace Symu.SysDyn.Core.Functions
{
    public struct TryReplaceStruct
    {
        public TryReplaceStruct(bool success, float value)
        {
            Success = success;
            Value = value;
        }
        public bool Success { get; set; }
        public float Value { get; set; }
    }
}