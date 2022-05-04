using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoUseCase : IUseCase<FiltroGraficoFrequenciaTurmaEvasaoDto, IEnumerable<GraficoFrequenciaTurmaEvasaoDto>>
    {
    }
}
