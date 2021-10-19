using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Dominio.Interfaces
{
    public interface IRepositorioDashBoardFrequencia
    {
        Task<IEnumerable<FrequenciaAlunoDashboardDto>> ObterFrequenciasConsolidadasPorTurmaEAno(int anoLetivo, long dreId, long ueId, int modalidade, int semestre, DateTime dataAula, DateTime dataInicioSemana, DateTime datafimSemana, int mes, int tipoPeriodoDashboard, bool visaoDre = false);
        Task<DadosParaConsolidacaoDashBoardFrequenciaDto> ObterDadosParaConsolidacao(int anoLetivo, long turmaId, int modalidade, int tipoPeriodo, DateTime dataAula, DateTime? dataInicioSemana, DateTime? datafimSemana, int? mes);       
    }
}