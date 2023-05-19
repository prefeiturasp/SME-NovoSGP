using Microsoft.AspNetCore.SignalR;

namespace SME.SGP.Notificacoes.Hub
{
    public static class EventoNotificacaoExtensions
    {
        private static IRepositorioUsuario repositorioUsuario;

        public static void Inicializa(IRepositorioUsuario repositorioUsuarioObj)
            => repositorioUsuario = repositorioUsuarioObj;

        public static IClientProxy Usuario(this IHubCallerClients clients, string usuarioRf)
            => clients.Clients(repositorioUsuario.Obter(usuarioRf).Result);
    }
}
