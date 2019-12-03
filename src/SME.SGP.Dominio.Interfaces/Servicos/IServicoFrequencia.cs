using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IServicoFrequencia
    {
        Task<IEnumerable<DisciplinaDto>> ObterDisciplinasLecionadasPeloProfessorPorTurma(string turmaId);

        IEnumerable<RegistroAusenciaAluno> ObterListaAusenciasPorAula(long aulaId);

        RegistroFrequencia ObterRegistroFrequenciaPorAulaId(long aulaId);

        Task Registrar(long aulaId, IEnumerable<RegistroAusenciaAluno> registroAusenciaAlunos);
        Task ExcluirFrequenciaAula(long aulaId);
    }
}