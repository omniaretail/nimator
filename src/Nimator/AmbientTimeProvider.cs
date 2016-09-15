using System;

namespace Nimator
{
    /// <summary>
    /// Utility class to provide time-related functionality.
    /// </summary>
    public static class AmbientTimeProvider
    {
        static AmbientTimeProvider()
        {
            GetNow = () => DateTime.Now;
        }

        /// <summary>
        /// Returns a DateTime instance representing the current moment. Default statically initialized to always
        /// return <see cref="DateTime.Now"/>, but can be overridden.
        /// </summary>
        public static Func<DateTime> GetNow { get; private set; }

        /// <summary>
        /// Overrides the default implementation behind <see cref="GetNow"/>. This can be useful for example when
        /// you want to run monitoring cycles as if they were at some other moment than the local machine's date
        /// and time, or when you want to set a mock implementation for automated tests.
        /// </summary>
        /// <param name="getDateTimeFunc">A new function to get <see cref="DateTime"/>s representing "right now".</param>
        /// <remarks>
        /// Note that if you set a new <see cref="GetNow"/> implementation, the old one is lost. If you only want
        /// to temporarily replace it you have to save the original implementation and call this function again to
        /// set it back. 
        /// </remarks>
        public static void SetNewTimeProvider(Func<DateTime> getDateTimeFunc)
        {
            GetNow = getDateTimeFunc;
        }
    }
}
