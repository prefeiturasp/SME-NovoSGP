using Hangfire;
using MediatR;
using SME.SGP.Aplicacao;
using System;

namespace SME.Background.Hangfire
{
    public class MediatRJobActivator : JobActivator
    {
        private readonly IMediator _mediator;

        public MediatRJobActivator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override object ActivateJob(Type type)
        {
            return new HangfireMediator(_mediator);
        }
    }
}
