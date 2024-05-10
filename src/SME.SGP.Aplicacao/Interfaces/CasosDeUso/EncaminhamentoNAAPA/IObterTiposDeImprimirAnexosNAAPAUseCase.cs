using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IObterTiposDeImprimirAnexosNAAPAUseCase : IUseCase<long, IEnumerable<ImprimirAnexoDto>>
    {
    }
}
