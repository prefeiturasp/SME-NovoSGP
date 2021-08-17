using System.Collections.Generic;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public interface IObterFechamentoSituacaoPorEstudanteUseCase : IUseCase<FiltroDashboardFechamentoDto, IEnumerable<FechamentoSituacaoPorEstudanteDto>>
    {
    }
}