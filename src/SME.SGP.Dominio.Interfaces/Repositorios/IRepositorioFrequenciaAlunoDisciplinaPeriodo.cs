using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFrequenciaAlunoDisciplinaPeriodo : IRepositorioBase<FrequenciaAluno>
    {
        FrequenciaAluno Obter(string codigoAluno, string disciplinaId, long periodoEscolarId, TipoFrequenciaAluno tipoFrequencia, string turmaId);
        
        Task<FrequenciaAluno> ObterAsync(string codigoAluno, string disciplinaId, long periodoEscolarId, TipoFrequenciaAluno tipoFrequencia, string turmaId);

        FrequenciaAluno ObterPorAlunoData(string codigoAluno, DateTime dataAtual, TipoFrequenciaAluno tipoFrequencia, string disciplinaId = "", string codigoTurma = "");
        
        Task<FrequenciaAluno> ObterPorAlunoDataAsync(string codigoAluno, DateTime dataAtual, TipoFrequenciaAluno tipoFrequencia, string disciplinaId = "", string codigoTurma = "");

        Task<FrequenciaAluno> ObterPorAlunoBimestreAsync(string codigoAluno, int bimestre, TipoFrequenciaAluno tipoFrequencia, string codigoTurma, string disciplinaId = "");

        FrequenciaAluno ObterPorAlunoDisciplinaData(string codigoAluno, string disciplinaId, DateTime dataAtual);

        IEnumerable<FrequenciaAluno> ObterAlunosComAusenciaPorDisciplinaNoPeriodo(long periodoId, bool eja);
        Task<IEnumerable<FrequenciaAlunoDto>> ObterFrequenciaGeralPorTurma(string turmaCodigo);
        IEnumerable<AlunoFaltosoBimestreDto> ObterAlunosFaltososBimestre(ModalidadeTipoCalendario modalidade, double percentualFrequenciaMinimo, int bimestre, int? anoLetivo);
        Task<IEnumerable<FrequenciaAluno>> ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolar(string codigoTurma, string componenteCurricularId, TipoFrequenciaAluno tipoFrequencia, IEnumerable<long> periodosEscolaresIds);
        Task<IEnumerable<FrequenciaAluno>> ObterFrequenciaGeralAluno(string alunoCodigo, string turmaCodigo, string componenteCurricularCodigo = "");

        Task<IEnumerable<FrequenciaAluno>> ObterFrequenciaBimestresAsync(string codigoAluno, int bimestre, string codigoTurma, TipoFrequenciaAluno tipoFrequencia = TipoFrequenciaAluno.PorDisciplina);

        

        Task<IEnumerable<FrequenciaAluno>> ObterPorAlunosAsync(IEnumerable<string> alunosCodigo, IEnumerable<long?> periodosEscolaresId, string turmaId);
        Task SalvarVariosAsync(IEnumerable<FrequenciaAluno> entidades);
          
        Task<IEnumerable<FrequenciaAluno>> ObterFrequenciaGeralAlunoPorAnoModalidadeSemestre(string alunoCodigo, int anoTurma, long tipoCalendarioId);
        Task<FrequenciaAluno> ObterPorAlunoDataTurmasAsync(string codigoAluno, DateTime dataAtual, TipoFrequenciaAluno tipoFrequencia, string[] turmasCodigo, string disciplinaId = "");
        Task<IEnumerable<FrequenciaAluno>> ObterPorAlunoTurmasDisciplinasDataAsync(string codigoAluno, TipoFrequenciaAluno tipoFrequencia,
            string[] disciplinasId, string[] turmasCodigo, int[] bimestres);
        Task<IEnumerable<FrequenciaAluno>> ObterFrequenciasAlunosPorCodigoAlunoCodigoComponentesTurmaAsync(string alunoCodigo, string[] turmasCodigos, string[] componenteCurricularCodigos);
        Task RemoverVariosAsync(long[] idsParaRemover);
        Task<bool> ExisteFrequenciaRegistradaPorTurmaComponenteCurricular(string codigoTurma, string componenteCurricularId, long periodoEscolarId);
        Task RemoverFrequenciaGeralAlunos(string[] alunos, string turmaCodigo, long periodoEscolarId);
        Task RemoverFrequenciasDuplicadas(string[] alunos, string turmaCodigo, long periodoEscolarId);
        Task<int> ObterTotalAulasPorDisciplinaETurmaAsync(DateTime dataAula, string disciplinaId, string turmaId);

        Task<IEnumerable<RegistroFrequenciaAlunoBimestreDto>> ObterFrequenciasRegistradasPorTurmasComponentesCurriculares(string codigoAluno, string[] codigosTurma, string[] componentesCurricularesId, long? periodoEscolarId);
        Task<IEnumerable<FrequenciaAluno>> ObterPorAlunosDataAsync(string[] alunosCodigo, DateTime dataAtual, TipoFrequenciaAluno tipoFrequencia, string codigoTurma, string componenteCurricularId);

        Task<IEnumerable<FrequenciaAluno>> ObterPorAlunoTurmaComponenteBimestres(string codigoAluno, TipoFrequenciaAluno tipoFrequencia,
           long componenteCurricularId, string turmaCodigo, int[] bimestres);
    }
}