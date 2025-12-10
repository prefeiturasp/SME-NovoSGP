using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SME.SGP.Notificacoes.Hub.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SME.SGP.Notificacoes.Hub
{
    //[Authorize(AuthenticationSchemes = Startup.CustomTokenScheme)]
    public class NotificacaoHub : Microsoft.AspNetCore.SignalR.Hub, IEventosNotificacao
    {
        private readonly IConfiguration configuration;
        private readonly IEventoNotificacaoCriada eventoCriada;
        private readonly IEventoNotificacaoLida eventoLida;
        private readonly IEventoNotificacaoExcluida eventoExcluida;
        private readonly IRepositorioUsuario repositorioUsuario;
        private readonly int limiteConexoes = 2;
        private readonly ILogger<NotificacaoHub> logger;

        private readonly static List<string> listaUsuarios = new List<string>();

        public NotificacaoHub(
            IEventoNotificacaoCriada eventoCriada,
            IEventoNotificacaoLida eventoLida,
            IEventoNotificacaoExcluida eventoExcluida,
            IRepositorioUsuario repositorioUsuario,
            ILogger<NotificacaoHub> logger,
            IConfiguration configuration)
        {
            this.eventoCriada = eventoCriada ?? throw new System.ArgumentNullException(nameof(eventoCriada));
            this.eventoLida = eventoLida ?? throw new System.ArgumentNullException(nameof(eventoLida));
            this.eventoExcluida = eventoExcluida ?? throw new System.ArgumentNullException(nameof(eventoExcluida));
            this.repositorioUsuario = repositorioUsuario ?? throw new System.ArgumentNullException(nameof(repositorioUsuario));
            this.logger = logger;
            this.configuration = configuration;

            this.limiteConexoes =  int.TryParse(configuration.GetSection("SGP_MaxConnections").Value, out var value) ? value : 100000;
        }

        public override async Task OnConnectedAsync()
        {
            var context = Context.GetHttpContext();
            var queryUsuario = context.Request.Query["usuarioRf"];
            if (queryUsuario.Any())
            {
                var usuarioRf = queryUsuario.First();
                context.User = new(new ClaimsIdentity(new Claim[] { new Claim(ClaimTypes.Name, usuarioRf) }));
                await ArmazenaConexaoUsuario(usuarioRf, Context.ConnectionId);
            }

            await AdicionarClienteNaListaUsuarios();

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(System.Exception exception)
        {
            var context = Context.GetHttpContext();
            var usuarioRf = context.User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name))?.Value;
            if (!string.IsNullOrEmpty(usuarioRf))
                await repositorioUsuario.Excluir(usuarioRf);

            await RemoverClienteDaListaUsuarios();

            await base.OnDisconnectedAsync(exception);
        }

        private Task ArmazenaConexaoUsuario(string usuarioRf, string connectionId)
            => repositorioUsuario.Salvar(usuarioRf, connectionId);

        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        public Task Criada(MensagemCriacaoNotificacaoDto mensagem)
            => eventoCriada.EnviarAsync(Clients, mensagem);

        public Task Lida(MensagemLeituraNotificacaoDto mensagem)
            => eventoLida.EnviarAsync(Clients, mensagem);

        public Task Excluida(MensagemExclusaoNotificacaoDto mensagem)
            => eventoExcluida.EnviarAsync(Clients, mensagem);

        [Authorize("Token")]
        public object TokenProtected()
        {
            return CompileResult();
        }

        private object CompileResult() =>
            new
            {
                UserId = Context.UserIdentifier,
                Claims = Context.User.Claims.Select(x => new { x.Type, x.Value })
            };


        private async Task AdicionarClienteNaListaUsuarios()
        {
            try
            {
                listaUsuarios.Add(Context.ConnectionId);

                if (listaUsuarios.Count > limiteConexoes)
                {
                    int posicaoFila = listaUsuarios.IndexOf(Context.ConnectionId);
                    await Clients.Client(Context.ConnectionId).SendAsync("BloqueioUsuario", (posicaoFila + 1) - limiteConexoes);
                }

                logger.LogInformation($"Usuário conectado. ConnectionId: {Context.ConnectionId}. Total de conexões: {listaUsuarios.Count}/{limiteConexoes}.");
            }
            catch (Exception ex)
            {
                logger.LogError($"Erro ao conectar usuário. ConnectionId: {Context.ConnectionId}.", ex.Message);
            }
        }

        private async Task RemoverClienteDaListaUsuarios()
        {
            try
            {
                int posicaoFila = listaUsuarios.IndexOf(Context.ConnectionId);
                listaUsuarios.Remove(Context.ConnectionId);

                if (posicaoFila < limiteConexoes)
                {
                    var proximoFila = listaUsuarios.ElementAtOrDefault(limiteConexoes - 1);

                    if (proximoFila != null)
                        await Clients.Client(proximoFila).SendAsync("DesbloqueioUsuario");

                    listaUsuarios.Skip(limiteConexoes).ToList().ForEach(async connectionId =>
                    {
                        int novaPosicaoFila = listaUsuarios.IndexOf(connectionId);
                        await Clients.Client(connectionId).SendAsync("BloqueioUsuario", (novaPosicaoFila + 1) - limiteConexoes);
                    });
                }
                else
                {
                    listaUsuarios.Skip(posicaoFila).ToList().ForEach(async connectionId =>
                    {
                        int novaPosicaoFila = listaUsuarios.IndexOf(connectionId);
                        await Clients.Client(connectionId).SendAsync("BloqueioUsuario", (novaPosicaoFila + 1) - limiteConexoes);
                    });
                }

                logger.LogInformation($"Usuário desconectado. ConnectionId: {Context.ConnectionId}. Total de conexões: {listaUsuarios.Count}/{limiteConexoes}.");
            }
            catch (Exception ex)
            {
                logger.LogError($"Erro ao desconectar usuário. ConnectionId: {Context.ConnectionId}.", ex.Message);
            }
        }
    }
}