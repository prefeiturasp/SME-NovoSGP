using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public interface IObterDocumentoUseCase : IUseCase<long, ObterDocumentoDto>
    {
    }
}
