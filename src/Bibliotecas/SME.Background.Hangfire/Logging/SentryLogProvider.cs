using Hangfire;
using Hangfire.Annotations;
using Hangfire.Logging;
using System;
using System.Collections.Generic;
using System.Text;

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
