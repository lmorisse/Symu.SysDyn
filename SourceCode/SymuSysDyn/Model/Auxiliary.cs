#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

using System;
using System.Collections.Generic;

namespace Symu.SysDyn.Model
{
    /// <summary>
    ///     Auxiliaries allow the isolation of any algebraic function that is used. They can both clarify a model and
    ///     factor out important or repeated calculations
    ///     a variable that is not a flow and is capable of changing its value instantaneously.
    /// </summary>
    public class Auxiliary : Variable
    {
        private Auxiliary(string name) : base(name)
        {
        }

        private Auxiliary(string name, string eqn) : base(name, eqn)
        {
        }
        public new static Auxiliary CreateInstance(Variables variables, string name, string eqn)
        {
            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }

            var variable = new Auxiliary(name, eqn);
            variables.Add(variable);
            return variable;
        }
        public static Auxiliary CreateInstance(List<IVariable> variables, string name, string eqn)
        {
            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }

            var variable = new Auxiliary(name, eqn);
            variables.Add(variable);
            return variable;
        }
        private Auxiliary(string name, string eqn, GraphicalFunction graph, Range range, Range scale,
            NonNegative nonNegative) : base(name, eqn,
            graph, range, scale, nonNegative)
        {
        }

        public static Auxiliary CreateInstance(Variables variables, string name, string eqn, GraphicalFunction graph, Range range, Range scale,
            NonNegative nonNegative)
        {
            if (variables == null)
            {
                throw new ArgumentNullException(nameof(variables));
            }

            var variable = new Auxiliary(name, eqn, graph, range, scale, nonNegative);
            variables.Add(variable);
            return variable;
        }

        public override IVariable Clone()
        {
            var clone = new Auxiliary(Name);
            CopyTo(clone);
            return clone;
        }

        //todo
        //Auxiliaries have one OPTIONAL attribute:
        // Flow concept: flow_concept="…" with true/false, which is true if the auxiliary represents a
        //flow concept(default: false). Besides documenting that the variable is conceptually a flow, this
        //affects how values are reported under certain integration methods and in tables.
    }
}