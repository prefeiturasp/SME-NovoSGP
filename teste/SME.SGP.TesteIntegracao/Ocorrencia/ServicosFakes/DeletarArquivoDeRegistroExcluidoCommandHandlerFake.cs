using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;

namespace SME.SGP.TesteIntegracao.Ocorrencia.ServicosFakes
{
    public class DeletarArquivoDeRegistroExcluidoCommandHandlerFake : IRequestHandler<DeletarArquivoDeRegistroExcluidoCommand, bool>
    {
        public async Task<bool> Handle(DeletarArquivoDeRegistroExcluidoCommand request, CancellationToken cancellationToken)
        {
            return true;
        }
    }
}