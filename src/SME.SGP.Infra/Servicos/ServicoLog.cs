﻿using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class ServicoLog : IServicoLog
    {
        private readonly TelemetryClient insightsClient;
        private readonly string sentryDSN;

        public ServicoLog(IConfiguration configuration, TelemetryClient insightsClient)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            this.insightsClient = insightsClient ?? throw new ArgumentNullException(nameof(insightsClient));
        }

        public void Registrar(string mensagem)
        {

        }

        public void Registrar(Exception ex)
        {

        }

        public void RegistrarAppInsights(string evento, string mensagem)
        {
            insightsClient.TrackEvent(evento,
                new Dictionary<string, string>()
                          {
                             { DateTime.Now.ToLongDateString(), mensagem }
                         });
        }

        public void RegistrarDependenciaAppInsights(string tipoDependencia, string alvo, string mensagem, DateTimeOffset inicio, TimeSpan duracao, bool sucesso)
        {
            insightsClient.TrackDependency(tipoDependencia, alvo, mensagem, inicio, duracao, sucesso);
        }
    }
}