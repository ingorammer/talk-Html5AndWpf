using System.IO;
using System.Net;
using CefSharp;

namespace TheIntegrator
{
    internal class AdvancedCefSharpRequestHandler : IRequestHandler
    {
        public bool OnBeforeBrowse(IWebBrowser browser, IRequest request, NavigationType naigationvType, bool isRedirect)
        {
            return false;
        }

        public bool OnBeforeResourceLoad(IWebBrowser browser, IRequestResponse requestResponse)
        {
            IRequest request = requestResponse.Request;
            System.Diagnostics.Debug.WriteLine("OnBeforeResourceLoad - " + request.Url);

            if (!request.Url.StartsWith("res://"))
                return false;

            string resourceName = request.Url.Substring(6).Replace("/", ".");

            try
            {
                var upperUrl = request.Url.ToUpperInvariant();
                string content = ScriptHelper.ReadResource(resourceName);
                MemoryStream ms = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(content));

                string mime = "text/plain";
                if (upperUrl.EndsWith("JS"))
                {
                    mime = "application/javascript";
                }
                else if (upperUrl.EndsWith("css"))
                {
                    mime = "text/css";
                }

                requestResponse.RespondWith(ms, mime);
            }
            catch
            {
                return false;
            }

            return false;
        }

        public void OnResourceResponse(IWebBrowser browser, string url, int status, string statusText, string mimeType,
                                       WebHeaderCollection headers)
        {
            return;
        }

        public bool GetDownloadHandler(IWebBrowser browser, string mimeType, string fileName, long contentLength,
                                       ref IDownloadHandler handler)
        {
            return false;
        }

        public bool GetAuthCredentials(IWebBrowser browser, bool isProxy, string host, int port, string realm, string scheme,
                                       ref string username, ref string password)
        {
            return false;
        }
    }
}