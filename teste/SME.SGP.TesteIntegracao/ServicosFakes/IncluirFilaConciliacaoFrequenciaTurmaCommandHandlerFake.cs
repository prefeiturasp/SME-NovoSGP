using MediatR;
using SME.SGP.Aplicacao;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao
{
    public class IncluirFilaConciliacaoFrequenciaTurmaCommandHandlerFake : IRequestHandler<IncluirFilaConciliacaoFrequenciaTurmaCommand, bool>
    {
        public Task<bool> Handle(IncluirFilaConciliacaoFrequenciaTurmaCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }
}
