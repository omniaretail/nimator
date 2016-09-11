using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Nimator.Util
{
    internal static class WebExceptionExtensions
    {
        public static string GetHttpStatus(this WebException ex)
        {
            if (ex == null) throw new ArgumentNullException("ex");

            if (ex.Status == WebExceptionStatus.ProtocolError)
            {
                var response = ex.Response as HttpWebResponse;
                if (response != null)
                {
                    return (int)response.StatusCode + " " + response.StatusCode.ToString();
                }
            }

            return "HttpStatus Not Available";
        }
    }
}
