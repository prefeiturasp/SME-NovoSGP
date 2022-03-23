using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IObterDashboardAcompanhamentoAprendizagemPorDreUseCase : IUseCase<FiltroDashboardAcompanhamentoAprendizagemPorDreDto, IEnumerable<GraficoBaseDto>>
    {
    }
}
