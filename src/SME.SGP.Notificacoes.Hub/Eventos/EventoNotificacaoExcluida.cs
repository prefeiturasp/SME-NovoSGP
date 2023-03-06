using Microsoft.AspNetCore.SignalR;
using SME.SGP.Infra;
using SME.SGP.Notificacoes.Hub.Interface;
using System.Threading.Tasks;

namespace SME.SGP.Notificacoes.Hub
{
    public class EventoNotificacaoExcluida : EventoNotificacao<MensagemExclusaoNotificacaoDto>, IEventoNotificacaoExcluida
    {
        public EventoNotificacaoExcluida(IServicoTelemetria servicoTelemetria) 
            : base(servicoTelemetria, "Excluida")
        {
        }

        protected override Task Disparar(IHubCallerClients clients, MensagemExclusaoNotificacaoDto mensagem)
            => clients.Usuario(mensagem.UsuarioRf).SendAsync("NotificacaoExcluida", mensagem.Codigo, mensagem.Status, mensagem.AnoAnterior);
    }
}
