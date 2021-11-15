using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioTurma
    {
        IEnumerable<Turma> MaterializarCodigosTurma(string[] idTurmas, out string[] codigosNaoEncontrados);

        Task<Turma> ObterPorCodigo(string turmaCodigo);

        Task<Turma> ObterPorId(long id);

        Task<Turma> ObterTurmaComUeEDrePorCodigo(string turmaCodigo);

        Task<Turma> ObterTurmaComUeEDrePorId(long turmaId);

        Task<bool> ObterTurmaEspecialPorCodigo(string turmaCodigo);

        Task<IEnumerable<Turma>> SincronizarAsync(IEnumerable<Turma> entidades, IEnumerable<Ue> ues);

        Task<long> ObterTurmaIdPorCodigo(string turmaCodigo);
        Task<DreUeDto> ObterCodigosDreUe(string turmaCodigo);
        Task<IEnumerable<TurmaModalidadeCodigoDto>> ObterModalidadePorCodigos(string[] turmasCodigo);
        Task<IEnumerable<TurmaAlunoBimestreFechamentoDto>> AlunosTurmaPorDreIdUeIdBimestreSemestre(long ueId, int ano, long dreId, int modalidade, int semestre, int bimestre);
        Task<IEnumerable<ModalidadesPorAnoDto>> ObterModalidadesPorAnos(int anoLetivo, long dreId, long ueId, int modalidade, int semestre);
        Task<IEnumerable<TurmaConsolidacaoFechamentoGeralDto>> ObterTurmasConsolidacaoFechamentoGeralAsync(string turmaCodigo);
        Task<IEnumerable<TurmaModalidadeDto>> ObterTurmasComModalidadePorAno(int ano);
        Task<Turma> ObterTurmaPorAnoLetivoModalidadeTipoAsync(long ueId, int anoLetivo, TipoTurma turmaTipo);
        Task<DreUeDto> ObterCodigosDreUePorId(long turmaId);
        Task<IEnumerable<Turma>> ObterTurmasInfantilNaoDeProgramaPorAnoLetivoAsync(int anoLetivo, string codigoTurma = null);
        Task<IEnumerable<Turma>> ObterTurmasPorIds(long[] turmasIds);
        Task<IEnumerable<string>> ObterCodigosTurmasPorAnoModalidade(int anoLetivo, int[] modalidades, string turmaCodigo = "");
        Task<IEnumerable<TurmaComponenteDto>> ObterTurmasComponentesPorAnoLetivo(DateTime dataReferencia);
        Task<IEnumerable<Turma>> ObterTurmasPorUeModalidadesAno(long? ueId, int[] modalidades, int ano);
        Task<IEnumerable<Turma>> ObterTurmasComInicioFechamento(long ueId, long periodoEscolarId, int[] modalidades);
        Task<IEnumerable<Turma>> ObterTurmasPorAnoLetivoModalidade(int anoLetivo, Modalidade[] modalidades);
        Task<IEnumerable<Turma>> ObterTurmasCompletasPorAnoLetivoModalidade(int anoLetivo, Modalidade[] modalidades, string turmaCodigo = "");
        Task<IEnumerable<Turma>> ObterTurmasComFechamentoOuConselhoNaoFinalizados(long ueId, int anoLetivo, long? periodoEscolarId, int[] modalidades, int semestre);
        Task<ObterTurmaSimplesPorIdRetornoDto> ObterTurmaSimplesPorId(long id);
        Task<IEnumerable<Turma>> ObterPorCodigosAsync(string[] codigos);
        Task<IEnumerable<long>> ObterTurmasPorUeAnos(string ueCodigo, int anoLetivo, string[] anos, int modalidadeId);
        Task<Modalidade> ObterModalidadePorCodigo(string turmaCodigo);
        Task<bool> AtualizarTurmaParaHistorica(string turmaId);
        Task<bool> SalvarAsync(TurmaParaSyncInstitucionalDto turma, long ueId);
        Task ExcluirTurmaExtintaAsync(string turmaCodigo, long turmaId);
        Task<bool> AtualizarTurmaSincronizacaoInstitucionalAsync(TurmaParaSyncInstitucionalDto turma, bool deveMarcarHistorica = false);
        Task<PaginacaoResultadoDto<TurmaAcompanhamentoFechamentoRetornoDto>> ObterTurmasFechamentoAcompanhamento(Paginacao paginacao, long dreId, long ueId, string[] turmasCodigo, Modalidade modalidade, int semestre, int bimestre, int anoLetivo, int? situacaoFechamento, int? situacaoConselhoClasse, bool listarTodasTurmas);
        Task<IEnumerable<GraficoBaseDto>> ObterInformacoesEscolaresTurmasAsync(int anoLetivo, long dreId, long ueId, AnoItinerarioPrograma[] anos, Modalidade modalidade, int? semestre);
        Task<Turma> ObterTurmaCompletaPorCodigo(string turmaCodigo);
        Task<IEnumerable<TurmaDTO>> ObterTurmasInfantilPorAno(int anoLetivo, string ueCodigo);
    }
}