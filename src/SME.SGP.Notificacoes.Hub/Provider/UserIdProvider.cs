using Microsoft.AspNetCore.SignalR;
using System.Linq;

namespace SME.SGP.Notificacoes.Hub
{
    public class UserIdProvider : IUserIdProvider
    {
        public UserIdProvider()
        {

        }

        public string GetUserId(HubConnectionContext connection)
            => connection.User?.Claims
                .FirstOrDefault(x => x.Type.Equals("usuario_rf"))?.Value;
    }
}
