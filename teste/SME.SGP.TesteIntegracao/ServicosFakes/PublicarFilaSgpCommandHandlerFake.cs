using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class PublicarFilaSgpCommandHandlerFake : IRequestHandler<PublicarFilaSgpCommand, bool>
    {
        public Task<bool> Handle(PublicarFilaSgpCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }
}