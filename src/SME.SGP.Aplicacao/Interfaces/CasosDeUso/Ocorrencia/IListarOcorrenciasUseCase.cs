using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public interface IListarOcorrenciasUseCase : IUseCase<FiltroOcorrenciaListagemDto, IEnumerable<OcorrenciaListagemDto>>
    {
    }
}
