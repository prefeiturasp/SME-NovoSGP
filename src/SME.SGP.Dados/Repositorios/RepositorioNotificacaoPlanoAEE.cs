using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioNotificacaoPlanoAEE : RepositorioBase<NotificacaoPlanoAEE>, IRepositorioNotificacaoPlanoAEE
    {
        public RepositorioNotificacaoPlanoAEE(ISgpContext database) : base(database)
        {
        }
    }
}
