using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;

namespace SME.SGP.Aplicacao
{
    public interface IObterDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoUseCase : IUseCase<FiltroGraficoFrequenciaTurmaEvasaoDto, FrequenciaTurmaEvasaoDto>
    {
    }
}
