using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Microsoft.SPOT;
using NetMf.CommonExtensions;

namespace MicroWebServer
{
    public class WebRequestHandlerInternal
    {
        public const int BUFFER_LENGTH = 256;
        private Socket socket;
        private RequestRouteList routeList;
        private static Decoder decoder = Encoding.UTF8.GetDecoder();

        public WebRequestHandlerInternal(Socket socket, RequestRouteList routeList)
        {
            this.socket = socket;
            this.routeList = routeList;
            var thread = new Thread(ThreadMethod);
            thread.Start();
        }

        private void ThreadMethod()
        {
            var buffer = new byte[BUFFER_LENGTH];
            StringBuilder sb = new StringBuilder();
            int bytesReceived;
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger, -2);
            while((bytesReceived = socket.Receive(buffer)) >= 0)
            {
                sb.Append(Encoding.UTF8.GetChars());
            }
        }
    }
}
