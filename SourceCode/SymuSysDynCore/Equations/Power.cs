using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Symu.SysDyn.Core.Equations
{
    public static class Power
    {
        public static string Parse(string input)
        {
            if (!input.Contains("^"))
            {
                return input;
            }

            //Regex rPower = new Regex(@"\w*\^\w*", RegexOptions.IgnoreCase);
            Regex rPower = new Regex(@"(\([^)]*\)|\w)\^(\([^)]*\)|\w)", RegexOptions.IgnoreCase);

            var result = rPower.Match(input);

            if (Count(input) == 1) {
                input = input.Replace("(", "");
                input = input.Replace(")", "");
                return ParseSimple(input);
            } else
            for (int y = 1; y < result.Groups.Count; y++)
            {
                string word = result.Groups[y].ToString();
                word = word.Replace("(", "");
                word = word.Replace(")", "");
                int nb = 0;
                foreach (char c in word)
                {
                    if (c == '^') nb++;
                }
                if (nb >= 2)
                {
                    input = input.Replace(result.Groups[y].Value, ParseMultiple(word));
                }
                else if (nb == 1)
                {
                    input = input.Replace(result.Groups[y].Value, ParseSimple(word));
                }
            }

            return ParseSimple(input);

        }

        public static string ParseMultiple(string resultat)
        {
            var nb = 0;
            var i = 0;
            string x = resultat[i].ToString();

            while (nb != 2)
            {
                if (x == "^") nb++;
                i += 1;
                x = resultat[i].ToString();
            }
            var stringToParse = resultat.Substring(0, i - 1);
            var resultat2 = Parse(stringToParse);
            resultat = resultat.Substring(i - 1, resultat.Length - (i - 1));
            Regex rPower = new Regex(@"[\w*]+", RegexOptions.IgnoreCase);
            foreach (Match match in rPower.Matches(resultat))
                resultat2 = "pow(" + resultat2 + "," + match.Value + ")";
            return resultat2;
        }

        public static string ParseSimple(string resultat)
        {
            var i = 0;
            string x = resultat.ToString();
            while (x != "^")
            {
                i += 1;
                x = resultat[i].ToString();
            }
            var variable1 = resultat.Substring(0, i);
            var variable2 = resultat.Substring(i + 1, resultat.Length - (i + 1));
            return "pow(" + variable1 + "," + variable2 + ")";
        }

        public static int Count(string resultat)
        {
            int count = 0;
            foreach (char c in resultat)
            {
                if (c == '^')
                {
                    count++;
                }
            }
            return count;
        }
    }
}
