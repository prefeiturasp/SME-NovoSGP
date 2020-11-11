using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace SME.SGP.Aplicacao
{
    public class UploadDeArquivoUseCase : AbstractUseCase, IUploadDeArquivoUseCase
    {
        public UploadDeArquivoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<Guid> Executar(IFormFile file, string tipoConteudo = "")
        {
            return await mediator.Send(new UploadArquivoCommand(file,Dominio.TipoArquivo.Geral,tipoConteudo));
        }

        public Task<Guid> Executar((IFormFile, string) param)
        {
            throw new NotImplementedException();
        }
    }
}
