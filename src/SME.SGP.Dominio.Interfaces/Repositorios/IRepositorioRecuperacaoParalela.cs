using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRecuperacaoParalela : IRepositorioBase<RecuperacaoParalela>
    {
        Task<IEnumerable<RetornoRecuperacaoParalela>> Listar(long turmaId, long periodoId);

        Task<IEnumerable<RetornoRecuperacaoParalelaTotalAlunosAnoDto>> ListarTotalAlunosSeries(FiltroRecuperacaoParalelaResumoDto filtro);

        Task<IEnumerable<RetornoRecuperacaoParalelaTotalAlunosAnoFrequenciaDto>> ListarTotalEstudantesPorFrequencia(FiltroRecuperacaoParalelaResumoDto filtro);

        Task<PaginacaoResultadoDto<RetornoRecuperacaoParalelaTotalResultadoDto>> ListarTotalResultado(FiltroRecuperacaoParalelaResumoDto filtro);

        Task<IEnumerable<RetornoRecuperacaoParalelaTotalResultadoDto>> ListarTotalResultadoEncaminhamento(FiltroRecuperacaoParalelaResumoDto filtro);
    }
}