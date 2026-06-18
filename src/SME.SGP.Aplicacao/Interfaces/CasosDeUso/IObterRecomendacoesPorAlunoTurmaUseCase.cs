using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterRecomendacoesPorAlunoTurmaUseCase
    {
        Task<IEnumerable<RecomendacaoConselhoClasseAlunoDTO>> Executar(FiltroRecomendacaoConselhoClasseAlunoTurmaDto filtro);
    }
}