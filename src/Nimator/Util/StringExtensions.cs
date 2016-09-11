using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimator.Util
{
    // By @LBushkin from http://stackoverflow.com/a/2776689/419956
    internal static class StringExtentsions
    {
        public static string Truncate(this string value, int maxLength)
        {
            if (maxLength < 0) throw new ArgumentOutOfRangeException("maxLength");
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
    }
}
