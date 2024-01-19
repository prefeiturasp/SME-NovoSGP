using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class PublicarFilaApiEOLCommandHandlerFake : IRequestHandler<PublicarFilaApiEOLCommand, bool>
    {
        public async Task<bool> Handle(PublicarFilaApiEOLCommand request, CancellationToken cancellationToken)
        {
            return true;
        }
    }
}