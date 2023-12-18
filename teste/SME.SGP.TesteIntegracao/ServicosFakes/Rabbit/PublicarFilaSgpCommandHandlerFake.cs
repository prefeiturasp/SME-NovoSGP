using MediatR;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class PublicarFilaSgpCommandHandlerFake : IRequestHandler<PublicarFilaSgpCommand, bool>
    {
        private readonly IMediator mediator;
        public readonly IUnitOfWork unitOfWork;

        public PublicarFilaSgpCommandHandlerFake(IMediator mediator, IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Handle(PublicarFilaSgpCommand request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(true);
        }
    }
}