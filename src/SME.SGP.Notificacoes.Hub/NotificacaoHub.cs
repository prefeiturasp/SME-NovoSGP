using DocumentFormat.OpenXml.Office2010.ExcelAc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MongoDB.Driver.Core.Connections;
using SME.SGP.Notificacoes.Hub.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SME.SGP.Notificacoes.Hub
{
    //[Authorize(AuthenticationSchemes = Startup.CustomTokenScheme)]
    public class NotificacaoHub : Microsoft.AspNetCore.SignalR.Hub, IEventosNotificacao
    {
        private readonly IEventoNotificacaoCriada eventoCriada;
        private readonly IEventoNotificacaoLida eventoLida;
        private readonly IEventoNotificacaoExcluida eventoExcluida;
        private readonly IRepositorioUsuario repositorioUsuario;
        private readonly int limiteConexoes = 10;

        private readonly List<string> listaUsuarios = new List<string>();

        public NotificacaoHub(
            IEventoNotificacaoCriada eventoCriada,
            IEventoNotificacaoLida eventoLida,
            IEventoNotificacaoExcluida eventoExcluida,
            IRepositorioUsuario repositorioUsuario)
        {
            this.eventoCriada = eventoCriada ?? throw new System.ArgumentNullException(nameof(eventoCriada));
            this.eventoLida = eventoLida ?? throw new System.ArgumentNullException(nameof(eventoLida));
            this.eventoExcluida = eventoExcluida ?? throw new System.ArgumentNullException(nameof(eventoExcluida));
            this.repositorioUsuario = repositorioUsuario ?? throw new System.ArgumentNullException(nameof(repositorioUsuario));
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

            listaUsuarios.Add(Context.ConnectionId);

            if (listaUsuarios.Count > limiteConexoes)
            {
                int posicaoFila = listaUsuarios.IndexOf(Context.ConnectionId);
                await Clients.Client(Context.ConnectionId).SendAsync("BloqueioUsuario", posicaoFila - limiteConexoes);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(System.Exception exception)
        {
            var context = Context.GetHttpContext();
            var usuarioRf = context.User.Claims.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name))?.Value;
            if (!string.IsNullOrEmpty(usuarioRf))
                await repositorioUsuario.Excluir(usuarioRf);

            int posicaoFila = listaUsuarios.IndexOf(Context.ConnectionId);
            listaUsuarios.Remove(Context.ConnectionId);

            if (posicaoFila < limiteConexoes)
            {
                var proximoFila = listaUsuarios.ElementAtOrDefault(limiteConexoes);

                if (proximoFila != null)
                    await Clients.Client(proximoFila).SendAsync("DesbloqueioUsuario");

                listaUsuarios.Skip(limiteConexoes).ToList().ForEach(async connectionId =>
                {
                    int novaPosicaoFila = listaUsuarios.IndexOf(connectionId);
                    await Clients.Client(connectionId).SendAsync("BloqueioUsuario", novaPosicaoFila - limiteConexoes);
                });
            }

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

    }
}
