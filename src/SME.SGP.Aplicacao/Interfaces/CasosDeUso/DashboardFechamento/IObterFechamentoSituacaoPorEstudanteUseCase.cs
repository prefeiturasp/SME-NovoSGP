using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IObterFechamentoSituacaoPorEstudanteUseCase : IUseCase<FiltroDashboardFechamentoDto, IEnumerable<GraficoBaseDto>>
    {
    }
}