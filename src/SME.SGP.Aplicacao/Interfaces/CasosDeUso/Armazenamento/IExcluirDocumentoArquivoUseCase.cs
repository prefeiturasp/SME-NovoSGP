using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public interface IExcluirDocumentoArquivoUseCase : IUseCase<(long DocumentoId, Guid CodigoArquivo), bool>
    {

    }
}
