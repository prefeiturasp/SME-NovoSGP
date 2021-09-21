using System.Collections.Generic;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IObterFechamentoConselhoClasseSituacaoUseCase : IUseCase<FiltroDashboardFechamentoDto, IEnumerable<GraficoBaseDto>>
    {
    }
}