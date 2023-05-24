using System;
using MediatR;
using SME.SGP.Aplicacao;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.CasosDeUso;
using SME.SGP.Infra;

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
            return true;
        }
    }
}
