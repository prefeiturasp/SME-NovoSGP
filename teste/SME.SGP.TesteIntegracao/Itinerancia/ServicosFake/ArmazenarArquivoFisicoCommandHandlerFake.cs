using MediatR;
using SME.SGP.Aplicacao;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.TesteIntegracao.Itinerancia.ServicosFake
{
    public class ArmazenarArquivoFisicoCommandHandlerFake : IRequestHandler<ArmazenarArquivoFisicoCommand, string>
    {
        public async Task<string> Handle(ArmazenarArquivoFisicoCommand request, CancellationToken cancellationToken)
        {
            return $"https://localhostr/arquivos/${request.NomeFisico}";
        }
    }
}