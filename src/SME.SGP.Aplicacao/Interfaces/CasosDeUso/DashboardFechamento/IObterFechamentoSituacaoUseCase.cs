using System.Collections.Generic;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public interface IObterFechamentoSituacaoUseCase : IUseCase<FiltroDashboardFechamentoDto, IEnumerable<FechamentoSituacaoDto>>
    {
    }
}