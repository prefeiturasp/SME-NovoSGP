using System.Collections.Generic;
using SME.SGP.Infra.Dtos;
using SME.SGP.Infra.Dtos.DashboardFechamento;

namespace SME.SGP.Aplicacao
{
    public interface IObterFechamentoSituacaoUseCase : IUseCase<FiltroDashboardFechamentoDto, IEnumerable<FechamentoSituacaoDto>>
    {
    }
}