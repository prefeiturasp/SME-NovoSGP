using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAulaEmManutencaoQueryHandler : IRequestHandler<ObterAulaEmManutencaoQuery, bool>
    {
        private readonly IRepositorioProcessoExecutando repositorio;

        public ObterAulaEmManutencaoQueryHandler(IRepositorioProcessoExecutando repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<bool> Handle(ObterAulaEmManutencaoQuery request, CancellationToken cancellationToken)
            => await repositorio.ObterAulaEmManutencaoAsync(request.AulaId);
    }
}
