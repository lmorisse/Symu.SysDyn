#region Licence

// Description: SymuBiz - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Symu.SysDyn.Models;
using Symu.SysDyn.Models.XMile;

#endregion

namespace Symu.SysDyn.Results
{
    public class Result 
    {
        private readonly Hashtable _result = new Hashtable();

        public Result(){}
        public Result(VariableCollection variables)
        {
            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }

            foreach (var variable in variables.Where(x => x.StoreResult))
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