using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public interface IObterDadosDashboardFrequenciaSemanalMensalPorAnoTurmaUseCase
    {
        Task<IEnumerable<GraficoFrequenciaSemanalMensalDTO>> Executar(int anoLetivo, long dreId, long ueId, int modalidade, string anoTurma, DateTime? dataInicio, DateTime? datafim, int? mes, TipoConsolidadoFrequencia tipoConsolidadoFrequencia, bool visaoDre = false);
    }
}