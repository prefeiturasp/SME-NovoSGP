using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciasEncaminhamentoAEECEFAICommandHandler : IRequestHandler<ExcluirPendenciasEncaminhamentoAEECEFAICommand, bool>
    {
        private readonly IMediator mediator;

        public ExcluirPendenciasEncaminhamentoAEECEFAICommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExcluirPendenciasEncaminhamentoAEECEFAICommand request, CancellationToken cancellationToken)
        {
            var pendencia = await mediator.Send(new ObterPendenciaEncaminhamentoAEEPorIdQuery(request.EncaminhamentoAEEId));
            if (pendencia != null)
                await mediator.Send(new ExcluirPendenciaEncaminhamentoAEECommand(pendencia.PendenciaId));

            return true;
        }
    }
}
