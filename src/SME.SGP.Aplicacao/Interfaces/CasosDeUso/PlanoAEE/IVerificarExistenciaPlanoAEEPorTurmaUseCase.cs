using SME.SGP.Infra.Dtos.PlanoAEE;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.PlanoAEE
{
    public interface IVerificarExistenciaPlanoAEEPorTurmaUseCase : IUseCase<FiltroTurmaPlanoAEEDto, IEnumerable<PlanoAEEResumoIntegracaoDto>>
    {

    }
}