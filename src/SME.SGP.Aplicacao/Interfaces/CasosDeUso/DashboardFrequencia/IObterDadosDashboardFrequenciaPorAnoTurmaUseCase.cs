using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterDadosDashboardFrequenciaPorAnoTurmaUseCase
    {
        Task<GraficoFrequenciaAlunoDto> Executar(int anoLetivo, long dreId, long ueId, int modalidade, int semestre, int anoTurma, DateTime dataInicio, DateTime datafim, int mes, int tipoPeriodoDashboard);
    }
}