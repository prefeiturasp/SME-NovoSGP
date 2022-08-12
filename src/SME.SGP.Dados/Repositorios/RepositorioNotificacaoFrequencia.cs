using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioNotificacaoFrequencia : RepositorioBase<NotificacaoFrequencia>, IRepositorioNotificacaoFrequencia
    {
        public RepositorioNotificacaoFrequencia(ISgpContext database) : base(database)
        {
        }
    }
}