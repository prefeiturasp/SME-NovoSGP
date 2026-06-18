using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IObterFaltasNaoCompensadaUseCase : IUseCase<FiltroFaltasNaoCompensadasDto, IEnumerable<RegistroFaltasNaoCompensadaDto>>
    {

    }
}