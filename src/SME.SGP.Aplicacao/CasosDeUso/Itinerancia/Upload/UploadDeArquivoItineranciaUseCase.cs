using MediatR;
using Microsoft.AspNetCore.Http;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class UploadDeArquivoItineranciaUseCase : AbstractUseCase, IUploadDeArquivoItineranciaUseCase
    {
        public UploadDeArquivoItineranciaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<ArquivoArmazenadoItineranciaDto> Executar(IFormFile file)
        {
            return await mediator.Send(new UploadArquivoItineranciaCommand(file));
        }
    }
}