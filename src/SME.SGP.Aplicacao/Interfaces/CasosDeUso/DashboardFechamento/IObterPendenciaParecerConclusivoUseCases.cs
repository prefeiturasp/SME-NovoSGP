﻿using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IObterPendenciaParecerConclusivoUseCases : IUseCase<FiltroDashboardFechamentoDto, IEnumerable<GraficoBaseDto>>
    {
    }
}
