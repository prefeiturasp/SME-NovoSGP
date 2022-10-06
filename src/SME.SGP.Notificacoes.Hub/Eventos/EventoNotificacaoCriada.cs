using Microsoft.AspNetCore.SignalR;
using SME.SGP.Infra;
using SME.SGP.Notificacoes.Hub.Interface;
using System.Threading.Tasks;

namespace SME.SGP.Notificacoes.Hub
{
    public class EventoNotificacaoCriada : EventoNotificacao<MensagemCriacaoNotificacaoDto>, IEventoNotificacaoCriada
    {
        public EventoNotificacaoCriada(IServicoTelemetria servicoTelemetria) 
            : base(servicoTelemetria, "Criada")
        {
        }

        protected override Task Disparar(IHubCallerClients clients, MensagemCriacaoNotificacaoDto mensagem)
            => clients.Usuario(mensagem.UsuarioRf)?.SendAsync("NotificacaoCriada", mensagem.Codigo, mensagem.Data, mensagem.Titulo, mensagem.Id);
    }
}
