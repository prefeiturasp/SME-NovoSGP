using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AulaExisteQueryHandler : IRequestHandler<AulaExisteQuery, bool>
    {
        private readonly IRepositorioAula repositorioAula;

        public AulaExisteQueryHandler(IRepositorioAula repositorioAula)
        {
            this.repositorioAula = repositorioAula ?? throw new ArgumentNullException(nameof(repositorioAula));
        }

        public async Task<bool> Handle(AulaExisteQuery request, CancellationToken cancellationToken)
            => await repositorioAula.Exists(request.AulaId);
    }
}
