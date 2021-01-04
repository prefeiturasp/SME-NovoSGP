using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio
{
    public interface IRepositorioOcorrenciaAluno : IRepositorioBase<OcorrenciaAluno>
    {
        Task<IEnumerable<string>> ObterAlunosPorOcorrencia(long ocorrenciaId);
    }
}
