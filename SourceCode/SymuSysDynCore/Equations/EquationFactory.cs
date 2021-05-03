#region Licence

// Description: SymuSysDyn - SymuSysDynCore
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Threading.Tasks;
using NCalcAsync.Domain;
using Range = Symu.SysDyn.Core.Models.XMile.Range;

#endregion

namespace Symu.SysDyn.Core.Equations
{
    public static class EquationFactory
    {
        /// <summary>
        /// </summary>
        /// <param name="model"></param>
        /// <param name="eqn"></param>
        /// <returns>return the Equation and the initial value of this equation</returns>
        public static async Task<EquationFactoryStruct> CreateInstance(string model, string eqn)
        {
            return await CreateInstanceAsync(model, eqn, null);
        }

        public static async Task<EquationFactoryStruct> CreateInstanceAsync(string model, string eqn,
            Range range)
        {
            if (string.IsNullOrEmpty(eqn))
            {
                return new EquationFactoryStruct(null, 0);
            }

            //Clean eqn
            // Remove string in Braces which are units, not equation
            var index = eqn.IndexOf('{');
            if (index > 0)
            {
                eqn = eqn.Remove(index);
            }

            eqn = eqn.Trim();
            eqn = eqn.Replace("'", "");
            // IfThenElse
            eqn = IfThenElse.Parse(eqn);
            eqn = Power.Parse(eqn);
            if (string.IsNullOrEmpty(eqn))
            {
                return new EquationFactoryStruct(null, 0);
            }

            // General case
            var equation = new Equation(eqn);
            var value = await equation.InitialValue(model);
            if (!string.IsNullOrEmpty(equation.Expression.Error))
            {
                // Expression has error
                throw new ArgumentException(equation.Expression.Error);
            }

            if (equation.Expression.ParsedExpression is ValueExpression
                || equation.CanBeOptimized())
            {
                return new EquationFactoryStruct(null, value);
            }

            return new EquationFactoryStruct(equation, value);
        }
    }
}