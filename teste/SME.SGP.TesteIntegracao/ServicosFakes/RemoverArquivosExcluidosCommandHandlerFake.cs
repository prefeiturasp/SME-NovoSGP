using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class RemoverArquivosExcluidosCommandHandlerFake : IRequestHandler<RemoverArquivosExcluidosCommand, bool>
    {
        private readonly IMediator mediator;


        public RemoverArquivosExcluidosCommandHandlerFake(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(RemoverArquivosExcluidosCommand request, CancellationToken cancellationToken)
        {   
            return await Task.FromResult(true);
        }
    }
}