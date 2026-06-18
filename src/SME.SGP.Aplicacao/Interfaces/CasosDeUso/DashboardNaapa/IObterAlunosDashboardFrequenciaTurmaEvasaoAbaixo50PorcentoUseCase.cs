using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IObterAlunosDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoUseCase : IUseCase<FiltroGraficoFrequenciaTurmaEvasaoAlunoDto, PaginacaoResultadoDto<AlunoFrequenciaTurmaEvasaoDto>>
    {
    }
}
