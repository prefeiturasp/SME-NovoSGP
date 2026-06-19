using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public interface IUploadDocumentoUseCase
    {
        Task<Guid> Executar(IFormFile file);
    }
}
