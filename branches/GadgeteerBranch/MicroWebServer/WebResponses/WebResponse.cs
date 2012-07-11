using System.IO;
using System.Net;

namespace MicroWebServer
{
    public abstract class WebResponse
    {
        public HttpStatusCode StatusCode { get; protected set; }
        public string Content { get; protected set; }
        public string ContentType { get; protected set; }
        private readonly WebHeaderCollection additionalHeaders;

        protected WebResponse(HttpStatusCode statusCode, string content, string contentType)
        {
            StatusCode = statusCode;
            Content = content;
            ContentType = contentType;
            additionalHeaders = new WebHeaderCollection();
        }

        protected void AddHeader(string key, string value)
        {
            additionalHeaders.Add(key, value);
        }

        public void Respond(Gadgeteer.Networking.Responder responder)
        {
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(Content);
            responder.Respond(buffer, ContentType);
            //TODO: wat met uitgaande headers ???
        }
    }
}
