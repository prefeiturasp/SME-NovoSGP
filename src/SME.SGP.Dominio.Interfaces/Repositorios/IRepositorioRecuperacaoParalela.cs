using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRecuperacaoParalela : IRepositorioBase<RecuperacaoParalela>
    {
        Task<IEnumerable<RetornoRecuperacaoParalela>> Listar(long turmaId, long periodoId);

        Task<IEnumerable<RetornoRecuperacaoParalelaTotalAlunosAnoDto>> ListarTotalAlunosSeries(int? periodo, string dreId, string ueId, int? cicloId, string turmaId, string ano, int anoLetivo);

        Task<IEnumerable<RetornoRecuperacaoParalelaTotalAlunosAnoFrequenciaDto>> ListarTotalEstudantesPorFrequencia(int? periodo, string dreId, string ueId, int? cicloId, string turmaId, string ano, int anoLetivo);

        Task<PaginacaoResultadoDto<RetornoRecuperacaoParalelaTotalResultadoDto>> ListarTotalResultado(int? periodo, string dreId, string ueId, int? cicloId, string turmaId, string ano, int anoLetivo, int? pagina);

        Task<IEnumerable<RetornoRecuperacaoParalelaTotalResultadoDto>> ListarTotalResultadoEncaminhamento(int? periodo, string dreId, string ueId, int? cicloId, string turmaId, string ano, int anoLetivo, int? pagina);
    }
}