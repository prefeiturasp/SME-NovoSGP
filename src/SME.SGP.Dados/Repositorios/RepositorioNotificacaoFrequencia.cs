using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioNotificacaoFrequencia : RepositorioBase<NotificacaoFrequencia>, IRepositorioNotificacaoFrequencia
    {
        public RepositorioNotificacaoFrequencia(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }
    }
}