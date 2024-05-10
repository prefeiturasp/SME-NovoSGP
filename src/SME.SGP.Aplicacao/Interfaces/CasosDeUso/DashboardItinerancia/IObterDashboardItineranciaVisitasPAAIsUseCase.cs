using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IObterDashboardItineranciaVisitasPAAIsUseCase : IUseCase<FiltroDashboardItineranciaDto, IEnumerable<DashboardItineranciaDto>>
    {
    }
}
