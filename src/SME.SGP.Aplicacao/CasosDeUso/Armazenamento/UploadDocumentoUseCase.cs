using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace SME.SGP.Aplicacao
{
    public class UploadDocumentoUseCase : AbstractUseCase, IUploadDocumentoUseCase
    {
        public UploadDocumentoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<Guid> Executar(IFormFile file, TipoConteudoArquivo )
        {
            return await mediator.Send(new UploadArquivoCommand(file,Dominio.TipoArquivo.Geral, Dominio.TipoConteudoArquivo.Indefinido));
        }

        public Task<Guid> Executar((IFormFile, string) param)
        {
            throw new NotImplementedException();
        }
    }
}
