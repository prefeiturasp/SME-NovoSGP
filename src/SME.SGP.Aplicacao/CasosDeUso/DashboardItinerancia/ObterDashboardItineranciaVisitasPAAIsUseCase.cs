using MediatR;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDashboardItineranciaVisitasPAAIsUseCase : AbstractUseCase, IObterDashboardItineranciaVisitasPAAIsUseCase
    {
        public ObterDashboardItineranciaVisitasPAAIsUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<DashboardItineranciaDto>> Executar(FiltroDashboardItineranciaDto param)
        {
            if (param.AnoLetivo == 0)
                param.AnoLetivo = DateTime.Now.Year;

            return await mediator.Send(new ObterDashboardItineranciaVisitasPAAIQuery(param.AnoLetivo, param.DreId, param.UeId, param.Mes));
        }
    }
}
