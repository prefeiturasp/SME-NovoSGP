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

        protected override async Task DispararAsync(IHubCallerClients clients, MensagemCriacaoNotificacaoDto mensagem)
        {
            var iClientProxy = await clients.UsuarioAsync(mensagem.UsuarioRf);
            await iClientProxy?.SendAsync("NotificacaoCriada", mensagem.Codigo, mensagem.Data,
                mensagem.Titulo, mensagem.Id);
        }



    }
}
