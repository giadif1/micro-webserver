using System;
using System.Collections;

namespace Logging
{
    public class LogChain : ILog
    {
        private readonly ArrayList loggers;

        public LogChain()
        {
            loggers = new ArrayList();
        }

        public void Add(ILog logger)
        {
            if (logger == null)
                throw new ArgumentException("Loggers cannot be null");
            loggers.Add(logger);
        }

        public void Report(PriorityType priority, string area, string message)
        {
            foreach (ILog logger in loggers)
                logger.Report(priority, area, message);
        }
    }
}
