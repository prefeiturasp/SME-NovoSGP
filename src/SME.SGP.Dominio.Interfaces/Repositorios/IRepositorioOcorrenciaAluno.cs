using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public interface IRepositorioOcorrenciaAluno
    {
        Task<IEnumerable<string>> ObterAlunosPorOcorrencia(long ocorrenciaId);
        Task ExcluirAsync(IEnumerable<long> idsOcorrenciasAlunos);
        Task<long> SalvarAsync(OcorrenciaAluno entidade);
    }
}
