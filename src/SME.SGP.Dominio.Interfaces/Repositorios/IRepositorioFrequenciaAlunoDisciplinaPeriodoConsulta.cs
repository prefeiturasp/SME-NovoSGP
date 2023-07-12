using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFrequenciaAlunoDisciplinaPeriodoConsulta
    {
        Task<IEnumerable<FrequenciaAluno>> ObterPorAlunos(IEnumerable<string> alunosCodigo, long periodoEscolarId, string turmaId, string[] disciplinaIdsConsideradas, string professor = null);
        FrequenciaAluno Obter(string codigoAluno, string disciplinaId, long periodoEscolarId, TipoFrequenciaAluno tipoFrequencia, string turmaId);
        Task<FrequenciaAluno> ObterAsync(string codigoAluno, string disciplinaId, long periodoEscolarId, TipoFrequenciaAluno tipoFrequencia, string turmaId);
        IEnumerable<FrequenciaAluno> ObterAlunosComAusenciaPorDisciplinaNoPeriodo(long periodoId, bool eja);
        IEnumerable<AlunoFaltosoBimestreDto> ObterAlunosFaltososBimestre(ModalidadeTipoCalendario modalidade, double percentualFrequenciaMinimo, int bimestre, int? anoLetivo);
        Task<IEnumerable<FrequenciaAluno>> ObterFrequenciaGeralAluno(string alunoCodigo, string turmaCodigo, string componenteCurricularCodigo = "");
        Task<IEnumerable<FrequenciaAluno>> ObterFrequenciaGeralPorAlunosETurmas(string[] alunoCodigo, string turmaCodigo);
        Task<IEnumerable<FrequenciaAluno>> ObterFrequenciaGeralAlunoPorTurmas(string codigoAluno, string[] codigosTurmas, long tipoCalendarioId);
        Task<IEnumerable<FrequenciaAluno>> ObterFrequenciaComponentesAlunoPorTurmas(string alunoCodigo, string[] codigosTurmas, long tipoCalendarioId, int bimestre = 0);
        Task<IEnumerable<FrequenciaAluno>> ObterFrequenciaBimestresAsync(string codigoAluno, int bimestre, string codigoTurma, TipoFrequenciaAluno tipoFrequencia = TipoFrequenciaAluno.PorDisciplina);
        Task<FrequenciaAluno> ObterPorAlunoBimestreAsync(string codigoAluno, int bimestre, TipoFrequenciaAluno tipoFrequencia, string codigoTurma, string[] disciplinasId = null, string professor = null);
        FrequenciaAluno ObterPorAlunoData(string codigoAluno, DateTime dataAtual, TipoFrequenciaAluno tipoFrequencia, string disciplinaId = "", string codigoTurma = "");
        Task<FrequenciaAluno> ObterPorAlunoDataAsync(string codigoAluno, DateTime dataAtual, TipoFrequenciaAluno tipoFrequencia, string disciplinaId = "", string codigoTurma = "");
        FrequenciaAluno ObterPorAlunoDisciplinaData(string codigoAluno, string[] disciplinasId, DateTime dataAtual, string turmaCodigo = "", string professor = null);
        FrequenciaAluno ObterPorAlunoDisciplinaPeriodo(string codigoAluno, string[] disciplinaIds, long periodoEscolarId, string turmaCodigo = "", string professor = null);
        Task<IEnumerable<FrequenciaAluno>> ObterFrequenciaPorListaDeAlunosDisciplinaData(string[] codigosAlunos,string[] disciplinaId, long periodoEscolarId, string turmaCodigo = "", string professor = null);
        Task<IEnumerable<FrequenciaAluno>> ObterPorAlunosDisciplinasDataAsync(string[] codigosAlunos, string[] disciplinasIds, DateTime dataAtual, string turmaCodigo = "");
        Task<FrequenciaAluno> ObterPorAlunoDisciplinaDataAsync(string codigoAluno, string disciplinaId, DateTime dataAtual, string turmaCodigo);
        Task<IEnumerable<FrequenciaAluno>> ObterFrequenciaAlunosPorTurmaDisciplinaEPeriodoEscolar(string codigoTurma, string[] componentesCurricularesId, TipoFrequenciaAluno tipoFrequencia, IEnumerable<long> periodosEscolaresIds, string professor = null);
        Task<IEnumerable<FrequenciaAluno>> ObterFrequenciaGeralAlunoPorAnoModalidadeSemestre(string alunoCodigo, int anoTurma, long tipoCalendarioId);
        Task<IEnumerable<FrequenciaAluno>> ObterFrequenciasAlunosPorCodigoAlunoCodigoComponentesTurmaAsync(string alunoCodigo, string[] turmasCodigos, string[] componenteCurricularCodigos);
        Task<IEnumerable<FrequenciaAlunoDto>> ObterFrequenciaGeralPorTurma(string turmaCodigo);
        Task<FrequenciaAluno> ObterPorAlunoDataTurmasAsync(string codigoAluno, DateTime dataAtual, TipoFrequenciaAluno tipoFrequencia, string[] turmasCodigo, string disciplinaId = "");
        Task<IEnumerable<FrequenciaAluno>> ObterPorAlunoTurmasDisciplinasDataAsync(string codigoAluno, TipoFrequenciaAluno tipoFrequencia, string[] disciplinasId, string[] turmasCodigo, int[] bimestres, long[] periodosEscolaresId = null);
        Task<bool> ExisteFrequenciaRegistradaPorTurmaComponenteCurricular(string codigoTurma, string[] componentesCurricularesId, long periodoEscolarId, string professor = null);
        Task<bool> ExisteFrequenciaRegistradaPorTurmaComponenteCurricularEBimestres(string codigoTurma, string[] componentesCurricularesId, long[] periodosEscolaresIds, string professor = null);
        Task<int> ObterTotalAulasPorDisciplinaETurmaAsync(DateTime dataAula, string disciplinaId, string turmaId);
        Task<IEnumerable<TurmaComponenteQntAulasDto>> ObterTotalAulasPorDisciplinaETurmaEBimestre(string[] turmasCodigo, string[] componentesCurricularesId, long tipoCalendarioId, int[] bimestres, DateTime? dataMatriculaAluno = null, DateTime? dataSituacaoAluno = null);
        Task<IEnumerable<TurmaDataAulaComponenteQtdeAulasDto>> ObterAulasPorDisciplinaETurmaEBimestre(string[] turmasCodigo, string[] codigosAlunos, string[] componentesCurricularesId, long tipoCalendarioId, int[] bimestres, DateTime? dataMatriculaAluno = null, DateTime? dataSituacaoAluno = null);
        Task<IEnumerable<RegistroFrequenciaAlunoBimestreDto>> ObterFrequenciasRegistradasPorTurmasComponentesCurriculares(string codigoAluno, string[] codigosTurma, string[] componentesCurricularesId, long? periodoEscolarId);
        Task<IEnumerable<FrequenciaAluno>> ObterPorAlunosDataAsync(string[] alunosCodigo, DateTime dataAtual, TipoFrequenciaAluno tipoFrequencia, string codigoTurma, string componenteCurricularId);
        Task<IEnumerable<FrequenciaAluno>> ObterPorAlunoTurmaComponenteBimestres(string codigoAluno, TipoFrequenciaAluno tipoFrequencia, string componenteCurricularId, string turmaCodigo, int[] bimestres);
        Task<IEnumerable<TotalFrequenciaEAulasAlunoDto>> ObterTotalFrequenciaEAulasAlunoPorTurmaComponenteBimestres(string alunoCodigo, long tipoCalendarioId, string[] componentesCurricularesIds, string[] turmasCodigo, int bimestre);
    }
}
