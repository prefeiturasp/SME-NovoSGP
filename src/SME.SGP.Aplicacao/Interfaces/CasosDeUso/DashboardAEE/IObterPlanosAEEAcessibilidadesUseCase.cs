using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IObterPlanosAEEAcessibilidadesUseCase : IUseCase<FiltroDashboardAEEDto, IEnumerable<AEEAcessibilidadeRetornoDto>>
    {
    }
}
