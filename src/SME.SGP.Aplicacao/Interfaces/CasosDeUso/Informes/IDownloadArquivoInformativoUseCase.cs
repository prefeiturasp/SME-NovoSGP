using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public interface IDownloadArquivoInformativoUseCase : IUseCase<Guid, (byte[], string, string)>
    {
    }
}
