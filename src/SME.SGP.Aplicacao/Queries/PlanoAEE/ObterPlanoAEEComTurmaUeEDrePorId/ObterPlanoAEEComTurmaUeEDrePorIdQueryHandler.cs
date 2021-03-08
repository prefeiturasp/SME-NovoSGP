using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanoAEEComTurmaUeEDrePorIdQueryHandler : IRequestHandler<ObterPlanoAEEComTurmaUeEDrePorIdQuery, PlanoAEE>
    {
        private readonly IRepositorioPlanoAEE repositorioPlanoAEE;

        public ObterPlanoAEEComTurmaUeEDrePorIdQueryHandler(IRepositorioPlanoAEE repositorioPlanoAEE)
        {
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
        }

        public async Task<PlanoAEE> Handle(ObterPlanoAEEComTurmaUeEDrePorIdQuery request, CancellationToken cancellationToken)
            => await repositorioPlanoAEE.ObterPlanoComTurmaUeDrePorId(request.PlanoId);
    }
}
