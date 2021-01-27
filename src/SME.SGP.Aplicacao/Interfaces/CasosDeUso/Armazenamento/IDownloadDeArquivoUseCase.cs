using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public interface IDownloadDeArquivoUseCase : IUseCase<Guid, (byte[], string, string)>
    {
    }
}
