using System;

namespace SME.SGP.Aplicacao
{
    public interface IExcluirDocumentoArquivoUseCase : IUseCase<(long DocumentoId, Guid CodigoArquivo), bool>
    {

    }
}
