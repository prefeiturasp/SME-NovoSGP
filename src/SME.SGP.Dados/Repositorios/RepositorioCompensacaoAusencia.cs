using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Dados
{
    public class RepositorioCompensacaoAusencia : RepositorioBase<CompensacaoAusencia>, IRepositorioCompensacaoAusencia
    {
        public RepositorioCompensacaoAusencia(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }        
    }
}
