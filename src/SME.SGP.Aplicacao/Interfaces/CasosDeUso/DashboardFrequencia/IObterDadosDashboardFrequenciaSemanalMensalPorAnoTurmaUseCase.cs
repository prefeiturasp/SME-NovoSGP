using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterDadosDashboardFrequenciaSemanalMensalPorAnoTurmaUseCase
    {
        Task<GraficoFrequenciaAlunoDto> Executar(FrequenciasConsolidadacaoPorTurmaEAnoDto frequenciaDto, DateTime? dataInicio, DateTime? datafim, int? mes, TipoConsolidadoFrequencia tipoConsolidadoFrequencia);
    }
}