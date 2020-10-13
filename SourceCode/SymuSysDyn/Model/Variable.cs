#region Licence

// Description: SymuSysDyn - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Symu.SysDyn.Parser;

#endregion

namespace Symu.SysDyn.Model
{
    /// <summary>
    ///     Base class for the variable of the model
    /// </summary>
    public class Variable
    {
        public Variable(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name.Replace(' ', '_');
        }

        public Variable(string name, string eqn) : this(name)
        {
            Value = CheckInitialValue(eqn);
            Equation = eqn;
            FindChildren();
        }

        public Variable(string name, string eqn, GraphicalFunction graph) : this(name, eqn)
        {
            Function = graph;
        }

        #region Xml attributes

        public string Eqn { get; set; }

        #endregion

        public string Name { get; }

        public float Value { get; set; }

        public string Equation { get; set; }

        public GraphicalFunction Function { get; set; }

        public float OldValue { get; set; }

        /// <summary>
        ///     The variable has been updated
        /// </summary>
        public bool Updated { get; set; }

        /// <summary>
        ///     The variable is being updating
        /// </summary>
        public bool Updating { get; set; }

        /// <summary>
        ///     Children are items from Equation that are not numbers, operators, blanks nor itself
        /// </summary>
        public List<string> Children { get; protected set; }

        /// <summary>
        ///     Find all children of a variable
        /// </summary>
        /// <returns></returns>
        public void FindChildren()
        {
            var words = Equation?.Split(' ', '+', '-', '*', '/');

            Children = words?.Where(word => !XmlConstants.Operators.Contains(word) &&
                                            !float.TryParse(word, NumberStyles.Any, CultureInfo.InvariantCulture,
                                                out _) &&
                                            word.Length > 0
                                            && !word.Equals(Name)).ToList();
        }

        public static float CheckInitialValue(string equation)
        {
            if (!float.TryParse(equation, out var n))
            {
                n = 0;
            }

            return n;
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}