using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ListarOcorrenciasQueryHandler : IRequestHandler<ListarOcorrenciasQuery, IEnumerable<OcorrenciaListagemDto>>
    {
        public Task<IEnumerable<OcorrenciaListagemDto>> Handle(ListarOcorrenciasQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
