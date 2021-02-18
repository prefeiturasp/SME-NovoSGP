using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class UploadDocumentoUseCase : AbstractUseCase, IUploadDocumentoUseCase
    {
        public UploadDocumentoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<Guid> Executar(IFormFile file, TipoConteudoArquivo tipoConteudoArquivo = TipoConteudoArquivo.Indefinido)
        {
            return await mediator.Send(new UploadArquivoCommand(file, TipoArquivo.Geral, tipoConteudoArquivo));
        }
    }
}
