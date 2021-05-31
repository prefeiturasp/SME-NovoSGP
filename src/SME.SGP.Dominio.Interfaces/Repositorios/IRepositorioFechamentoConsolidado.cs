using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public interface IRepositorioFechamentoConsolidado : IRepositorioBase<FechamentoConsolidadoComponenteTurma>
    {
        Task<IEnumerable<FechamentoConsolidadoComponenteTurma>> ObterFechamentosConsolidadoPorTurmaBimestre(long turmaId, int bimestre);
        Task<FechamentoConsolidadoComponenteTurma> ObterFechamentoConsolidadoPorTurmaBimestreComponenteCurricularAsync(long turmaId, long componenteCurricularId, int bimestre);
        Task<IEnumerable<ConsolidacaoTurmaComponenteCurricularDto>> ObterComponentesFechamentoConsolidadoPorTurmaBimestre(long turmaId, int bimestre);
    }
}
