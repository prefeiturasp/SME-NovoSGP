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
    public class ObterDevolutivaPorIdQueryHandler : IRequestHandler<ObterDevolutivaPorIdQuery, Devolutiva>
    {
        private readonly IRepositorioDevolutiva repositorioDevolutiva;

        public ObterDevolutivaPorIdQueryHandler(IRepositorioDevolutiva repositorioDevolutiva)
        {
            this.repositorioDevolutiva = repositorioDevolutiva ?? throw new ArgumentNullException(nameof(repositorioDevolutiva));
        }

        public async Task<Devolutiva> Handle(ObterDevolutivaPorIdQuery request, CancellationToken cancellationToken)
            => await repositorioDevolutiva.ObterPorIdAsync(request.DevolutivaId);
    }
}
