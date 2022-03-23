using System;
using System.Collections.Generic;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IObterFrequenciasPorPeriodoUseCase : IUseCase<FiltroFrequenciaPorPeriodoDto, RegistroFrequenciaPorDataPeriodoDto>
    {
    }
}
