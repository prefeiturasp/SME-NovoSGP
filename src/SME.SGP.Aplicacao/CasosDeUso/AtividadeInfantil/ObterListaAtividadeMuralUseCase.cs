using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterListaAtividadeMuralUseCase : IObterListaAtividadeMuralUseCase
    {
        private readonly IMediator mediator;
        public ObterListaAtividadeMuralUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<AtividadeInfantilDto>> BuscarPorAulaId(long aulaId)
        {
            return await mediator.Send(new ObterListaAtividadesMuralPorAulaIdQuery(aulaId));
        }
    }
}
