using Gadgeteer.Networking;

namespace MicroWebServer
{
    public class WebRequest
    {
        private readonly Responder responder;
        public string Uri { get { return responder.Path; } }
        public string ContentType { get { return responder.Body.ContentType; } }
        public string Content { get { return responder.Body.Text; } }
        public HttpMethod Method { get { return responder.HttpMethod.HttpMethodParse(); } }

        public WebRequest(Responder responder)
        {
            this.responder = responder;
        }

        public string GetHeaderField(string name)
        {
            return responder.GetHeaderField(name);
        }

    }
}
