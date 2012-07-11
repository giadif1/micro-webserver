using System;
using Microsoft.SPOT;

namespace MicroWebServer
{
    public enum HttpMethod
    {
        GET, 
        POST, 
        PUT, 
        DELETE
    }

    public static class HttpMethodParser
    {
        public static HttpMethod HttpMethodParse(this string method)
        {
            switch(method)
            {
                case "GET": return HttpMethod.GET;
                case "POST": return HttpMethod.POST;
                case "DELETE": return HttpMethod.DELETE;
                case "PUT": return HttpMethod.PUT;
                default: throw new ApplicationException("Unknown Http Method");
            }
        }

        public static HttpMethod HttpMethodParse(this Gadgeteer.Networking.WebServer.HttpMethod method)
        {
            switch (method)
            {
                case Gadgeteer.Networking.WebServer.HttpMethod.GET: return HttpMethod.GET;
                case Gadgeteer.Networking.WebServer.HttpMethod.POST: return HttpMethod.POST;
                case Gadgeteer.Networking.WebServer.HttpMethod.DELETE: return HttpMethod.DELETE;
                case Gadgeteer.Networking.WebServer.HttpMethod.PUT: return HttpMethod.PUT;
                default: throw new ApplicationException("Unknown Http Method");
            }
        }

    }
}
