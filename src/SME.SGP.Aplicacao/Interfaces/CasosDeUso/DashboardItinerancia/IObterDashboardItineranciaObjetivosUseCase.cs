using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IObterDashboardItineranciaObjetivosUseCase : IUseCase<FiltroDashboardItineranciaDto, IEnumerable<DashboardItineranciaDto>>
    {
    }
}
