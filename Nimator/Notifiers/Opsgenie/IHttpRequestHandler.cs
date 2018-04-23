using System.Net;

namespace Nimator.Notifiers.Opsgenie
{    
    /// <summary>
    /// Abstract for http request handling.
    /// </summary>
    public interface IHttpRequestHandler
    {
        /// <summary>
        /// Factory method for creating a <see cref="HttpWebRequest"/>.
        /// </summary>
        /// <param name="url">The url for the request</param>
        /// <returns></returns>
        HttpWebRequest CreateRequest(string url);

        /// <summary>
        /// Handle a simple request.
        /// </summary>
        /// <param name="request">The <see cref="HttpWebRequest"/> to handle</param>
        /// <returns>Response content</returns>
        string HandleRequest(HttpWebRequest request);

        /// <summary>
        /// Handler for request with content.
        /// </summary>
        /// <param name="request">The <see cref="HttpWebRequest"/> to handle</param>
        /// <param name="requestContent">The content to send with the <see cref="HttpWebRequest"/></param>
        /// <returns>Response content</returns>
        string HandleRequest(HttpWebRequest request, string requestContent);
    }
}