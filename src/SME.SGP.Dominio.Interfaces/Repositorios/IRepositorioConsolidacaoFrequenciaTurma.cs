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
        Task ExcluirConsolidacaoDashBoard(int anoletivo, long turmaId, DateTime dataAula, DateTime? dataInicioSemanda, DateTime? dataFinalSemena, int? mes, TipoPeriodoDashboardFrequencia tipoPeriodo);
        Task<RetornoConsolidacaoExistenteDto> ObterConsolidacaoDashboardPorTurmaAnoTipoPeriodoMes(long turmaId, int anoLetivo, TipoPeriodoDashboardFrequencia tipo, DateTime dataAula, int? mes, DateTime? dataInicioSemana, DateTime? dataFimSemana);
        Task AlterarConsolidacaoDashboardTurmaMesPeriodoAno(long id, int quantidadePresente, int quantidadeAusente, int quantidadeRemoto);
        Task Excluir(long turmaId);
    }
}
