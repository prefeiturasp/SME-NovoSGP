using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulaPossuiPlanoAulaQueryHandler : IRequestHandler<ObterAulaPossuiPlanoAulaQuery, bool>
    {
        private readonly IRepositorioPlanoAula repositorioPlanoAula;

        public ObterAulaPossuiPlanoAulaQueryHandler(IRepositorioPlanoAula repositorioPlanoAula)
        {
            this.repositorioPlanoAula = repositorioPlanoAula ?? throw new ArgumentNullException(nameof(repositorioPlanoAula));
        }

        public async Task<bool> Handle(ObterAulaPossuiPlanoAulaQuery request, CancellationToken cancellationToken)
            => await repositorioPlanoAula.PlanoAulaRegistradoAsync(request.AulaId);
    }
}
