using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimator.CouchDb
{
    public static class Guard
    {
        /// <summary>
        /// Check for null values
        /// </summary>
        /// <param name="argumentName">Name of the value</param>
        /// <param name="value">Value of the value</param>
        public static void AgainstNull(string argumentName, object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }
    }
}
