using MediatR;
using SME.SGP.Aplicacao;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao
{
    public class PublicarFilaEmLoteSgpCommandHandlerFake : IRequestHandler<PublicarFilaEmLoteSgpCommand, bool>
    {
        public Task<bool> Handle(PublicarFilaEmLoteSgpCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }
}
