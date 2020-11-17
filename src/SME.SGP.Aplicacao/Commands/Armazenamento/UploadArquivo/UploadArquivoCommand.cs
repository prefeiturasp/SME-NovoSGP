using MediatR;
using Microsoft.AspNetCore.Http;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class UploadArquivoCommand : IRequest<Guid>
    {
        public UploadArquivoCommand(IFormFile arquivo, TipoArquivo tipoArquivo = TipoArquivo.Geral)
        {
            Arquivo = arquivo;
            Tipo = tipoArquivo;
        }

        public IFormFile Arquivo { get; set; }
        public TipoArquivo Tipo { get; set; }
    }
}
