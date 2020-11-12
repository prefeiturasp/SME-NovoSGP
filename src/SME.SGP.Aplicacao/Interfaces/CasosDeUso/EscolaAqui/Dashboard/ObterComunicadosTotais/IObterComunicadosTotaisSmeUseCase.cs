using SME.SGP.Infra.Dtos.EscolaAqui.Dashboard;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterComunicadosTotaisSmeUseCase
    {
        Task<ComunicadosTotaisSmeResultado> Executar(int anoLetivo);
    }

}