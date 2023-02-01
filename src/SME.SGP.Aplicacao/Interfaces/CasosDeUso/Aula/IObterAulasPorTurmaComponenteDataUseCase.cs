using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Interfaces
{
    public interface IObterAulasPorTurmaComponenteDataUseCase: IUseCase<FiltroObterAulasPorTurmaComponenteDataDto, IEnumerable<AulaQuantidadeTipoDto>>
    {}
}
