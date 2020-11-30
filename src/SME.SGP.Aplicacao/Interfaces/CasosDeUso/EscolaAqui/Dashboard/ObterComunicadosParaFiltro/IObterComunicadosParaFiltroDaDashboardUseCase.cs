using SME.SGP.Infra.Dtos.EscolaAqui.Dashboard.ComunicadosFiltro;
using SME.SGP.Infra.Dtos.EscolaAqui.Dashboard.ComunicadosPesquisa;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterComunicadosParaFiltroDaDashboardUseCase
    {
        Task<IEnumerable<ComunicadoParaFiltroDaDashboardDto>> Executar(ObterComunicadosParaFiltroDaDashboardDto obterComunicadosFiltroDto);
    }
}
