#region Licence

// Description: SymuBiz - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

#endregion

namespace Symu.SysDyn.Core.Functions
{
    /// <summary>
    ///     A list of built in functions
    /// </summary>
    public class BuiltInFunctionCollection : IEnumerable<IBuiltInFunction>
    {
        private readonly List<IBuiltInFunction> _functions = new List<IBuiltInFunction>();

        public BuiltInFunctionCollection(IEnumerable<IBuiltInFunction> functions)
        {
            _functions.AddRange(functions);
        }

        public async Task<List<IBuiltInFunction>> Clone()
        {
            var result = new List<IBuiltInFunction>();
            foreach (var function in _functions)
            {
                result.Add(await function.Clone());
            }
            return result;
        }

        public bool Any()
        {
            return _functions.Any();
        }

        public IEnumerable<IBuiltInFunction> ToImmutableList()
        {
            return _functions.ToImmutableList();
        }

        public void Remove(IBuiltInFunction function)
        {
            _functions.Remove(function);
        }


        #region IEnumerator members

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<IBuiltInFunction> GetEnumerator()
        {
            return _functions.GetEnumerator();
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _functions.GetEnumerator();
        }

        #endregion
    }
}