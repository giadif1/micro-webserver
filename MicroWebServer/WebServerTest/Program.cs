using System;
using System.Threading;
using MicroWebServer;
using MicroWebServer.Json;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace WebServerTest
{
    public class Program
    {
        public static void Main()
        {
            Debug.EnableGCMessages(false);
            Debug.Print("Web Server test software");

            // Print the network interface information to the debug interface
            Microsoft.SPOT.Net.NetworkInformation.NetworkInterface NI = Microsoft.SPOT.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces()[0];
            //NI.EnableDhcp();
            Debug.Print("IP Address = " + NI.IPAddress + ", Gateway = " + NI.GatewayAddress + ", MAC = " + NI.PhysicalAddress);

            var webServer = new WebServer(null, Resources.ResourceManager);
            webServer.Add(new RequestRoute("/", Resources.StringResources.Index, "text/html"));
            webServer.Add(new RequestRoute("/MyStyles.css", Resources.StringResources.MyStyles, "text/css"));
            webServer.Add(new RequestRoute("/test", HttpMethods.GET, request => new HtmlResponse("Hello World !")));
            webServer.Add(new RequestRoute("/redirect", HttpMethods.GET, request => new RedirectResponse("/")));
            webServer.Add(new RequestRoute("/api/time", HttpMethods.GET, GetTime));
            webServer.Add(new RequestRoute("/api/time", HttpMethods.PUT, SetTime));

            Thread.Sleep(Timeout.Infinite);
        }

        private static WebResponse GetTime(WebRequest request)
        {
            DateTime now = DateTime.Now;
            Debug.Print("Sending " + now.ToLocalTime());
            return new JsonResponse("\"" + JsonDateTime.ToASPNetAjax(now) + "\"");
        }

        private static WebResponse SetTime(WebRequest request)
        {
            DateTime time = JsonDateTime.FromASPNetAjax(request.Content);
            Utility.SetLocalTime(time);
            Debug.Print("Time Set to " + time.ToUniversalTime() + " (day = " + time.Day + ")");
            return new EmptyResponse();
        }
    }
}
