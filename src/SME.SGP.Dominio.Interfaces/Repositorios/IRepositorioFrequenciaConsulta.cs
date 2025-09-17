using SME.SGP.Dto;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Dtos.Frequencia;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioFrequenciaConsulta : IRepositorioBase<RegistroFrequencia>
    {
        Task<bool> FrequenciaAulaRegistrada(long aulaId);
        RegistroFrequenciaAulaDto ObterAulaDaFrequencia(long registroFrequenciaId);
        IEnumerable<AulasPorTurmaDisciplinaDto> ObterAulasSemRegistroFrequencia(string turmaId, string disciplinaId, TipoNotificacaoFrequencia tipoNotificacao);
        Task<IEnumerable<AusenciaAlunoDto>> ObterAusencias(string turmaCodigo, string disciplinaCodigo, DateTime[] datas, string[] alunoCodigos);
        Task<IEnumerable<RecuperacaoParalelaFrequenciaDto>> ObterFrequenciaAusencias(string[] CodigoAlunos, string CodigoDisciplina, int Ano, PeriodoRecuperacaoParalela Periodo);
        Task<IEnumerable<AulaComFrequenciaNaDataDto>> ObterAulasComRegistroFrequenciaPorTurma(string turmaCodigo);
        Task<IEnumerable<AlunoComponenteCurricularDto>> ObterAlunosAusentesPorTurmaEPeriodo(string turmaCodigo, DateTime dataInicio, DateTime dataFim, string componenteCurricularId);
        Task<IEnumerable<RegistroAusenciaAluno>> ObterListaFrequenciaPorAula(long aulaId);
        Task<RegistroFrequencia> ObterRegistroFrequenciaPorAulaId(long aulaId);
        Task<IEnumerable<AlunosFaltososDto>> ObterAlunosFaltosos(DateTime dataReferencia, long tipoCalendarioId, long ueId);
        Task<IEnumerable<AusenciaMotivoDto>> ObterAusenciaMotivoPorAlunoTurmaBimestreAno(string codigoAluno, string turma, short bimestre, short anoLetivo);
        Task<IEnumerable<GraficoBaseDto>> ObterDashboardFrequenciaAusenciasPorMotivo(int anoLetivo, long dreId, long ueId, Modalidade? modalidade, string ano, long turmaId, int semestre);
        Task<IEnumerable<string>> ObterTurmasCodigosFrequenciasExistentesPorAnoAsync(int[] anosLetivos);        
        Task<IEnumerable<RegistroFrequencia>> ObterRegistroFrequenciaPorDataEAulaId(string disciplina, string turmaId, DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<RegistroFrequenciaAlunoPorAulaDto>> ObterFrequenciasDetalhadasPorData(string turmaCodigo, string[] componentesCurricularesId, string[] codigosAlunos, DateTime dataInicio, DateTime dataFim);
        Task<bool> RegistraFrequencia(long componenteCurricularId, long? codigoTerritorioSaber = null);
        Task<FrequenciaTurmaEvasaoDto> ObterDashboardFrequenciaTurmaEvasaoAbaixo50Porcento(int anoLetivo, string dreCodigo, string ueCodigo, Modalidade modalidade, int semestre, int mes);
        Task<FrequenciaTurmaEvasaoDto> ObterDashboardFrequenciaTurmaEvasaoSemPresenca(int anoLetivo, string dreCodigo, string ueCodigo, Modalidade modalidade, int semestre, int mes);
        Task<IEnumerable<AusenciaAlunoDto>> ObterAusenciasPorAluno(string turmaCodigo, string disciplinaCodigo, DateTime[] datas, string alunoCodigo);
        Task<IEnumerable<FrequenciaAlunoDto>> ObterFrequenciaPorTurmaPeriodo(string codigoTurma, DateTime dataInicio, DateTime dataFim);
        Task<long[]> ObterFrequenciasAlunosIdsComPresencasMaiorTotalAulas(long ueId, int anoLetivo);
        Task<PaginacaoResultadoDto<AlunoFrequenciaTurmaEvasaoDto>> ObterAlunosDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoPaginado(int mes, FiltroAbrangenciaGraficoFrequenciaTurmaEvasaoAlunoDto filtroAbrangencia, Paginacao paginacao);
        Task<PaginacaoResultadoDto<AlunoFrequenciaTurmaEvasaoDto>> ObterAlunosDashboardFrequenciaTurmaEvasaoSemPresencaPaginado(int mes, FiltroAbrangenciaGraficoFrequenciaTurmaEvasaoAlunoDto filtroAbrangencia, Paginacao paginacao);
        Task<IEnumerable<RegistroFrequenciaProdutividadeDto>> ObterInformacoesProdutividadeFrequencia(int anoLetivo, string ueCodigo, int bimestre);
        Task<IEnumerable<RegistroFrequenciaPainelEducacionalDto>> ObterInformacoesFrequenciaPainelEducacional(IEnumerable<long> turmaIds);
        Task<ComponenteCurricularSugeridoDto> ObterPrimeiroRegistroFrequenciaPorDataETurma(string turmaId, DateTime dataAula);
    }
}