using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta
    {
        Task<IEnumerable<FrequenciaAluno>> ObterPorAlunos(IEnumerable<string> alunosCodigo, IEnumerable<long?> periodosEscolaresId, string turmaId);
    }
}
