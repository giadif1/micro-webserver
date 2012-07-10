using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.SPOT;

namespace Logging
{
    //TODO: nog opnieuw te testen (2 klassen samengevoegd)
    //TODO: nakijken of deze klasse niet kan vereenvoudigd worden !!

    public class SyslogLogger: ILog
    {
        private readonly bool copyToDebugger;
        private readonly string machineName;
        private readonly PriorityType priorityThreshold;
        private readonly UTF8Encoding ascii = new UTF8Encoding();
        private readonly Socket socket;
        private readonly IPEndPoint sysLogEndPoint;

        public SyslogLogger(bool copyToDebugger, IPEndPoint syslogTarget, string machineName, PriorityType threshold)
        {
            sysLogEndPoint = syslogTarget;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            this.copyToDebugger = copyToDebugger;
            this.machineName = machineName;
            priorityThreshold = threshold;
        }

        public void Report(PriorityType priority, string area, string message)
        {
            if (copyToDebugger)
                Debug.Print(priority.Name() + " (" + area + ")\t" + " --> " + message + (priority >= priorityThreshold ? "*" : ""));
            if (priority >= priorityThreshold)
                Send(priority, DateTime.Now, machineName, area, message);
        }
        public void Send(PriorityType priority, DateTime time, string machine, string programArea, string body)
        {
            SendInternal(priority, time, machine, programArea, body, 0);
        }

        public void SendInternal(PriorityType priority, DateTime time, string machine, string programArea, string body, int facility)
        {
            var msg = "<" + (7 - (int)priority) + ">" + time.ToString("MMM d HH:mm:ss") + " " + machine + " " +
                         programArea + " " + body;
            if (time.Day < 10) msg = "<" + (7 - (int)priority) + ">" + time.ToString("MMM  d HH:mm:ss") + " " + machine + " " +
                         programArea + " " + body;
            var rawMsg = ascii.GetBytes(msg);
            socket.SendTo(rawMsg, sysLogEndPoint);
        }

        public void SendBootInfo(PriorityType priority, DateTime time, string machine, string programArea, string body)
        {
            SendInternal(priority, time, machine, programArea, body, 8);
        }
    }
}
