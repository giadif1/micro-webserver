using System;
using System.Net;
using System.Threading;
using Microsoft.SPOT;
using System.Resources;
using Gadgeteer.Networking;
using Microsoft.SPOT.Net.NetworkInformation;
using gWebServer = Gadgeteer.Networking.WebServer;
using gHttpMethod = Gadgeteer.Networking.WebServer.HttpMethod;

namespace MicroWebServer
{
    public class WebServer
    {
        private readonly RequestRouteList routeList;
        private readonly ILog log;
        private readonly ResourceManager resourceManager;

        public WebServer(ILog log, ResourceManager resourceManager)
        {
            this.log = log;
            routeList = new RequestRouteList();
            this.resourceManager = resourceManager;
            NetworkInterface NI = NetworkInterface.GetAllNetworkInterfaces()[0];

            gWebServer.StartLocalServer(NI.IPAddress, 80);
        }


        //For static text resource files (only for GET requests)
        public void Add(string path, Enum fileResource, string contentType)
        {
            routeList.Add(new RequestRoute(path, fileResource, contentType));
            gWebServer.SetupWebEvent(path).WebEventReceived += MicroWebServer_WebEventReceived;
        }

        //For callback responses
        public void Add(string path, HttpMethod httpMethod, WebRequestHandler requestHandler)
        {
            routeList.Add(new RequestRoute(path, httpMethod, requestHandler));
        }

        //For Gadgeteer event responses
        public void Add(string path, WebEvent eEvent)
        {
            throw new NotImplementedException();
        }

        void MicroWebServer_WebEventReceived(string path, gHttpMethod httpMethod, Responder responder)
        {
            var request = new WebRequest(responder);
            var method = request.Method;
            
            if(!routeList.Contains(method, path))
            {
                //TODO: how to send a status code with the Gadgeteer web server ?
                // Return file not found
                var response = new HtmlResponse("File not found");
                response.Respond(responder);
            }
            
            var route = routeList.Find(method, path);
            if (route.IsCallbackResponse)
                route.RequestHandler(request).Respond(responder);
            else
            {
                //Resource file response
                var content = (string)ResourceUtility.GetObject(resourceManager, route.FileResource);
                var response = new FileResponse(content, route.ContentType);
                response.Respond(responder);
            }
        }
    }
}