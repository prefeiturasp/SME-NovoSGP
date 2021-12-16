using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioTurma
    {
        IEnumerable<Turma> MaterializarCodigosTurma(string[] idTurmas, out string[] codigosNaoEncontrados);
        Task<IEnumerable<Turma>> SincronizarAsync(IEnumerable<Turma> entidades, IEnumerable<Ue> ues);
        Task<bool> AtualizarTurmaParaHistorica(string turmaId);
        Task<bool> SalvarAsync(TurmaParaSyncInstitucionalDto turma, long ueId);
        Task ExcluirTurmaExtintaAsync(string turmaCodigo, long turmaId);
        Task<bool> AtualizarTurmaSincronizacaoInstitucionalAsync(TurmaParaSyncInstitucionalDto turma, bool deveMarcarHistorica = false);
        Task<PaginacaoResultadoDto<TurmaAcompanhamentoFechamentoRetornoDto>> ObterTurmasFechamentoAcompanhamento(Paginacao paginacao, long dreId, long ueId, string[] turmasCodigo, Modalidade modalidade, int semestre, int bimestre, int anoLetivo, int? situacaoFechamento, int? situacaoConselhoClasse, bool listarTodasTurmas);
        Task<IEnumerable<GraficoBaseDto>> ObterInformacoesEscolaresTurmasAsync(int anoLetivo, long dreId, long ueId, AnoItinerarioPrograma[] anos, Modalidade modalidade, int? semestre);
        Task<Turma> ObterTurmaCompletaPorCodigo(string turmaCodigo);
        Task<IEnumerable<TurmaDTO>> ObterTurmasInfantilPorAno(int anoLetivo, string ueCodigo);
        Task<IEnumerable<long>> ObterTurmasIdsPorUeEAnoLetivo(int anoLetivo, string ueCodigo);
    }
}