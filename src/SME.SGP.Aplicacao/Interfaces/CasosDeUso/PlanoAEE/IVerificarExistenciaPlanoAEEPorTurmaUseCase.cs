using System.Collections.Generic;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PlanoAEE;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.PlanoAEE;

public interface IVerificarExistenciaPlanoAEEPorTurmaUseCase : IUseCase<FiltroTurmaPlanoAEEDto, IEnumerable<PlanoAEEResumoIntegracaoDto>>
{
    
}