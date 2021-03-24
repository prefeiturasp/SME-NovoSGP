using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class UploadArquivoEditorUseCase : AbstractUseCase, IUploadArquivoEditorUseCase
    {
        public UploadArquivoEditorUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<RetornoArquivoEditorDto> Executar(IFormFile file,string caminho, TipoArquivo tipoArquivo = TipoArquivo.Geral)
        {
            Guid resposta = await mediator.Send(new UploadArquivoCommand(file, tipoArquivo));
            
            var fileName = $"{resposta}{Path.GetExtension(file.FileName)}";

            return new RetornoArquivoEditorDto()
            {
                Success = true,
                Data = new ArquivoEditorDto()
                {
                    Files = new[] { fileName },
                    BaseUrl = $"{caminho}",
                    Message = "",
                    Error = "",
                    Path = $"{caminho}{fileName}",
                    ContentType = file.ContentType
                }
            };
        }

    }
}
