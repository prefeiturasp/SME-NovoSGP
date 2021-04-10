using MediatR;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterVisitasPAAIsUseCase : AbstractUseCase, IObterVisitasPAAIsUseCase
    {
        public ObterVisitasPAAIsUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<ItineranciaVisitaDto>> Executar(FiltroDashboardItineranciaDto param)
        {
            if (param.AnoLetivo == 0)
                param.AnoLetivo = DateTime.Now.Year;

            if (param.Mes == 0)
                param.Mes = 1;

            return await mediator.Send(new ObterItineranciasVisitasPAAIQuery(param.AnoLetivo, param.DreId, param.UeId, param.Mes));
        }
    }
}
