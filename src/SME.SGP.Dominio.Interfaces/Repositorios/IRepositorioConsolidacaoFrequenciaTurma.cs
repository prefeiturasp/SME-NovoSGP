using SME.SGP.Dominio.Enumerados;
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
    }
}
