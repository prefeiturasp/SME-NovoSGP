using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Commands.Aulas.InserirAula
{
    public class InserirAulaRecorrenteCommandHandler : IRequestHandler<InserirAulaRecorrenteCommand, bool>
    {
        public Task<bool> Handle(InserirAulaRecorrenteCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
