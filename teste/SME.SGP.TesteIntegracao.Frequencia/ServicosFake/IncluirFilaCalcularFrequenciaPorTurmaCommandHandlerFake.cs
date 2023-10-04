using MediatR;
using SME.SGP.Aplicacao;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Frequencia.ServicosFake
{
    public class IncluirFilaCalcularFrequenciaPorTurmaCommandHandlerFake : IRequestHandler<IncluirFilaCalcularFrequenciaPorTurmaCommand, bool>
    {
        public Task<bool> Handle(IncluirFilaCalcularFrequenciaPorTurmaCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }
    }
}
