using MediatR;
using SME.SGP.Aplicacao;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Itinerancia.ServicosFake
{
    public class ExcluirArquivoMinioCommandHandlerFakeTrue : IRequestHandler<ExcluirArquivoMinioCommand, bool>
    {
        public async Task<bool> Handle(ExcluirArquivoMinioCommand request, CancellationToken cancellationToken)
        {
            return true;
        }
    }
}