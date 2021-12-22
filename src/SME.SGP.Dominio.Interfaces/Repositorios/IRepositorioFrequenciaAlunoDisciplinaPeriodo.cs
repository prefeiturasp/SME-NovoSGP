using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFrequenciaAlunoDisciplinaPeriodo : IRepositorioBase<FrequenciaAluno>
    {
        Task SalvarVariosAsync(IEnumerable<FrequenciaAluno> entidades);
          
        Task<IEnumerable<FrequenciaAluno>> ObterFrequenciaGeralAlunoPorAnoModalidadeSemestre(string alunoCodigo, int anoTurma, long tipoCalendarioId);
        Task<IEnumerable<FrequenciaAluno>> ObterFrequenciaGeralAlunoPorTurmas(string alunoCodigo, string[] codigosTurmas, long tipoCalendarioId);
        Task<FrequenciaAluno> ObterPorAlunoDataTurmasAsync(string codigoAluno, DateTime dataAtual, TipoFrequenciaAluno tipoFrequencia, string[] turmasCodigo, string disciplinaId = "");
        Task<IEnumerable<FrequenciaAluno>> ObterPorAlunoTurmasDisciplinasDataAsync(string codigoAluno, TipoFrequenciaAluno tipoFrequencia,
            string[] disciplinasId, string[] turmasCodigo, int[] bimestres);
        Task<IEnumerable<FrequenciaAluno>> ObterFrequenciasAlunosPorCodigoAlunoCodigoComponentesTurmaAsync(string alunoCodigo, string[] turmasCodigos, string[] componenteCurricularCodigos);
        Task RemoverVariosAsync(long[] idsParaRemover);
        Task RemoverFrequenciaGeralAlunos(string[] alunos, string turmaCodigo, long periodoEscolarId);
        Task RemoverFrequenciasDuplicadas(string[] alunos, string turmaCodigo, long periodoEscolarId);
    }
}