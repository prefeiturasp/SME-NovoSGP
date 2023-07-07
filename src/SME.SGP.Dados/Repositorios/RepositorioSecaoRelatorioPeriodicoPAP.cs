using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioSecaoRelatorioPeriodicoPAP : RepositorioBase<SecaoRelatorioPeriodicoPAP>, IRepositorioSecaoRelatorioPeriodicoPAP
    {
        public RepositorioSecaoRelatorioPeriodicoPAP(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }
    }
}
