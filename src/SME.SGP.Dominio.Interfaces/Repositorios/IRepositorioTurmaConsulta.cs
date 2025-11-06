using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioTurmaConsulta
    {
        Task<Turma> ObterPorCodigo(string turmaCodigo);
        Task<string> ObterTurmaCodigoPorConselhoClasseId(long conselhoClasseId);
        Task<IEnumerable<Turma>> ObterPorCodigosAsync(string[] codigos);
        Task<long> ObterTurmaIdPorCodigo(string turmaCodigo);
        Task<string> ObterTurmaCodigoPorId(long turmaId);
        Task<Turma> ObterPorId(long id);
        Task<ObterTurmaSimplesPorIdRetornoDto> ObterTurmaSimplesPorId(long id);
        Task<Turma> ObterTurmaComUeEDrePorCodigo(string turmaCodigo);
        Task<IEnumerable<Turma>> ObterTurmasComUeEDrePorCodigo(string turmaCodigo);
        Task<Turma> ObterTurmaComUeEDrePorId(long turmaId);
        Task<bool> ObterTurmaEspecialPorCodigo(string turmaCodigo);
        Task<IEnumerable<string>> ObterCodigosTurmasPorUeAno(int anoLetivo, string ueCodigo);
        Task<DreUeDaTurmaDto> ObterCodigosDreUe(string turmaCodigo);
        Task<IEnumerable<TurmaModalidadeCodigoDto>> ObterModalidadePorCodigos(string[] turmasCodigo);
        Task<IEnumerable<TurmaAlunoBimestreFechamentoDto>> AlunosTurmaPorDreIdUeIdBimestreSemestre(long ueId, int ano, long dreId, int modalidade, int semestre, int bimestre);
        Task<IEnumerable<ModalidadesPorAnoDto>> ObterModalidadesPorAnos(int anoLetivo, long dreId, long ueId, int modalidade, int semestre);
        Task<IEnumerable<TurmaConsolidacaoFechamentoGeralDto>> ObterTurmasConsolidacaoFechamentoGeralAsync(string turmaCodigo);
        Task<IEnumerable<TurmaModalidadeDto>> ObterTurmasComModalidadePorAnoUe(int ano, long ueId);
        Task<Turma> ObterTurmaPorAnoLetivoModalidadeTipoAsync(long ueId, int anoLetivo, TipoTurma turmaTipo);
        Task<IEnumerable<Turma>> ObterTurmasPorAnoLetivo(int anoLetivo);
        Task<DreUeDaTurmaDto> ObterCodigosDreUePorId(long turmaId);
        Task<IEnumerable<Turma>> ObterTurmasInfantilNaoDeProgramaPorAnoLetivoAsync(int anoLetivo, string codigoTurma = null, int pagina = 1);
        Task<IEnumerable<Turma>> ObterTurmasPorIds(long[] turmasIds);
        Task<IEnumerable<string>> ObterCodigosTurmasPorAnoModalidade(int anoLetivo, int[] modalidades, string turmaCodigo = "");
        Task<IEnumerable<TurmaComponenteDto>> ObterTurmasComponentesPorAnoLetivo(DateTime dataReferencia);
        Task<IEnumerable<Turma>> ObterTurmasPorUeModalidadesAno(long? ueId, int[] modalidades, int ano);
        Task<IEnumerable<Turma>> ObterTurmasComInicioFechamento(long ueId, long periodoEscolarId, int[] modalidades);
        Task<IEnumerable<Turma>> ObterTurmasPorAnoLetivoModalidade(int anoLetivo, Modalidade[] modalidades);
        Task<IEnumerable<Turma>> ObterTurmasCompletasPorAnoLetivoModalidade(int anoLetivo, Modalidade[] modalidades, string turmaCodigo = "");
        Task<IEnumerable<Turma>> ObterTurmasComFechamentoOuConselhoNaoFinalizados(long? ueId, int anoLetivo, long? periodoEscolarId, int[] modalidades, int semestre);
        Task<IEnumerable<long>> ObterTurmasPorUeAnos(string ueCodigo, int anoLetivo, string[] anos, int modalidadeId);
        Task<Modalidade> ObterModalidadePorCodigo(string turmaCodigo);
        Task<PaginacaoResultadoDto<TurmaAcompanhamentoFechamentoRetornoDto>> ObterTurmasFechamentoAcompanhamento(Paginacao paginacao, long dreId, long ueId, string[] turmasCodigo, Modalidade modalidade, int semestre, int bimestre, int anoLetivo, int? situacaoFechamento, int? situacaoConselhoClasse, bool listarTodasTurmas);
        Task<IEnumerable<GraficoBaseDto>> ObterInformacoesEscolaresTurmasAsync(int anoLetivo, long dreId, long ueId, AnoItinerarioPrograma[] anos, Modalidade modalidade, int? semestre);
        Task<Turma> ObterTurmaCompletaPorCodigo(string turmaCodigo);
        Task<IEnumerable<TurmaDTO>> ObterTurmasInfantilPorAno(int anoLetivo, string ueCodigo);
        Task<IEnumerable<long>> ObterTurmasIdsPorUeEAnoLetivo(int anoLetivo, string ueCodigo);
        Task<IEnumerable<TurmaNaoHistoricaDto>> ObterTurmasPorUsuarioEAnoLetivo(long usuarioId, int anoLetivo);
        Task<IEnumerable<int>> BuscarAnosLetivosComTurmasVigentes(string codigoUe);
        Task<bool> VerificaSeVirouHistorica(long turmaId);
        Task<IEnumerable<RetornoConsultaTurmaNomeFiltroDto>> ObterTurmasNomeFiltro(string[] turmasCodigos);
        Task<IEnumerable<Turma>> ObterTurmasDreUeCompletaPorCodigos(string[] turmasCodigo);
        Task<IEnumerable<long>> ObterIdsTurmasPorAnoModalidadeUeTipoRegular(int anoLetivo, int modalidade, long ueId);
        Task<IEnumerable<TurmaConsolidacaoFechamentoGeralDto>> ObterTurmasConsolidacaoFechamentoGeralAsync(int anoLetivo, int pagina, int quantidadeRegistrosPorPagina, params TipoEscola[] tiposEscola);
        Task<IEnumerable<TurmaComplementarDto>> ObterTurmasComplementaresPorAlunos(string[] alunosCodigos);
        Task<Turma> ObterSomenteTurmaPorId(long turmaId);
        Task<IEnumerable<TurmaBimestreDto>> ObterTurmasComFechamentoTurmaPorUeId(long ueId, int anoLetivo);
        Task<IEnumerable<long>> ObterIdsPorCodigos(string[] codigosTurma);
        Task<IEnumerable<TurmaAlunoDto>> ObterPorAlunos(long[] codigoAlunos, int anoLetivo);
        Task<IEnumerable<TurmaDTO>> ObterTurmasAulasNormais(long ueId, int anoLetivo, int[] tiposTurma, int[] modalidades, int[] ignorarTiposCiclos);
        Task<IEnumerable<TurmaRetornoDto>> ObterTurmasSondagem(string codigoUe, int anoLetivo);
        Task<IEnumerable<TurmaPainelEducacionalFrequenciaDto>> ObterTodasTurmasPainelEducacionalFrequenciaAsync();
        Task<IEnumerable<TurmaPainelEducacionalDto>> ObterTurmasPainelEducacionalAsync(int anoLetivo);
        Task<IEnumerable<TurmaModalidadeDto>> ObterTurmasComModalidadePorModalidadeAnoUe(int ano, long ueId, IEnumerable<int> modalidades);
    }
}
