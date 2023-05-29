using MediatR;
using SME.SGP.Aplicacao;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.ServicosFakes
{
    public class RecalcularFrequenciaPorTurmaCommandHandlerFake : IRequestHandler<RecalcularFrequenciaPorTurmaCommand, bool>
    {
        public Task<bool> Handle(RecalcularFrequenciaPorTurmaCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }
}
