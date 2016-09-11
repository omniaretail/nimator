using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimator
{
    public static class AmbientTimeProvider
    {
        static AmbientTimeProvider()
        {
            GetNow = () => DateTime.Now;
        }

        public static Func<DateTime> GetNow { get; private set; }

        public static void SetNewTimeProvider(Func<DateTime> getDateTimeFunc)
        {
            GetNow = getDateTimeFunc;
        }
    }
}
