using System.Collections.Generic;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public interface IObterAlunosSemNotasRecomendacoesUseCase: IUseCase<FiltroInconsistenciasAlunoFamiliaDto,IEnumerable<InconsistenciasAlunoFamiliaDto>>

    {

    }
}