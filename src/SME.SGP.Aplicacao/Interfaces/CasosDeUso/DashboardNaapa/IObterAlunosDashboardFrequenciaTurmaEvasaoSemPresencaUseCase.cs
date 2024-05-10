using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IObterAlunosDashboardFrequenciaTurmaEvasaoSemPresencaUseCase : IUseCase<FiltroGraficoFrequenciaTurmaEvasaoAlunoDto, PaginacaoResultadoDto<AlunoFrequenciaTurmaEvasaoDto>>
    {
    }
}
