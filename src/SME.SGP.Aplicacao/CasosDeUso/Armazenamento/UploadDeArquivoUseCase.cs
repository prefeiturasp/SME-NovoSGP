using MediatR;
using Microsoft.AspNetCore.Http;
using SME.SGP.Dominio;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class UploadDeArquivoUseCase : AbstractUseCase, IUploadDeArquivoUseCase
    {
        public UploadDeArquivoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<Guid> Executar(IFormFile file, TipoArquivo tipoArquivo = TipoArquivo.Geral)
        {
            return await mediator.Send(new UploadArquivoCommand(file, tipoArquivo));
        }

    }
}
