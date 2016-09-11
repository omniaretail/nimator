using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Nimator.Util
{
    // We might need to replace this with a more robust outgoing REST 
    // implementation. But for now, let's isolate all Api Calls here.
    internal static class SimpleRestUtils
    {
        public static void PostToRestApi(string url, object message)
        {
            using (var client = new WebClient())
            {
                try
                {
                    // Don't use Squid proxy on production by bypassing default proxy:
                    client.Proxy = new WebProxy();

                    client.UploadString(url, JsonConvert.SerializeObject(message));
                }
                catch (WebException ex)
                {
                    throw new NotificationCommunicationException($"Notification failed with '{ex.GetHttpStatus()}' for Url {url}", ex);
                }
            }
        }
    }
}
