using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterDadosDashboardFrequenciaDiariaPorAnoTurmaUseCase
    {
        Task<GraficoFrequenciaAlunoDto> Executar(int anoLetivo, long dreId, long ueId, int modalidade, int semestre, long[] turmaIds, DateTime dataAula, bool visaoDre = false);
    }
}