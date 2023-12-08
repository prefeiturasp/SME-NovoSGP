using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;

namespace SME.SGP.TesteIntegracao.Ocorrencia.ServicosFakes
{
    public class RemoverArquivosExcluidosCommandHandlerFake : IRequestHandler<RemoverArquivosExcluidosCommand, bool>
    {
        public async Task<bool> Handle(RemoverArquivosExcluidosCommand request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(true);
        }
    }
}