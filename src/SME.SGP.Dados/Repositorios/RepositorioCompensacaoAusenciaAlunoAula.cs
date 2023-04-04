using SME.SGP.Dados.Repositorios;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interface;

namespace SME.SGP.Dados
{
    public class RepositorioCompensacaoAusenciaAlunoAula : RepositorioBase<CompensacaoAusenciaAlunoAula>, IRepositorioCompensacaoAusenciaAlunoAula
    {
        public RepositorioCompensacaoAusenciaAlunoAula(ISgpContext database, IServicoAuditoria servicoAuditoria) : base(database, servicoAuditoria)
        {
        }
    }
}
