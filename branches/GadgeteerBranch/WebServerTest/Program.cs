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
            webServer.Add("/", Resources.StringResources.Index, "text/html");
            webServer.Add("/MyStyles.css", Resources.StringResources.MyStyles, "text/css");
            webServer.Add("/test", HttpMethod.GET, request => new HtmlResponse("Hello World !"));
            webServer.Add("/redirect", HttpMethod.GET, request => new RedirectResponse("/"));
            webServer.Add("/api/time", HttpMethod.GET, GetTime);
            webServer.Add("/api/time", HttpMethod.PUT, SetTime);

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
