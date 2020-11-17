using Microsoft.AspNetCore.Http;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IUploadDocumentoUseCase
    {
        Task<Guid> Executar(IFormFile file, TipoConteudoArquivo tipoConteudoArquivo = TipoConteudoArquivo.Indefinido);
    }
}
