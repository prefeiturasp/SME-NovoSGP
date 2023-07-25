using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioConsolidacaoFrequenciaTurma
    {
        Task<long> Inserir(ConsolidacaoFrequenciaTurma consolidacao);
        Task LimparConsolidacaoFrequenciasTurmasPorAno(int ano);
        Task<long> InserirConsolidacaoDashBoard(ConsolidacaoDashBoardFrequencia consolidacao);
        Task<RetornoConsolidacaoExistenteDto> ObterConsolidacaoDashboardPorTurmaAnoTipoPeriodoMes(long turmaId, int anoLetivo, TipoPeriodoDashboardFrequencia tipo, DateTime dataAula, int? mes, DateTime? dataInicioSemana, DateTime? dataFimSemana);
        Task AlterarConsolidacaoDashboardTurmaMesPeriodoAno(long id, int quantidadePresente, int quantidadeAusente, int quantidadeRemoto);
        Task Excluir(long turmaId);
        Task<ConsolidacaoDashBoardFrequencia> ObterConsolidacaoDashboardPorTurmaAulaModalidadeAnoLetivoDreUeTipo(long turmaId, DateTime dataAula, Modalidade modalidadeCodigo, int anoLetivo, long dreId, long ueId, TipoPeriodoDashboardFrequencia tipo);
        Task<long> SalvarAsync(ConsolidacaoDashBoardFrequencia consolidacaoDashBoardFrequencia);
    }
}
