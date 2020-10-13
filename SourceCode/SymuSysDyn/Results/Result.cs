#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections;
using Symu.SysDyn.Model;

#endregion

namespace Symu.SysDyn.Results
{
    public class Result
    {
        private readonly Hashtable _result = new Hashtable();

        public Result(Variables variables)
        {
            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }

            foreach (var variable in variables)
            {
                Add(variable.Name, variable.Value);
            }
        }

        public int Count => _result.Count;

        public static Result CreateInstance(Variables variables)
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