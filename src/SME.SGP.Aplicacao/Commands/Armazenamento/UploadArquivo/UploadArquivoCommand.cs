using MediatR;
using Microsoft.AspNetCore.Http;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class UploadArquivoCommand : IRequest<ArquivoArmazenadoDto>
    {
        public UploadArquivoCommand(IFormFile arquivo, TipoArquivo tipoArquivo = TipoArquivo.Geral, TipoConteudoArquivo tipoConteudo = TipoConteudoArquivo.Indefinido)
        {
            Arquivo = arquivo;
            Tipo = tipoArquivo;
            TipoConteudo = tipoConteudo;
        }

        public IFormFile Arquivo { get; set; }
        public TipoArquivo Tipo { get; set; }
        public TipoConteudoArquivo TipoConteudo { get; set; }
    }
}
