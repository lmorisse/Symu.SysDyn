#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections;
using Symu.SysDyn.Models;

#endregion

namespace Symu.SysDyn.Results
{
    public class Result
    {
        private readonly Hashtable _result = new Hashtable();

        public Result(VariableCollection variables)
        {
            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }

            foreach (var variable in variables)
            {
                Add(variable.FullName, variable.Value);
            }
        }

        public int Count => _result.Count;

        public static Result CreateInstance(VariableCollection variables)
        {
            return new Result(variables);
        }

        public void Add(string name, float value)
        {
            _result.Add(name, value);
        }

        public float GetValue(string name)
        {
            return (float) _result[name];
        }
    }
}