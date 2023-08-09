using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace SME.SGP.Notificacoes.Hub
{
    public static class EventoNotificacaoExtensions
    {
        private static IRepositorioUsuario repositorioUsuario;

        public static void Inicializa(IRepositorioUsuario repositorioUsuarioObj)
            => repositorioUsuario = repositorioUsuarioObj;

        public static async Task<IClientProxy> UsuarioAsync(this IHubCallerClients clients, string usuarioRf)
        {
            var usuario = await repositorioUsuario.Obter(usuarioRf);
            return clients.Client(usuario);
        }
    }
}
