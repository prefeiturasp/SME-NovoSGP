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
        private readonly HubConnection conexao;
        private readonly string endpoint;
        private readonly string token;

        public NotificacaoSgpHub(IOptions<HubOptions> hubOptions)
        {
            endpoint = hubOptions.Value.Endpoint;
            token = hubOptions.Value.HubNotificacoesToken;

            conexao = new HubConnectionBuilder()
                .WithUrl($"{endpoint}/notificacao", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(token);
                })
                .WithAutomaticReconnect(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(3),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10)
                })
                .Build();

            conexao.Reconnecting += error =>
            {
                Console.WriteLine("SignalR: reconectando...");
                return Task.CompletedTask;
            };

            conexao.Reconnected += connectionId =>
            {
                Console.WriteLine("SignalR: reconectado.");
                return Task.CompletedTask;
            };

            conexao.Closed += async error =>
            {
                Console.WriteLine("SignalR: conexão fechada. Tentando reiniciar...");
                await TentarIniciarConexaoAsync();
            };

            _ = TentarIniciarConexaoAsync();
        }

        private async Task TentarIniciarConexaoAsync()
        {
            try
            {
                if (conexao.State == HubConnectionState.Disconnected)
                    await conexao.StartAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao iniciar conexão SignalR: {ex.Message}");
            }
        }

        private async Task GarantirConexaoAtivaAsync()
        {
            if (conexao.State == HubConnectionState.Connected)
                return;

            // Aguarda até reconectar ou força start
            var timeout = TimeSpan.FromSeconds(30);
            var inicio = DateTime.UtcNow;

            while (conexao.State != HubConnectionState.Connected)
            {
                if (DateTime.UtcNow - inicio > timeout)
                    throw new TimeoutException("Conexão com SignalR não está ativa.");

                if (conexao.State == HubConnectionState.Disconnected)
                    await TentarIniciarConexaoAsync();

                await Task.Delay(500);
            }
        }

        public async Task Criada(MensagemRabbit mensagem)
        {
            var mensagemCriacao = mensagem.ObterObjetoMensagem<MensagemCriacaoNotificacaoDto>();
            await GarantirConexaoAtivaAsync();
            await conexao.InvokeAsync("Criada", mensagemCriacao);
        }

        public async Task Excluida(MensagemRabbit mensagem)
        {
            var mensagemExclusao = mensagem.ObterObjetoMensagem<MensagemExclusaoNotificacaoDto>();
            await GarantirConexaoAtivaAsync();
            await conexao.InvokeAsync("Excluida", mensagemExclusao);
        }

        public async Task Lida(MensagemRabbit mensagem)
        {
            var mensagemLeitura = mensagem.ObterObjetoMensagem<MensagemLeituraNotificacaoDto>();
            await GarantirConexaoAtivaAsync();
            await conexao.InvokeAsync("Lida", mensagemLeitura);
        }
    }
}
