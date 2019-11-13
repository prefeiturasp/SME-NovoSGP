using Microsoft.Extensions.Configuration;
using Sentry;
using System;

namespace SME.SGP.Infra
{
    public class ServicoLog : IServicoLog
    {
        private readonly string sentryDSN;

        public ServicoLog(IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }
            sentryDSN = configuration.GetSection("Sentry:DSN").Value;
        }

        public void Registrar(string mensagem)
        {
            using (SentrySdk.Init(sentryDSN))
            {
                SentrySdk.CaptureMessage(mensagem);
            }
        }

        public void Registrar(Exception ex)
        {
            using (SentrySdk.Init(sentryDSN))
            {
                SentrySdk.CaptureException(ex);
            }
        }
    }
}