using System.IO.Ports;
using System.Text;

namespace Logging
{
    public class SerialLogger : ILog
    {
        private readonly PriorityType reportLevel;
        private readonly SerialPort port;

        public SerialLogger(PriorityType reportLevel, SerialPort port)
        {
            this.reportLevel = reportLevel;
            this.port = port;
        }

        public void Report(PriorityType priority, string area, string message)
        {

            if (priority < reportLevel) return;
            var str = (area + " : " + priority.ToString() + " -> " + message);

            if (port.IsOpen)
            {
                byte[] buffer = Encoding.UTF8.GetBytes(str + "\r\n");
                port.Write(buffer, 0, buffer.Length);
            }
        }
    }
}