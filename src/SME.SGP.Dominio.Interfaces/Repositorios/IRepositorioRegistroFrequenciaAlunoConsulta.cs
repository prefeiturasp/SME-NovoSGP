using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioRegistroFrequenciaAlunoConsulta
    {
        Task<IEnumerable<FrequenciaAlunoSimplificadoDto>> ObterFrequenciasPorAulaId(long aulaId);
        Task<IEnumerable<RegistroFrequenciaAluno>> ObterRegistrosAusenciaPorAula(long aulaId);
        Task<IEnumerable<RegistroFrequenciaAluno>> ObterRegistrosFrequenciaPorAulaId(long aulaId);
        Task<IEnumerable<RegistroFrequenciaPorDisciplinaAlunoDto>> ObterRegistroFrequenciaAlunosPorAlunosETurmaIdEDataAula(DateTime dataAula, string[] turmasId, IEnumerable<string> alunos);
        Task<IEnumerable<RegistroFrequenciaGeralPorDisciplinaAlunoTurmaDataDto>> ObterFrequenciaAlunosGeralPorAnoQuery(int ano);
        Task<IEnumerable<RegistroFrequenciaAluno>> ObterRegistrosAusenciaPorAulaAsync(long aulaId);
        Task<IEnumerable<FrequenciaAlunoAulaDto>> ObterFrequenciasDoAlunoNaAula(string codigoAluno, long aulaId);
        Task<int> ObterTotalAulasPorDisciplinaETurma(DateTime dataAula, string[] disciplinaIdsConsideradas, DateTime? dataMatriculaAluno = null, DateTime? dataSituacaoAluno = null, string professor = null, params string[] turmasId);
        Task<IEnumerable<RegistroFrequenciaAlunoPorTurmaEMesDto>> ObterRegistroFrequenciaAlunosPorTurmaEMes(string turmaCodigo, int mes);
        Task<IEnumerable<RegistroFrequenciaAluno>> ObterRegistrosAusenciaPorIdRegistro(long registroFrequenciaId);
        Task<IEnumerable<FrequenciaAlunoTurmaDto>> ObterRegistroFrequenciaAlunosNaTurma(string turmaCodigo, string alunoCodigo);
    }
}
