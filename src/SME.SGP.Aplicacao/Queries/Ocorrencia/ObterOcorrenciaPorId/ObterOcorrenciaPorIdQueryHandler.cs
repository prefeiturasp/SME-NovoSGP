using MediatR;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterOcorrenciaPorIdQueryHandler : IRequestHandler<ObterOcorrenciaPorIdQuery, OcorrenciaDto>
    {
        public Task<OcorrenciaDto> Handle(ObterOcorrenciaPorIdQuery request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
