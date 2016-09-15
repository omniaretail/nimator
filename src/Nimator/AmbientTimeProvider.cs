using System;

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
