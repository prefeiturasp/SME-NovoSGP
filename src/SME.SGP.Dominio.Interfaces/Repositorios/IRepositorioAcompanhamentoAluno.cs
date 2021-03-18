using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioAcompanhamentoAluno : IRepositorioBase<AcompanhamentoAluno>
    {
        Task<IEnumerable<AcompanhamentoAlunoTurmaSemestreDto>> ObterAcompanhamentoPorTurmaAlunoESemestre(long turmaId, string alunoCodigo, int semestre);
    }
}
