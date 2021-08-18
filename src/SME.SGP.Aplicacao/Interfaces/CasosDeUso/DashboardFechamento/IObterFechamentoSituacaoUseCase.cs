using System.Collections.Generic;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IObterFechamentoSituacaoUseCase : IUseCase<FiltroDashboardFechamentoDto, IEnumerable<FechamentoSituacaoDto>>
    {
    }
}