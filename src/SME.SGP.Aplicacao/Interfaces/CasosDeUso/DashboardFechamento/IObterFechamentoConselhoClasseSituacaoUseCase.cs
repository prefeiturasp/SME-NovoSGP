using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IObterFechamentoConselhoClasseSituacaoUseCase : IUseCase<FiltroDashboardFechamentoDto, IEnumerable<GraficoBaseDto>>
    {
    }
}