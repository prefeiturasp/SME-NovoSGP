using SME.SGP.Infra.Dtos.EscolaAqui.ComunicadosFiltro;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IObterComunicadosParaFiltroDaDashboardUseCase : IUseCase<ObterComunicadosParaFiltroDaDashboardDto, IEnumerable<ComunicadoParaFiltroDaDashboardDto>>
    {
    }
}