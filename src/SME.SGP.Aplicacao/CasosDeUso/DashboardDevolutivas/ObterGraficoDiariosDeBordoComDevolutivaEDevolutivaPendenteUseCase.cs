using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterGraficoDiariosDeBordoComDevolutivaEDevolutivaPendenteUseCase : IObterGraficoDiariosDeBordoComDevolutivaEDevolutivaPendenteUseCase
    {
        private readonly IMediator mediator;

        public ObterGraficoDiariosDeBordoComDevolutivaEDevolutivaPendenteUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<GraficoDiariosDeBordoComDevolutivaEDevolutivaPendenteDto>> Executar(FiltroGraficoDiariosDeBordoComDevolutivaEDevolutivaPendenteDto filtro)
            => await mediator.Send(new ObterDiariosDeBordoComDevolutivaEDevolutivaPendenteQuery(filtro.AnoLetivo, filtro.Modalidade, DateTime.Today, filtro.DreId, filtro.UeId));
    }
}