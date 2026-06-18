using System;

namespace SME.SGP.Aplicacao
{
    public interface IDownloadDeArquivoUseCase : IUseCase<Guid, (byte[], string, string)>
    {
    }
}
