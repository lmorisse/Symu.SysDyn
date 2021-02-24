#region Licence

// Description: SymuBiz - SymuSysDyn
// Website: https://symu.org
// Copyright: (c) 2020 laurent Morisseau
// License : the program is distributed under the terms of the GNU General Public License

#endregion

#region using directives

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NCalcAsync;
using Symu.SysDyn.Core.Functions;
using Symu.SysDyn.Core.Parser;

#endregion

namespace Symu.SysDyn.Core.Equations
{
    public struct EquationFactoryStruct
    {
        public EquationFactoryStruct(IEquation equation, float value)
        {
            Equation = equation;
            Value = value;
        }
        public IEquation Equation { get; set; }
        public float Value { get; set; }
    }
    public struct EquationFactoryInitializeStruct
    {
        public EquationFactoryInitializeStruct(List<IBuiltInFunction> functions, List<string> variables, List<string> words, string initializedEquation)
        {
            Functions= functions;
            Variables= variables;
            Words = words;
            InitializedEquation = initializedEquation;
        }
        public List<IBuiltInFunction> Functions { get; set; }
        public List<string> Variables { get; set; }
        public List<string> Words {get; set; }
        public string InitializedEquation { get; set; }
}
    public static class EquationFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="eqn"></param>
        /// <returns>return the Equation and the initial value of this equation</returns>
        public static async Task<EquationFactoryStruct> CreateInstance(string model, string eqn)
        {
            return await CreateInstance(model, eqn, null);
        }

        public static async Task<EquationFactoryStruct> CreateInstance(string model, string eqn, Models.XMile.Range range)
        {
            if (string.IsNullOrEmpty(eqn))
            {
                return new EquationFactoryStruct(null,0);
            }

            var value = 0F;
            //Clean eqn
            // Remove string in Braces which are units, not equation
            var index = eqn.IndexOf('{');
            if (index > 0)
            {
                eqn = eqn.Remove(index);
            }

            eqn = eqn.Trim();
            eqn = eqn.Replace("'", "");

            if (float.TryParse(eqn, NumberStyles.Number, CultureInfo.InvariantCulture, out var floatEqn))
                //NumberStyles.Any => doesn't work for (1) => success and floatEqn = -1!
            {
                //value = floatEqn;
                return new EquationFactoryStruct(null, floatEqn); 
            }

            try
            {
                var existsFunctionTimeDependent = FunctionUtils.FunctionsTimeDependent.Any(function =>
                    eqn.ToLowerInvariant().Contains(function));
   

                if (!existsFunctionTimeDependent)
                {
                    // Test literal such as "1/10"
                    var expression = new Expression(eqn);
                    var eval = await expression.EvaluateAsync(0,0,1);
                    value = Convert.ToSingle(eval);
                    return new EquationFactoryStruct(null, value);
                }
            }
        
            catch
            {
                // not an constant
            }

            var initialise = await Initialize(model, eqn);

            float sumEval = 0;

            for (var i = 0; i < initialise.Words.Count; i++)
            {
                var word = initialise.Words[i];
                if (word.Length <= 1)
                {
                    continue;
                }

                float eval;
                var function = initialise.Functions.Find(x => x.IndexName == word);
                if (function != null)
                {
                    var tryReplace = await function.TryReplace(null);
                    if (!tryReplace.Success)
                    {
                        continue;
                    }

                    eval = tryReplace.Value;
                    initialise.Functions.Remove(function);
                }
                else
                {
                    var expression = new Expression(word);
                    try
                    {
                        // Test literal such as ".01"
                        eval = Convert.ToSingle(await expression.EvaluateAsync(0,0,1));
                    }
                    catch
                    {
                        continue;
                    }
                }

                // Variable can be replaced by a constant
                initialise.Words[i] = eval.ToString(CultureInfo.InvariantCulture);
                sumEval += eval;
            }

            var initializedEquation = string.Join(string.Empty, initialise.Words);

            // Equation with functions or only variables with brackets
            if (initialise.Functions.Any() || initialise.Words.Contains("("))
            {
                var complexEquation = new ComplexEquation(eqn, initializedEquation, initialise.Functions, initialise.Variables, initialise.Words, range);
                try
                {
                    value = await complexEquation.InitialValue();
                    return new EquationFactoryStruct(null, value);
                }
                catch
                {
                    //return complexEquation;
                    return new EquationFactoryStruct(complexEquation, value);
                }
            }

            // Only variables without brackets
            if (initialise.Variables.Any())
            {
                var equation =  new SimpleEquation(eqn, initializedEquation, initialise.Variables, initialise.Words, range);
                return new EquationFactoryStruct(equation, value);
            }

            // Only constants
            value = sumEval;
            return new EquationFactoryStruct(null, value);
        }

        /// <summary>
        ///     Clean equation to be able to be computed
        ///     Done once at the creation of the variable
        /// </summary>
        /// <param name="model">Model's name</param>
        /// <param name="originalEquation"></param>
        /// <returns>Initialized equation</returns>
        public static async Task<EquationFactoryInitializeStruct> Initialize(string model, string originalEquation)
        {
            if (originalEquation == null)
            {
                throw new ArgumentNullException(nameof(originalEquation));
            }

            var functions = await FunctionUtils.ParseFunctions(model, originalEquation);
            for (var i = 0; i < functions.Count; i++)
            {
                var function = functions[i];
                function.IndexName = function.Name + i;
                function.Name = StringUtils.CleanName(function.Name);
                originalEquation = originalEquation.Replace(function.OriginalFunction, function.IndexName);
            }

            // split equation in words
            var regexWords =
                new Regex(
                    @"[a-zA-Z0-9_]*\.?\,?[a-zA-Z0-9_]+|[-^+*\/()<>=]|\w+"); //@"[0-9]*\.?\,?[0-9]+|[-^+*\/()<>=]|\w+");
            var matches = regexWords.Matches(originalEquation);
            var words = new List<string>();
            var variables = new List<string>();
            foreach (var match in matches)
            {
                var word = StringUtils.CleanName(match.ToString());
                variables.AddRange(SetVariables(model, words, word, functions));
            }

            variables = variables.Distinct().ToList();
            var initializedEquation = string.Join(string.Empty, words);
            return new EquationFactoryInitializeStruct(functions, variables, words, initializedEquation);
        }

        /// <summary>
        ///     Functions Without Brackets ni lowercase
        /// </summary>
        private static readonly List<string> comparatorList =
            new List<string> { "==", "!=", "<", ">", "<=", ">=", "and", "or", "Greater", "lesser", "lesserorequal", "modulo", "notequal", "times", "bitwiseand", "bitwiseor", "bitwisexor", "leftshift", "rightshift", "not", "negate", "bitwisenot" };

        private static IEnumerable<string> SetVariables(string model, List<string> words, string word,
            List<IBuiltInFunction> functions)
        {
            var variables = new List<string>();
            if (word.Length <= 1 
                || float.TryParse(word, NumberStyles.Number, CultureInfo.InvariantCulture, out _)
                || comparatorList.Contains(word.ToLowerInvariant()))
            {
                words.Add(word);
                return variables;
            }

            var function = functions.Find(x => word == x.IndexName);
            if (function != null)
            {
                //Get the variables of the function
                foreach (var equation in function.Parameters.Where(x => x != null))
                {
                    variables.AddRange(equation.Variables);
                }

                words.Add(word);
            }
            else
            {
                var variable = StringUtils.FullName(model, word);
                words.Add(variable);
                variables.Add(variable);
            }

            return variables;
        }
    }
}