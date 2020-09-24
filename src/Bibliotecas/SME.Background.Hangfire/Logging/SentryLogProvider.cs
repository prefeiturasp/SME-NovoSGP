using Hangfire.Logging;

namespace SME.Background.Hangfire.Logging
{
    public class SentryLogProvider : ILogProvider
    {
        public ILog GetLogger(string name)
        {
            return new SentryLog();
        }
    }
}
