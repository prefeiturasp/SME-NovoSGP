﻿using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IObterPAAIPorDreUseCase : IUseCase<string, IEnumerable<SupervisorEscolasDreDto>>
    {
    }
}
