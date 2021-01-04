using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public interface IRepositorioOcorrenciaAluno
    {
        Task<IEnumerable<string>> ObterAlunosPorOcorrencia(long ocorrenciaId);
        Task<long> SalvarAsync(OcorrenciaAluno entidade);
    }
}
