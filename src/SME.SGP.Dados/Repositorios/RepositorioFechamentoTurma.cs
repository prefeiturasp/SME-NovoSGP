using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Dados.Repositorios
{
    public class RepositorioFechamentoTurma : RepositorioBase<FechamentoTurma>, IRepositorioFechamentoTurma
    {
        public RepositorioFechamentoTurma(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }  
    }
}
