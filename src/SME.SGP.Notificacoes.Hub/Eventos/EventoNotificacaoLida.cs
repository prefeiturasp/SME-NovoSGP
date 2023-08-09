using Microsoft.AspNetCore.SignalR;
using SME.SGP.Infra;
using SME.SGP.Notificacoes.Hub.Interface;
using System.Threading.Tasks;

namespace SME.SGP.Notificacoes.Hub
{
    public class EventoNotificacaoLida : EventoNotificacao<MensagemLeituraNotificacaoDto>, IEventoNotificacaoLida
    {
        public EventoNotificacaoLida(IServicoTelemetria servicoTelemetria)
            : base(servicoTelemetria, "Lida")
        {
        }

        protected override async Task DispararAsync(IHubCallerClients clients, MensagemLeituraNotificacaoDto mensagem)
        {
            var iClientProxy = await clients.UsuarioAsync(mensagem.UsuarioRf);
            await iClientProxy.SendAsync("NotificacaoLida", mensagem.Codigo, mensagem.AnoAnterior);
        }
    }
}