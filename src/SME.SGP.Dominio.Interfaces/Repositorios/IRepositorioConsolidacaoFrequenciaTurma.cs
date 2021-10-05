using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioConsolidacaoFrequenciaTurma
    {
        Task<IEnumerable<GraficoAusenciasComJustificativaDto>> ObterAusenciasComJustificativaASync(int anoLetivo, long dreId, long ueId, Modalidade? modalidade, int semestre);
        Task<IEnumerable<FrequenciaGlobalPorAnoDto>> ObterFrequenciaGlobalPorAnoAsync(int anoLetivo, long dreId, long ueId, Modalidade? modalidade, int semestre);
        Task<IEnumerable<FrequenciaGlobalPorDreDto>> ObterFrequenciaGlobalPorDreAsync(int anoLetivo, Modalidade modalidade, string ano, int? semestre);
        Task<bool> ExisteConsolidacaoFrequenciaTurmaPorAno(int ano);
        Task<long> Inserir(ConsolidacaoFrequenciaTurma consolidacao);
        Task LimparConsolidacaoFrequenciasTurmasPorAno(int ano);
        Task<long> InserirConsolidacaoDashBoard(ConsolidacaoDashBoardFrequencia consolidacao);
        Task ExcluirConsolidacaoDashBoard(int anoletivo, long turmaId, DateTime dataAula, DateTime? dataInicioSemanda, DateTime? dataFinalSemena, int? mes, TipoPeriodoDashboardFrequencia tipoPeriodo);
    }
}
