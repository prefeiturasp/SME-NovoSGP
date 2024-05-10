using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class UploadDeArquivoItineranciaUseCase : AbstractUseCase,IUploadDeArquivoItineranciaUseCase
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