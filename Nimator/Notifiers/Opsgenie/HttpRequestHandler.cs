using System;
using System.IO;
using System.Net;
using Nimator.Util;

namespace Nimator.Notifiers.Opsgenie
{
    /// <inheritdoc/>
    public class HttpRequestHandler : IHttpRequestHandler
    {
        /// <inheritdoc/>
        public HttpWebRequest CreateRequest(string url)
            => (HttpWebRequest)WebRequest.Create(url);

        /// <inheritdoc/>
        public string HandleRequest(HttpWebRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            try
            {
                using (var response = request.GetResponse())
                using (var resp = response.GetResponseStream())
                using (var reader = new StreamReader(resp))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                throw new NotificationCommunicationException($"Notification failed with '{ex.GetHttpStatus()}' for Url {request.RequestUri}", ex);
            }
        }

        /// <inheritdoc/>
        public string HandleRequest(HttpWebRequest request, string requestContent)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            if (string.IsNullOrEmpty(requestContent)) throw new ArgumentException("Cannot be empty", nameof(request));

            try
            {
                using (var requestStream = request.GetRequestStream())
                using (var writer = new StreamWriter(requestStream))
                {
                    writer.Write(requestContent);
                }

                using (var response = request.GetResponse())
                using (var resp = response.GetResponseStream())
                using (var reader = new StreamReader(resp))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                throw new NotificationCommunicationException($"Notification failed with '{ex.GetHttpStatus()}' for Url {request.RequestUri}", ex);
            }
        }
    }
}
