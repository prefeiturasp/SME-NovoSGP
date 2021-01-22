using SME.SGP.Infra.Dtos.EscolaAqui.ComunicadosFiltro;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterComunicadosParaFiltroDaDashboardUseCase
    {
        Task<IEnumerable<ComunicadoParaFiltroDaDashboardDto>> Executar(ObterComunicadosParaFiltroDaDashboardDto obterComunicadosFiltroDto);
    }
}