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
    public class ObterPlanoAulaPorAulaIdQueryHandler : IRequestHandler<ObterPlanoAulaPorAulaIdQuery, PlanoAula>
    {
        private readonly IRepositorioPlanoAula repositorioPlanoAula;

        public ObterPlanoAulaPorAulaIdQueryHandler(IRepositorioPlanoAula repositorioPlanoAula)
        {
            this.repositorioPlanoAula = repositorioPlanoAula ?? throw new ArgumentNullException(nameof(repositorioPlanoAula));
        }

        public async Task<PlanoAula> Handle(ObterPlanoAulaPorAulaIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPlanoAula.ObterPlanoAulaPorAula(request.AulaId);
        }
    }
}
