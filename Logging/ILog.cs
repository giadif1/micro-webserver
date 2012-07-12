namespace Logging
{
    public interface ILog
    {
        void Report(PriorityType priority, string area, string message);
    }
}
