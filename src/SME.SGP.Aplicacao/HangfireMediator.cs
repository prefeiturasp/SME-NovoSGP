using MediatR;

namespace SME.SGP.Aplicacao
{
    public class HangfireMediator
    {
        private readonly IMediator _mediator;

        public HangfireMediator(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void SendCommand<T>(IRequest<T> request)
        {
            _mediator.Send(request);
        }
    }
}
