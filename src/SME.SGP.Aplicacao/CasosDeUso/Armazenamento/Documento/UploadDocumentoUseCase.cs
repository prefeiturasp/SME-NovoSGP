using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class UploadDocumentoUseCase : AbstractUseCase, IUploadDocumentoUseCase
    {
        public UploadDocumentoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<Guid> Executar(IFormFile file)
        {
            return await mediator.Send(new UploadArquivoCommand(file));
        }
    }
}
