using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public interface IUploadDeArquivoUseCase : IUseCase<IFormFile, Guid>
    {
    }
}
