using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterMuralAvisosUseCase : IObterMuralAvisosUseCase
    {
        private readonly IMediator mediator;

        public ObterMuralAvisosUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<MuralAvisosRetornoDto>> BuscarPorAulaId(long aulaId)
        {
            return await mediator.Send(new ObterMuralAvisoPorAulaIdQuery(aulaId));
        }
    }
}