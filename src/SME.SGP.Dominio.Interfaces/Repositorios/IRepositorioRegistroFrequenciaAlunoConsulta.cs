using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRegistroFrequenciaAlunoConsulta
    {
        Task<IEnumerable<AusenciaPorDisciplinaAlunoDto>> ObterAusenciasAlunosPorAlunosETurmaIdEDataAula(DateTime dataAula, IEnumerable<string> codigoAlunos, params string[] turmasId);
        Task<IEnumerable<FrequenciaAlunoSimplificadoDto>> ObterFrequenciasPorAulaId(long aulaId);
        Task<IEnumerable<RegistroFrequenciaAluno>> ObterRegistrosAusenciaPorAula(long aulaId);
        Task<IEnumerable<RegistroFrequenciaPorDisciplinaAlunoDto>> ObterRegistroFrequenciaAlunosPorAlunosETurmaIdEDataAula(DateTime dataAula, string[] turmasId, IEnumerable<string> alunos);
        Task<IEnumerable<RegistroFrequenciaGeralPorDisciplinaAlunoTurmaDataDto>> ObterFrequenciaAlunosGeralPorAnoQuery(int ano);
        Task<IEnumerable<RegistroFrequenciaAluno>> ObterRegistrosAusenciaPorAulaAsync(long aulaId);
        Task<IEnumerable<FrequenciaAlunoAulaDto>> ObterFrequenciasDoAlunoNaAula(string codigoAluno, long aulaId);
        Task<int> ObterTotalAulasPorDisciplinaETurma(DateTime dataAula, string disciplinaId, params string[] turmasId);
    }
}
