using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IObterDashboardAcompanhamentoAprendizagemUseCase : IUseCase<FiltroDashboardAcompanhamentoAprendizagemDto, IEnumerable<GraficoBaseDto>>
    {
    }
}
