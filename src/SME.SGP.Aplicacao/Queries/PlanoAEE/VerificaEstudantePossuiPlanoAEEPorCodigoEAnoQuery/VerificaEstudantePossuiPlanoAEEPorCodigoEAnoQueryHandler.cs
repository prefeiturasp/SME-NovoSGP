using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQueryHandler : IRequestHandler<VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery, bool>
    {
        private readonly IRepositorioPlanoAEE repositorioPlanoAEE;
        private readonly IMediator mediator;

        public VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQueryHandler(IRepositorioPlanoAEE repositorioPlanoAEE, IMediator mediator)
        {
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery request, CancellationToken cancellationToken)
        {
            var planoAEE = await repositorioPlanoAEE.ObterPlanoPorEstudanteEAno(request.CodigoEstudante, request.AnoLetivo);
            if (planoAEE == null)
                return false;

            return true;
        }
    }
}

