using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Infra;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioCompensacaoAusenciaAlunoAulaConsulta : IRepositorioBase<CompensacaoAusenciaAlunoAula>
    {
        Task<IEnumerable<CompensacaoAusenciaAlunoAulaDto>> ObterCompensacoesAusenciasAlunoEAulaPorAulaIdTurmaComponenteQuantidade(long aulaId, int quantidade);
    }
}
