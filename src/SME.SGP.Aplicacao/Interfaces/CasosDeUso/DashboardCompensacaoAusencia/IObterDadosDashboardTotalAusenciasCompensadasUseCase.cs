using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterDadosDashboardTotalAusenciasCompensadasUseCase
    {
        Task<GraficoCompensacaoAusenciaDto> Executar(int anoLetivo, long dreId, long ueId, int modalidade, int bimestre, int semestre);
    }
}