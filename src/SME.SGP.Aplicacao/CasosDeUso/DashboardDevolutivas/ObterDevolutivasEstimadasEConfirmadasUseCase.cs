using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterDevolutivasEstimadasEConfirmadasUseCase : IObterDevolutivasEstimadasEConfirmadasUseCase
    {
        private readonly IMediator mediator;

        public ObterDevolutivasEstimadasEConfirmadasUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<GraficoDevolutivasEstimadasEConfirmadasDto>> Executar(FiltroGraficoDevolutivasEstimadasEConfirmadasDto filtro)
            => await mediator.Send(new ObterDevolutivasEstimadasEConfirmadasQuery(filtro.AnoLetivo, filtro.Modalidade, filtro.DreId, filtro.UeId));
    }
}