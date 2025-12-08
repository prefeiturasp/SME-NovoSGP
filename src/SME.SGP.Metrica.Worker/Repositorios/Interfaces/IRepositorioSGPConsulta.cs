using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Metrica.Worker.Entidade;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Metrica.Worker.Repositorios.Interfaces
{
    public interface IRepositorioSGPConsulta
    {
        Task<IEnumerable<long>> ObterUesIds();
        Task<IEnumerable<string>> ObterUesCodigo();
        Task<IEnumerable<long>> ObterTurmasIds(int[] modalidades);
        Task<IEnumerable<long>> ObterTurmasIdsPorUE(long ueId, int? anoLetivo = null);
        Task<IEnumerable<string>> ObterTurmasCodigoPorUE(string ueCodigo, int? anoLetivo = null);
        Task<Turma> ObterTurmaComUeEDrePorCodigo(string turmaCodigo);
        Task<bool> ComponenteCurriculareEhRegencia(long id);
        Task<int> ObterQuantidadeAcessosDia(DateTime data);
        Task<Grade> ObterGradeTurmaAno(TipoEscola tipoEscola, Modalidade modalidade, int duracao, int ano, string anoLetivo);
        Task<int> ObterHorasComponente(long gradeId, long[] componentesCurriculares, int ano);
        Task<int> ObterQuantidadeAulasTurmaExperienciasPedagogicasDia(string turma, DateTime dataAula, bool consideraSomenteAulasComRegistroFrequencia = true);
        Task<int> ObterQuantidadeAulasTurmaComponenteCurricularDia(string turma, string componenteCurricular, DateTime dataAula, bool consideraSomenteAulasComRegistroFrequencia = true);
        Task<int> ObterQuantidadeAulasTurmaExperienciasPedagogicasSemana(string turma, int semana, string componenteCurricular, bool consideraSomenteAulasComRegistroFrequencia = true);
        Task<int> ObterQuantidadeAulasTurmaDisciplinaSemana(string turma, string componenteCurricular, int semana, bool consideraSomenteAulasComRegistroFrequencia = true);
        Task<long> ObterTipoCalendarioId(int anoLetivo, int modalidadeTipoCalendario);
        Task<PeriodoIdDto> ObterPeriodoEscolarPorTipoCalendarioData(long tipoCalendarioId, DateTime dataParaVerificar);
        Task<PeriodoIdDto> ObterPeriodoFechamentoPorPeriodoEscolar(long periodoEscolarId);
        Task<IEnumerable<ConselhoClasseDuplicado>> ObterConselhosClasseDuplicados();
        Task<IEnumerable<ConselhoClasseAlunoDuplicado>> ObterConselhosClasseAlunoDuplicados(long ueId);
        Task<IEnumerable<ConselhoClasseNotaDuplicado>> ObterConselhosClasseNotaDuplicados();
        Task<IEnumerable<FechamentoTurmaDuplicado>> ObterFechamentosTurmaDuplicados();
        Task<IEnumerable<FechamentoTurmaDisciplinaDuplicado>> ObterFechamentosTurmaDisciplinaDuplicados();
        Task<IEnumerable<FechamentoAlunoDuplicado>> ObterFechamentosAlunoDuplicados(long ueId);
        Task<IEnumerable<FechamentoNotaDuplicado>> ObterFechamentosNotaDuplicados(long turmaId);
        Task<IEnumerable<ConsolidacaoConselhoClasseNotaNulos>> ObterConsolidacaoCCNotasNulos();
        Task<IEnumerable<ConsolidacaoConselhoClasseAlunoTurmaDuplicado>> ObterConsolidacaoCCAlunoTurmaDuplicados(long ueId);
        Task<IEnumerable<ConsolidacaoCCNotaDuplicado>> ObterConsolidacaoCCNotasDuplicados();
        Task<IEnumerable<ConselhoClasseNaoConsolidado>> ObterConselhosClasseNaoConsolidados(long ueId);
        Task<IEnumerable<FrequenciaAlunoInconsistente>> ObterFrequenciaAlunoInconsistente(long turmaId);
        Task<IEnumerable<FrequenciaAlunoDuplicado>> ObterFrequenciaAlunoDuplicados(long ueId);
        Task<IEnumerable<RegistroFrequenciaDuplicado>> ObterRegistroFrequenciaDuplicados(long ueId);
        Task<IEnumerable<RegistroFrequenciaAlunoDuplicado>> ObterRegistroFrequenciaAlunoDuplicados(long turmaId);
        Task<IEnumerable<DiarioBordoDuplicado>> ObterDiariosBordoDuplicados();
        Task<int> ObterQuantidadeRegistrosFrequenciaDia(DateTime data);
        Task<int> ObterQuantidadeDiariosBordoDia(DateTime data);
        Task<int> ObterQuantidadeDevolutivasDiarioBordoMes(DateTime data);
        Task<int> ObterQuantidadeAulasCJMes(DateTime data);
        Task<int> ObterQuantidadePlanosAulaDia(DateTime data);
        Task<int> ObterQuantidadeEncaminhamentosAEEMes(DateTime data);
        Task<int> ObterQuantidadePlanosAEEMes(DateTime data);
        Task<IEnumerable<DevolutivaDuplicado>> ObterDevolutivaDuplicados();
        Task<IEnumerable<DevolutivaMaisDeUmaNoDiario>> ObterDevolutivaMaisDeUmaNoDiario();
        Task<IEnumerable<DevolutivaSemDiario>> ObterDevolutivaSemDiario();
        Task<IEnumerable<(int Bimestre, int Quantidade)>> ObterQuantidadeFechamentosNotaDia(DateTime data);
        Task<IEnumerable<(int Bimestre, int Quantidade)>> ObterQuantidadeConselhosClasseAlunoDia(DateTime data);
        Task<IEnumerable<(int Bimestre, int Quantidade)>> ObterQuantidadeFechamentosTurmaDisciplinaDia(DateTime data);
    }
}
