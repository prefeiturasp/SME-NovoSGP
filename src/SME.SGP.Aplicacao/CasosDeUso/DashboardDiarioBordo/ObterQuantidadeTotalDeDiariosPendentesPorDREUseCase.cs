using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuantidadeTotalDeDiariosPendentesPorDREUseCase : IObterQuantidadeTotalDeDiariosPendentesPorDREUseCase
    {
        private readonly IMediator mediator;

        public ObterQuantidadeTotalDeDiariosPendentesPorDREUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<GraficoTotalDiariosEDevolutivasPorDreDTO>> Executar(int anoLetivo, string ano)
        {
            return await mediator.Send(new ObterQuantidadeTotalDeDiariosPendentesPorDreQuery(anoLetivo, ano));
        }
    }
}
