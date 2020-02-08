using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRecuperacaoParalela : IRepositorioBase<RecuperacaoParalela>
    {
        Task<IEnumerable<RetornoRecuperacaoParalela>> Listar(string turmaId, long periodoId);

        Task<IEnumerable<RetornoRecuperacaoParalelaTotalAlunosAnoDto>> ListarTotalAlunosSeries(long dreId, long ueId, int cicloId, int turmaId, int ano);

        Task<IEnumerable<RetornoRecuperacaoParalelaTotalAlunosAnoFrequenciaDto>> ListarTotalEstudantesPorFrequencia(long dreId, long ueId, int cicloId, int turmaId, int ano);
    }
}