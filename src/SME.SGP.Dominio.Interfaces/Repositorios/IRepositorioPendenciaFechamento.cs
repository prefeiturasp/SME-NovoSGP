using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioPendenciaFechamento : IRepositorioBase<PendenciaFechamento>
    {
        Task<PaginacaoResultadoDto<PendenciaFechamentoResumoDto>> ListarPaginada(Paginacao paginacao, string turmaCodigo, int bimestre, long componenteCurricularId);
        Task<PendenciaFechamentoCompletoDto> ObterPorPendenciaId(long pendenciaId);
        bool VerificaPendenciasAbertoPorFechamento(long fechamentoId);
        Task<Turma> ObterTurmaPorPendenciaId(long pendenciaId);
        Task<IEnumerable<PendenciaFechamento>> ObterPorFechamentoIdDisciplinaId(long fechamentoId, long disciplinaId);

        Task RemoverAsync(PendenciaFechamento pendencia);
        Task<bool> ExistePendenciaFechamentoPorPendenciaId(long pendenciaId);
        Task<IEnumerable<PendenciaParaFechamentoConsolidadoDto>> ObterPendenciasParaFechamentoConsolidado(long turmaId, int bimestre, long componenteCurricularId);
    }
}
