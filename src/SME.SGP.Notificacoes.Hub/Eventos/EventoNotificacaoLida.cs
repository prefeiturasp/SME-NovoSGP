using Microsoft.AspNetCore.SignalR;
using SME.SGP.Notificacoes.Hub.Interface;
using System.Threading.Tasks;

namespace SME.SGP.Notificacoes.Hub
{
    public class EventoNotificacaoLida : IEventoNotificacaoLida
    {
        public Task Enviar(IHubCallerClients clients, MensagemLeituraNotificacaoDto mensagem)
            => clients.Usuario(mensagem.UsuarioRf).SendAsync("NotificacaoLida", mensagem.Codigo);
    }
}
