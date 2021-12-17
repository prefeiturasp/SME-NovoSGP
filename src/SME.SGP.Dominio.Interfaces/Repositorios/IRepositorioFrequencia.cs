using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFrequencia : IRepositorioBase<RegistroFrequencia>
    {
        Task ExcluirFrequenciaAula(long aulaId);

        Task<bool> FrequenciaAulaRegistrada(long aulaId);

        RegistroFrequenciaAulaDto ObterAulaDaFrequencia(long registroFrequenciaId);

        IEnumerable<AulasPorTurmaDisciplinaDto> ObterAulasSemRegistroFrequencia(string turmaId, string disciplinaId, TipoNotificacaoFrequencia tipoNotificacao);

        Task<IEnumerable<AusenciaAlunoDto>> ObterAusencias(string turmaCodigo, string disciplinaCodigo, DateTime[] datas, string[] alunoCodigos);
        Task<bool> RegistraFrequencia(long componenteCurricularId);
        Task<IEnumerable<RecuperacaoParalelaFrequenciaDto>> ObterFrequenciaAusencias(string[] CodigoAlunos, string CodigoDisciplina, int Ano, PeriodoRecuperacaoParalela Periodo);
        Task<IEnumerable<AulaComFrequenciaNaDataDto>> ObterAulasComRegistroFrequenciaPorTurma(string turmaCodigo);
        Task SalvarConciliacaoTurma(string turmaId, string disciplinaId, DateTime dataReferencia, string alunos);
        Task<IEnumerable<AlunoComponenteCurricularDto>> ObterAlunosAusentesPorTurmaEPeriodo(string turmaCodigo, DateTime dataInicio, DateTime dataFim, string componenteCurricularId);
        IEnumerable<RegistroAusenciaAluno> ObterListaFrequenciaPorAula(long aulaId);

        Task<RegistroFrequencia> ObterRegistroFrequenciaPorAulaId(long aulaId);

        IEnumerable<AlunosFaltososDto> ObterAlunosFaltosos(DateTime dataReferencia, long tipoCalendarioId);
        Task<IEnumerable<AusenciaMotivoDto>> ObterAusenciaMotivoPorAlunoTurmaBimestreAno(string codigoAluno, string turma, short bimestre, short anoLetivo);
        Task<IEnumerable<GraficoBaseDto>> ObterDashboardFrequenciaAusenciasPorMotivo(int anoLetivo, long dreId, long ueId, Modalidade? modalidade, string ano, long turmaId, int semestre);
        Task<IEnumerable<string>> ObterTurmasCodigosFrequenciasExistentesPorAnoAsync(int[] anosLetivos);        
        Task<IEnumerable<TotalFrequenciaEAulasPorPeriodoDto>> ObterTotalFrequenciaEAulasPorPeriodo(int anoLetivo, long dreId, long ueId, int modalidade, int semestre, DateTime dataInicio, DateTime datafim, int mes, int tipoPeriodoDashboard);
        Task<IEnumerable<RegistroFrequenciaAlunoPorAulaDto>> ObterFrequenciasDetalhadasPorData(string turmaCodigo, string componenteCurricularId, string[] codigoAluno, DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<RegistroFrequencia>> ObterRegistroFrequenciaPorDataEAulaId(string disciplina, string tumaId, DateTime dataInicio, DateTime dataFim);
    }
}