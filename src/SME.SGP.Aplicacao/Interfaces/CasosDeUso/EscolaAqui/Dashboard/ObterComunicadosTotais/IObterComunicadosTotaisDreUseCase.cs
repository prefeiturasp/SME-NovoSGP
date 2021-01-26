using SME.SGP.Infra.Dtos.EscolaAqui.Dashboard;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterComunicadosTotaisAgrupadosPorDreUseCase
    {
        Task<IEnumerable<ComunicadosTotaisPorDreResultado>> Executar(int anoLetivo);
    }
}