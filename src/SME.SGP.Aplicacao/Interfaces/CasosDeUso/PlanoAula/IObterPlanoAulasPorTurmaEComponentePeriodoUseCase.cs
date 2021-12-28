using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IObterPlanoAulasPorTurmaEComponentePeriodoUseCase : IUseCase<FiltroObterPlanoAulaPeriodoDto, IEnumerable<PlanoAulaRetornoDto>>
    {}
}
