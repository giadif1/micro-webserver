using System;
using System.Net;
using System.Threading;
using Microsoft.SPOT;
using System.Resources;
using System.Net.Sockets;

namespace MicroWebServer
{
    public class WebServer
    {
        public const int MAX_CONNECTIONS_BACKLOG = 10;
        private readonly RequestRouteList routeList;
        private Thread mainThread;
        private readonly ILog log;
        private readonly ResourceManager resourceManager;
        private readonly int portNumber;
        private readonly string defaultFileName;


        public WebServer(ILog log, ResourceManager resourceManager, int portNumber=80, string defaultFileName= "index.html")
        {
            this.log = log;
            routeList = new RequestRouteList();
            this.resourceManager = resourceManager;
            this.portNumber = portNumber;
            this.defaultFileName = defaultFileName;
            mainThread = new Thread(MainThreadHandler);
            mainThread.Start();
        }

        //public bool MainThreadIsAlive { get { return mainThread.IsAlive; } }

        //public ThreadState MainThreadStatus { get { return mainThread.ThreadState; } }

        /*public void RestartMainThread()
        {
            //TODO: hopelijk is deze binnenkort niet meer nodig
            mainThread.Abort();
            Thread.Sleep(2000);
            //log.Report(PriorityType.Alert, "Webserver", "Creating a new main thread for the webserver !!!");
            mainThread = new Thread(MainThreadHandler);
            mainThread.Start();
        }*/

        public void Add(RequestRoute route)
        {
            routeList.Add(route);
        }

        private void MainThreadHandler()
        {
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Any, portNumber));
            listener.Listen(MAX_CONNECTIONS_BACKLOG);

            while (true)//TODO: the vervangen door while (!abortRequested)
            {
                Socket socket = listener.Accept();
                new WebRequestHandlerInternal(socket, routeList);
            }

            //bool abortRequested = false;
            listener.Start();

            //TODO: Threading verhaal nog te bekijken !!
            while (!abortRequested)
            {
                try
                {
                    Debug.Print("Webserver while loop beginning");
                    var context = listener.GetContext();
                    var path = context.Request.Url.OriginalString;
                    var method = context.Request.HttpMethod.HttpMethodParse();
                    //Debug.Print("Webserver incoming request : '" + url + "'");

                    WebResponse response = new NotFoundResponse(path);

                    if (routeList.Contains(method, path))
                    {
                        var route = routeList.Find(method, path);
                        if (route.IsFileResponse)
                        {
                            string content = (string) ResourceUtility.GetObject(resourceManager, route.FileResource);
                            response = new FileResponse(content, route.ContentType);
                        }
                        else
                        {
                            var webRequest = new WebRequest(context.Request);
                            response = route.RequestHandler(webRequest);
                        }
                    }

                    response.Respond(context);
                    context.Close();
                }
                catch (Exception exception)
                {
                    listener.Stop();
                    listener.Close();
                    abortRequested = true;
                }
            }
        }
    }
}