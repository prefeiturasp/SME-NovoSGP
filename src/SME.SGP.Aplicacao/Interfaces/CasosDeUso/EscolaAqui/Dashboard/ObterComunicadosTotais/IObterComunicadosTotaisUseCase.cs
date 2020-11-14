using SME.SGP.Infra.Dtos.EscolaAqui.Dashboard;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterComunicadosTotaisUseCase
    {
        Task<ComunicadosTotaisResultado> Executar(int anoLetivo, string codigoDre, string codigoUe);
    }
}