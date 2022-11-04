using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;

namespace SME.SGP.TesteIntegracao.Ocorrencia.ServicosFakes
{
    public class ExcluirOcorrenciaServidorPorIdOcorrenciaCommandHandlerFake : IRequestHandler<ExcluirOcorrenciaServidorPorIdOcorrenciaCommand>
    {
        public async Task<Unit> Handle(ExcluirOcorrenciaServidorPorIdOcorrenciaCommand request, CancellationToken cancellationToken)
        {
            return Unit.Value;
        }
    }
}