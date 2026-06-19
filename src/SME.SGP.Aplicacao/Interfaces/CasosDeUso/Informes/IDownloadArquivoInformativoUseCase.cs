using System;

namespace SME.SGP.Aplicacao
{
    public interface IDownloadArquivoInformativoUseCase : IUseCase<Guid, (byte[], string, string)>
    {
    }
}
