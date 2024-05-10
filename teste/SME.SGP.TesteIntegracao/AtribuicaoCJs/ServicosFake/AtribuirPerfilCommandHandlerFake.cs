using MediatR;
using SME.SGP.Aplicacao;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.AtribuicaoCJs.ServicosFake
{
    public class AtribuirPerfilCommandHandlerFake : IRequestHandler<AtribuirPerfilCommand>
    {
        public AtribuirPerfilCommandHandlerFake()
        {
        }
        public async Task<Unit> Handle(AtribuirPerfilCommand request, CancellationToken cancellationToken)
        {
            return Unit.Value;
        }
    }
}
