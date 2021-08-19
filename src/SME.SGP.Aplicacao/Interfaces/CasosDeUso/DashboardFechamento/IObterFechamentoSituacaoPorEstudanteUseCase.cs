using System.Collections.Generic;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IObterFechamentoSituacaoPorEstudanteUseCase : IUseCase<FiltroDashboardFechamentoDto, IEnumerable<FechamentoSituacaoPorEstudanteDto>>
    {
    }
}