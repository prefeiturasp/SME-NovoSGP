using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces.Repositorios;
using SME.SGP.Infra;

namespace SME.SGP.Dados
{
    public class RepositorioNotificacaoPlanoAEEObservacao : RepositorioBase<NotificacaoPlanoAEEObservacao>, IRepositorioNotificacaoPlanoAEEObservacao
    {
        public RepositorioNotificacaoPlanoAEEObservacao(ISgpContext database) : base(database)
        {
        }
    }
}
