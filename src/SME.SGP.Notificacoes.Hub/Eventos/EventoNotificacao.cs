using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using SME.SGP.Infra;
using SME.SGP.Notificacoes.Hub.Interface;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Notificacoes.Hub
{
    public class EventoNotificacao<T> : IEventoNotificacao<T>
        where T : MensagemNotificacaoDto
    {
        private readonly IServicoTelemetria servicoTelemetria;
        private readonly string nomeEvento;

        public EventoNotificacao(IServicoTelemetria servicoTelemetria, string nomeEvento)
        {
            this.servicoTelemetria = servicoTelemetria ?? throw new System.ArgumentNullException(nameof(servicoTelemetria));
            this.nomeEvento = nomeEvento ?? throw new ArgumentNullException(nameof(nomeEvento));
        }

        public Task EnviarAsync(IHubCallerClients clients, T mensagem)
        {
            var transacao = servicoTelemetria.Iniciar($"EventoNotificacao.{nomeEvento}", "Hub");
            try
            {
                servicoTelemetria.RegistrarAsync(() => DispararAsync(clients, mensagem), "Hub", "EventoNotificacao", nomeEvento, JsonConvert.SerializeObject(mensagem));
            }
            finally
            {
                servicoTelemetria.Finalizar(transacao);
            }

            return Task.CompletedTask;
        }
        protected virtual Task DispararAsync(IHubCallerClients clients,T mensagem)
            => throw new NotImplementedException("Disparo do evento de notificação não implementado");
    }
}
