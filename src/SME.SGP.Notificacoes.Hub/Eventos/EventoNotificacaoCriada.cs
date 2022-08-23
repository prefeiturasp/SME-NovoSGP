using Microsoft.AspNetCore.SignalR;
using SME.SGP.Notificacoes.Hub.Interface;
using System.Threading.Tasks;

namespace SME.SGP.Notificacoes.Hub
{
    public class EventoNotificacaoCriada : IEventoNotificacaoCriada
    {
        public Task Enviar(IHubCallerClients clients, MensagemCriacaoNotificacaoDto mensagem)
            => clients.Usuario(mensagem.UsuarioRf)?.SendAsync("NotificacaoCriada", mensagem.Codigo, mensagem.Data, mensagem.Titulo, mensagem.Id);
    }
}
