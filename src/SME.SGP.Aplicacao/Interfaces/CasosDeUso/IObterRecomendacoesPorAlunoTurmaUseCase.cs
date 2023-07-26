using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterRecomendacoesPorAlunoTurmaUseCase 
    {
        Task<IEnumerable<RecomendacaoConselhoClasseAlunoDTO>> Executar(FiltroRecomendacaoConselhoClasseAlunoTurmaDto filtro);
    }
}