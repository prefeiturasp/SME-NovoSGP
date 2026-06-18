using SME.SGP.Infra.Dtos.ConselhoClasse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterTotalAulasSemFrequenciaPorTurmaUseCase
    {
        Task<IEnumerable<TotalAulasPorAlunoTurmaDto>> Executar(string disciplinaId, string codigoTurma);
    }
}
