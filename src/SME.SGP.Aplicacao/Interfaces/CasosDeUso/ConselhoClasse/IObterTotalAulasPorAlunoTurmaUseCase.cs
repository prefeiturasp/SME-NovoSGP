using SME.SGP.Infra.Dtos.ConselhoClasse;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterTotalAulasPorAlunoTurmaUseCase
    {
        Task<IEnumerable<TotalAulasPorAlunoTurmaDto>> Executar(string codigoAluno, string codigoTurma);
    }
}
