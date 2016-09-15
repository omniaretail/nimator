using System;
using System.Net;

namespace Nimator.Util
{
    internal static class WebExceptionExtensions
    {
        public static string GetHttpStatus(this WebException exception)
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));

            if (exception.Status == WebExceptionStatus.ProtocolError)
            {
                var response = exception.Response as HttpWebResponse;
                if (response != null)
                {
                    return (int)response.StatusCode + " " + response.StatusCode.ToString();
                }
            }

            return "HttpStatus Not Available";
        }
    }
}
