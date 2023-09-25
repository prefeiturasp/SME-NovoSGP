using MediatR;
using SME.SGP.Aplicacao;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.AtribuicaoCJs.ServicosFake
{
    public class RemoverPerfisUsuarioAtualCommandHandlerFake : IRequestHandler<RemoverPerfisUsuarioAtualCommand>
    {
        public RemoverPerfisUsuarioAtualCommandHandlerFake()
        {
        }

        public async Task<Unit> Handle(RemoverPerfisUsuarioAtualCommand request, CancellationToken cancellationToken)
        {
            return Unit.Value;
        }
    }
}
