using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IObterAlunosSemNotasRecomendacoesUseCase : IUseCase<FiltroInconsistenciasAlunoFamiliaDto, IEnumerable<InconsistenciasAlunoFamiliaDto>>

    {

    }
}