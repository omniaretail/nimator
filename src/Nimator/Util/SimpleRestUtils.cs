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
        public static Action<string, object> PostToRestApi { get; set; } = PostToRestApiInternal;

        // Okay, this method is real dirty, but pending a more robust REST
        // implementation it might be worth to have some bastard injection
        // to be able to set a mock instance in unit tests (utilizing the
        // equally dirty InternalsVisibleTo attribute).
        internal static void SetPostToRestApiAction(Action<string, object> action)
        {
            PostToRestApi = action;
        }
        
        private static void PostToRestApiInternal(string url, object message)
        {
            using (var client = new WebClient())
            {
                try
                {
                    // Don't use a proxy. TODO: Make this configurable?
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
