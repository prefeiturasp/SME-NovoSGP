using Hangfire.Logging;
using Sentry;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.Background.Hangfire.Logging
{
    public class SentryLog : ILog
    {
        public bool Log(LogLevel logLevel, Func<string> messageFunc, Exception exception = null)
        {
            if ((messageFunc == null && logLevel <= LogLevel.Warn) || (logLevel >= LogLevel.Error && exception == null))
                return false;

            switch (logLevel)
            {
                case LogLevel.Error:
                case LogLevel.Fatal:
                    if (messageFunc != null)
                        SentrySdk.CaptureException(new Exception(messageFunc(), exception));
                    else
                        SentrySdk.CaptureException(exception);
                    break;
                case LogLevel.Warn:
                    SentrySdk.CaptureMessage(messageFunc(), Sentry.Protocol.SentryLevel.Warning);
                    break;

                default:
                    SentrySdk.AddBreadcrumb(messageFunc(), $"Hangfire {logLevel.ToString()} event", "Logging");
                    break;
            }
            return true;
        }
    }
}
