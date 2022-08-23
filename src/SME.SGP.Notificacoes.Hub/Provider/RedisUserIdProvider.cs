using Microsoft.AspNetCore.SignalR;

namespace SME.SGP.Notificacoes.Hub
{
    public class RedisUserIdProvider : IUserIdProvider
    {
        private readonly IRepositorioUsuario repositorioUsuario;

        public RedisUserIdProvider(IRepositorioUsuario repositorioUsuario)
        {
            this.repositorioUsuario = repositorioUsuario ?? throw new System.ArgumentNullException(nameof(repositorioUsuario));
        }

        public string GetUserId(HubConnectionContext connection)
        {
            var usuarioRf = connection.UserIdentifier;

            return repositorioUsuario.Obter(usuarioRf).Result;
        }
    }
}
