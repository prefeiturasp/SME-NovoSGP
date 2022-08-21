using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SME.SGP.Notificacao.Worker
{
    public class NotificacaoHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        [Action(RotasRabbitNotificacao.Criacao, "Notiicar Criação de Notificação")]
        public async Task Criada(MensagemRabbit mensagem)
        {
            var mensagemCriacao = mensagem.ObterObjetoMensagem<MensagemCriacaoNotificacaoDto>();

            await Clients.All.SendAsync("ReceiveCriada", mensagemCriacao.UsuarioRf, mensagemCriacao.Titulo);
        }

        [Action(RotasRabbitNotificacao.Leitura, "Notiicar Criação de Notificação")]
        public async Task Leitura(MensagemRabbit mensagem)
        {
            var mensagemLeitura = mensagem.ObterObjetoMensagem<MensagemLeituraNotificacaoDto>();

            await Clients.All.SendAsync("ReceiveLeitura", mensagemLeitura.Codigo);
        }

        [Action(RotasRabbitNotificacao.Exclusao, "Notiicar Criação de Notificação")]
        public async Task Excluida(MensagemRabbit mensagem)
        {
            var mensagemExclusao = mensagem.ObterObjetoMensagem<MensagemExclusaoNotificacaoDto>();

            await Clients.All.SendAsync("ReceiveExcluida", mensagemExclusao.Codigo);
        }

    }
}
