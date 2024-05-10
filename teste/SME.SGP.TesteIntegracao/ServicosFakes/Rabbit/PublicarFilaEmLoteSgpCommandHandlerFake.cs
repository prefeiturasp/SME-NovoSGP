using MediatR;
using SME.SGP.Aplicacao;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao
{
    public class PublicarFilaEmLoteSgpCommandHandlerFake : IRequestHandler<PublicarFilaEmLoteSgpCommand, bool>
    {
        private readonly IMediator mediator;

        public PublicarFilaEmLoteSgpCommandHandlerFake(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(PublicarFilaEmLoteSgpCommand request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(true);
        }
    }
}
