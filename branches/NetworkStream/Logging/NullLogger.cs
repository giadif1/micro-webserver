namespace Logging
{
    public class NullLogger : ILog
    {
        public void Report(PriorityType priority, string area, string message)
        {
        }
    }
}
