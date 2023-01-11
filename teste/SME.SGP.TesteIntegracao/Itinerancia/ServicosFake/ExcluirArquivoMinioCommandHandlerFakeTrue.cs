using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Aplicacao;

namespace SME.SGP.TesteIntegracao.Itinerancia.ServicosFake
{
    public class ExcluirArquivoMinioCommandHandlerFakeTrue : IRequestHandler<ExcluirArquivoMinioCommand,bool>
    {
        public async Task<bool> Handle(ExcluirArquivoMinioCommand request, CancellationToken cancellationToken)
        {
            return true;
        }
    }
}