using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Options;
using SME.SGP.Infra;
using SME.SGP.Notificacoes.Hub.Interface;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Notificacoes.Worker
{
    public class NotificacaoSgpHub : INotificacaoSgpHub
    {
        HubConnection conexao;
        public NotificacaoSgpHub(IOptions<HubOptions> hubOptions)
        {
            conexao = new HubConnectionBuilder()
                .WithUrl($"{hubOptions.Value.Endpoint}/notificacao", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(hubOptions.Value.HubNotificacoesToken);
                })
                .WithAutomaticReconnect(new[] { TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10) })
                .Build();

            Iniciar().Wait();
        }

        private Task Iniciar()
            => conexao.StartAsync();

        public async Task Criada(MensagemRabbit mensagem)
        {
            var mensagemCriacao = mensagem.ObterObjetoMensagem<MensagemCriacaoNotificacaoDto>();

            await conexao.InvokeAsync("Criada", mensagemCriacao);
        }

        public Task Excluida(MensagemRabbit mensagem)
        {
            var mensagemExclusao = mensagem.ObterObjetoMensagem<MensagemExclusaoNotificacaoDto>();

            return conexao.InvokeAsync("Excluida", mensagemExclusao);
        }

        public Task Lida(MensagemRabbit mensagem)
        {
            var mensagemLeitura = mensagem.ObterObjetoMensagem<MensagemLeituraNotificacaoDto>();

            return conexao.InvokeAsync("Lida", mensagemLeitura);
        }
    }
}
