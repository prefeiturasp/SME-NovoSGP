using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterDadosDashboardFrequenciaSemanalMensalPorAnoTurmaUseCase
    {
        Task<IEnumerable<GraficoFrequenciaSemanalMensalDTO>> Executar(int anoLetivo, long dreId, long ueId, int modalidade, string anoTurma, DateTime? dataInicio, DateTime? datafim, int? mes, int tipoConsolidadoFrequencia, bool visaoDre = false);
    }
}