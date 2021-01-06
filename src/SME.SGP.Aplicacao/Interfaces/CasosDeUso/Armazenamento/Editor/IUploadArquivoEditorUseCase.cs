using Microsoft.AspNetCore.Http;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IUploadArquivoEditorUseCase
    {
        Task<RetornoArquivoEditorDto> Executar(IFormFile arquivo, string caminho, TipoArquivo tipoArquivo = TipoArquivo.Geral);
    }
}
