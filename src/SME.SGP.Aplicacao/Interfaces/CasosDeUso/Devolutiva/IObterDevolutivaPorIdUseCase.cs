using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public interface IObterDevolutivaPorIdUseCase : IUseCase<long, DevolutivaDto>
    {
    }
}
