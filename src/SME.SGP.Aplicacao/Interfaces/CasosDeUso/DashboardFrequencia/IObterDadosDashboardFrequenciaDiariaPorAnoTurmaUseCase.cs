using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterDadosDashboardFrequenciaDiariaPorAnoTurmaUseCase
    {
        Task<GraficoFrequenciaAlunoDto> Executar(FrequenciasConsolidadacaoPorTurmaEAnoDto frequenciaDto);
    }
}