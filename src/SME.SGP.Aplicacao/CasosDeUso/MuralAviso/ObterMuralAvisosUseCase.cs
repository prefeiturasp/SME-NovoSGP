using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterMuralAvisosUseCase : IObterMuralAvisosUseCase
    {
        private readonly IMediator mediator;

        public ObterMuralAvisosUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IList<MuralAvisosRetornoDto>> BuscarPorAulaId(long aulaId)
        {
            return await mediator.Send(new ObterMuralAvisoPorAulaIdQuery(aulaId));
        }
    }
}