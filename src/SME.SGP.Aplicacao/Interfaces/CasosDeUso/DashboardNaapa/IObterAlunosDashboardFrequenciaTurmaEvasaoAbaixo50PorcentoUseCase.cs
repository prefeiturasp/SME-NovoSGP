using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IObterAlunosDashboardFrequenciaTurmaEvasaoAbaixo50PorcentoUseCase : IUseCase<FiltroGraficoFrequenciaTurmaEvasaoAlunoDto, PaginacaoResultadoDto<AlunoFrequenciaTurmaEvasaoDto>>
    {
    }
}
