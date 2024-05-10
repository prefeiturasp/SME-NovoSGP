using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public abstract class AbstractUseCase
    {
        protected readonly IMediator mediator;

        protected AbstractUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
    }
}
