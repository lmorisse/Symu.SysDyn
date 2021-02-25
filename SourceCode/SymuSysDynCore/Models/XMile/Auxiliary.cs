#region Licence

// Description: SymuSysDyn - SymuSysDynCore
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Threading.Tasks;

#endregion

namespace Symu.SysDyn.Core.Models.XMile
{
    /// <summary>
    ///     Auxiliaries allow the isolation of any algebraic function that is used. They can both clarify a model and
    ///     factor out important or repeated calculations
    ///     a variable that is not a flow and is capable of changing its value instantaneously.
    /// </summary>
    public class Auxiliary : Variable
    {
        public Auxiliary()
        {
        }

        public Auxiliary(string name, string model) : base(name, model)
        {
        }

        public static async Task<Auxiliary> CreateAuxiliary(string name, string model, string eqn)
        {
            return await CreateVariable<Auxiliary>(name, model, eqn);
        }

        private static async Task<Auxiliary> CreateAuxiliary(string name, string model, string eqn,
            GraphicalFunction graph, Range range, Range scale,
            NonNegative nonNegative, VariableAccess access)
        {
            return await CreateVariable<Auxiliary>(name, model, eqn, graph, range, scale, nonNegative,
                access);
        }

        //public new static async Task<Auxiliary> CreateInstance(string name, XMileModel model, string eqn)
        //{
        //    if (model == null)
        //    {
        //        throw new ArgumentNullException(nameof(model));
        //    }

        //    var variable = await CreateAuxiliary(name, model.Name, eqn);
        //    model.Variables.Add(variable);
        //    return variable;
        //}

        public static async Task<Auxiliary> CreateInstance(string name, XMileModel model, string eqn,
            GraphicalFunction graph,
            Range range, Range scale,
            NonNegative nonNegative, VariableAccess access)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var variable = await CreateAuxiliary(name, model.Name, eqn, graph, range, scale, nonNegative, access);
            model.Variables.Add(variable);
            return variable;
        }

        public override async Task<IVariable> Clone()
        {
            var clone = new Auxiliary(Name, Model);
            await CopyTo(clone);
            return clone;
        }

        //todo
        //Auxiliaries have one OPTIONAL attribute:
        // Flow concept: flow_concept="…" with true/false, which is true if the auxiliary represents a
        //flow concept(default: false). Besides documenting that the variable is conceptually a flow, this
        //affects how values are reported under certain integration methods and in tables.
    }
}