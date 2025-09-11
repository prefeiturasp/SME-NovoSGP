using Microsoft.AspNetCore.Http;
using SME.SGP.Dominio;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IUploadDeArquivoUseCase
    {
        Task<Guid> Executar(IFormFile file, TipoArquivo tipoArquivo = TipoArquivo.Geral);
    }
}
