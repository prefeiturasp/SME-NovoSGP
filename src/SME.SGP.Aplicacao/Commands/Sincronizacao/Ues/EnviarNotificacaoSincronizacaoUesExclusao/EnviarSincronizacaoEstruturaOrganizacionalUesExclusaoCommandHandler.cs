using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class EnviarSincronizacaoEstruturaOrganizacionalUesExclusaoCommandHandler : IRequestHandler<EnviarSincronizacaoEstruturaOrganizacionalUesExclusaoCommand, bool>
    {
        private readonly IMediator mediator;

        public EnviarSincronizacaoEstruturaOrganizacionalUesExclusaoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(EnviarSincronizacaoEstruturaOrganizacionalUesExclusaoCommand request, CancellationToken cancellationToken)
        {
            return true;
        }
    }
}
