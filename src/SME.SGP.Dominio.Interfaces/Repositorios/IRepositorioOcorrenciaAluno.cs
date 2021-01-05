using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioOcorrenciaAluno
    {
        Task<IEnumerable<string>> ObterAlunosPorOcorrencia(long ocorrenciaId);

        Task ExcluirAsync(IEnumerable<long> idsOcorrenciasAlunos);

        Task<long> SalvarAsync(OcorrenciaAluno entidade);
    }
}