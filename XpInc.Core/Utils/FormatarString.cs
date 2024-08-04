using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace XpInc.Core.Utils
{
    public static class FormatarString
    {
        public static string ExtractNumbers(string input)
        {
            string pattern = @"\D";
            string numbersOnly = Regex.Replace(input, pattern, "");
            return numbersOnly;
        }
    }
}
