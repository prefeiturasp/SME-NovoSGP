using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IObterAlunosDashboardFrequenciaTurmaEvasaoSemPresencaUseCase : IUseCase<FiltroGraficoFrequenciaTurmaEvasaoAlunoDto, PaginacaoResultadoDto<AlunoFrequenciaTurmaEvasaoDto>>
    {
    }
}
