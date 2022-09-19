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

        protected override Task Disparar(IHubCallerClients clients, MensagemLeituraNotificacaoDto mensagem)
            => clients.Usuario(mensagem.UsuarioRf).SendAsync("NotificacaoLida", mensagem.Codigo);
    }
}
