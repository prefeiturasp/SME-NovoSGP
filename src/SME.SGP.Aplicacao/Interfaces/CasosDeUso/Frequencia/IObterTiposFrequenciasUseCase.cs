using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IObterTiposFrequenciasUseCase : IUseCase<TipoFrequenciaFiltroDto, IEnumerable<TipoFrequenciaDto>>
    {
    }
}