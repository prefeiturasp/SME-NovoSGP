using System.Collections.Generic;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterCiclosPorModalidadeECodigoUeUseCase : IUseCase<FiltroCicloPorModalidadeECodigoUeDto, IEnumerable<RetornoCicloDto>>
    {
    }
}
