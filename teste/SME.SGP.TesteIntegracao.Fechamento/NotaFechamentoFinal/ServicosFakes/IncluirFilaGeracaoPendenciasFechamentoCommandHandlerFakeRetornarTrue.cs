using MediatR;
using SME.SGP.Aplicacao;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.NotaFechamento.ServicosFakes
{
    public class IncluirFilaGeracaoPendenciasFechamentoCommandHandlerFakeRetornarTrue : IRequestHandler<IncluirFilaGeracaoPendenciasFechamentoCommand, bool>
    {
        public IncluirFilaGeracaoPendenciasFechamentoCommandHandlerFakeRetornarTrue() { }

        public async Task<bool> Handle(IncluirFilaGeracaoPendenciasFechamentoCommand request, CancellationToken cancellationToken)
        {
            return await Task.FromResult(true);
        }
    }
}
