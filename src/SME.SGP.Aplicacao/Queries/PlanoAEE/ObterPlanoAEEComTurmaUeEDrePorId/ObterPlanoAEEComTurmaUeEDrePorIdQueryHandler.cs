using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanoAEEComTurmaUeEDrePorIdQueryHandler : IRequestHandler<ObterPlanoAEEComTurmaUeEDrePorIdQuery, PlanoAEE>
    {
        private readonly IRepositorioPlanoAEEConsulta repositorioPlanoAEE;

        public ObterPlanoAEEComTurmaUeEDrePorIdQueryHandler(IRepositorioPlanoAEEConsulta repositorioPlanoAEE)
        {
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
        }

        public async Task<PlanoAEE> Handle(ObterPlanoAEEComTurmaUeEDrePorIdQuery request, CancellationToken cancellationToken)
            => await repositorioPlanoAEE.ObterPlanoComTurmaUeDrePorId(request.PlanoId);
    }
}
