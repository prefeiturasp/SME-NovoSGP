using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeTotalDeDevolutivasPorDREUseCase : IObterQuantidadeTotalDeDevolutivasPorDREUseCase
    {
        private readonly IMediator mediator;
        public ObterQuantidadeTotalDeDevolutivasPorDREUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<IEnumerable<GraficoTotalDevolutivasPorAnoDTO>> Executar(FiltroDasboardDiarioBordoDevolutivasDto filtro)
        {
            return await mediator.Send(new ObterQuantidadeTotalDeDevolutivasPorAnoDreQuery(filtro.AnoLetivo, filtro.Mes, filtro.DreId));
        }
    }
}
