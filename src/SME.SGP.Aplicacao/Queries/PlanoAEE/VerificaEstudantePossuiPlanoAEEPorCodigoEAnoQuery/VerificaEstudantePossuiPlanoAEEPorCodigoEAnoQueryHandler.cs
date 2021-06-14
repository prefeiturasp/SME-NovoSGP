using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQueryHandler : IRequestHandler<VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQuery, bool>
    {
        private readonly IRepositorioPlanoAEE repositorioPlanoAEE;

        public VerificaEstudantePossuiPlanoAEEPorCodigoEAnoQueryHandler(IRepositorioPlanoAEE repositorioPlanoAEE)
        {
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
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

