using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioConsolidacaoFrequenciaTurma
    {
        Task LimparConsolidacaoFrequenciasTurmasPorAno(int ano);
        Task AlterarConsolidacaoDashboardTurmaMesPeriodoAno(long id, int quantidadePresente, int quantidadeAusente, int quantidadeRemoto);
        Task<ConsolidacaoDashBoardFrequencia> ObterConsolidacaoDashboardPorTurmaAulaModalidadeAnoLetivoDreUeTipo(long turmaId, DateTime dataAula, Modalidade modalidadeCodigo, int anoLetivo, long dreId, long ueId, TipoPeriodoDashboardFrequencia tipo);
        Task<long> SalvarConsolidacaoDashBoardFrequencia(ConsolidacaoDashBoardFrequencia consolidacaoDashBoardFrequencia);
        Task<long> SalvarConsolidacaoFrequenciaTurma(ConsolidacaoFrequenciaTurma consolidacaoFrequenciaTurma);
        Task<ConsolidacaoFrequenciaTurma> ObterConsolidacaoDashboardPorTurmaETipoPeriodo(long turmaId, TipoConsolidadoFrequencia tipoConsolidacao, DateTime? periodoInicio, DateTime? periodoFim);
    }
}
