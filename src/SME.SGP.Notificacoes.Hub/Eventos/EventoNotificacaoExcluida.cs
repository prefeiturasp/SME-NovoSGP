using Microsoft.AspNetCore.SignalR;
using SME.SGP.Notificacoes.Hub.Interface;
using System.Threading.Tasks;

namespace SME.SGP.Notificacoes.Hub
{
    public class EventoNotificacaoExcluida : IEventoNotificacaoExcluida
    {
        public Task Enviar(IHubCallerClients clients, MensagemExclusaoNotificacaoDto mensagem)
            => clients.Usuario(mensagem.UsuarioRf).SendAsync("NotificacaoExcluida", mensagem.Codigo);
    }
}
