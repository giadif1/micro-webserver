using Microsoft.SPOT;

namespace Logging
{
    public class DebugLogger : ILog
    {
        private readonly PriorityType reportLevel;

        public DebugLogger(PriorityType reportLevel)
        {
            this.reportLevel = reportLevel;
        }

        public void Report(PriorityType level, string area, string message)
        {
            if (level >= reportLevel)
                Debug.Print(area + " : " + level.ToString() + " -> " + message);
        }
    }
}
