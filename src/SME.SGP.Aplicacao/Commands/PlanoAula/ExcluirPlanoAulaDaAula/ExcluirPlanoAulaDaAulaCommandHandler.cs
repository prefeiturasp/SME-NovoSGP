using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPlanoAulaDaAulaCommandHandler : IRequestHandler<ExcluirPlanoAulaDaAulaCommand, bool>
    {
        private readonly IRepositorioPlanoAula repositorioPlanoAula;
        private readonly IMediator mediator;

        public ExcluirPlanoAulaDaAulaCommandHandler(IRepositorioPlanoAula repositorioPlanoAula, IMediator mediator)
        {
            this.repositorioPlanoAula = repositorioPlanoAula ?? throw new ArgumentNullException(nameof(repositorioPlanoAula));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExcluirPlanoAulaDaAulaCommand request, CancellationToken cancellationToken)
        {
            await repositorioPlanoAula.ExcluirPlanoDaAula(request.AulaId);
            return true;
        }
    }
}
