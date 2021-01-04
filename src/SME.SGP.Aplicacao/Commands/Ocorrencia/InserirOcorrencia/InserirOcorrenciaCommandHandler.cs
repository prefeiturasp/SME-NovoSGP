using MediatR;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class InserirOcorrenciaCommandHandler : IRequestHandler<InserirOcorrenciaCommand, AuditoriaDto>
    {
        public Task<AuditoriaDto> Handle(InserirOcorrenciaCommand request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
