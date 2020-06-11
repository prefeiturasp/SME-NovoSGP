using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ReceberRelatorioProntoCommandHandler : IRequestHandler<ReceberRelatorioProntoCommand, bool>
    {
        public Task<bool> Handle(ReceberRelatorioProntoCommand request, CancellationToken cancellationToken)
        {
            //TODO TRATAR RELATORIO GRAVAR CORRELAÇÃO
            return Task.FromResult(true);
        }
    }
}
