using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeTotalDeDiariosEDevolutivasPorAnoTurmaUseCase : IObterQuantidadeTotalDeDiariosEDevolutivasPorAnoTurmaUseCase
    {
        private readonly IMediator mediator;

        public ObterQuantidadeTotalDeDiariosEDevolutivasPorAnoTurmaUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<GraficoBaseDto>> Executar(FiltroDasboardDiarioBordoDto filtro)
        {
            return await mediator.Send(new ObterQuantidadeTotalDeDiariosEDevolutivasPorAnoTurmaQuery(filtro.AnoLetivo, filtro.DreId, filtro.UeId, filtro.Modalidade));
        }
    }
}
