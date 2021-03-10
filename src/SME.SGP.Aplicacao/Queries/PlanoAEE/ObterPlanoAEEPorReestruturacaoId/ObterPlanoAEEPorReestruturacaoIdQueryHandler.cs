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
    public class ObterPlanoAEEPorReestruturacaoIdQueryHandler : IRequestHandler<ObterPlanoAEEPorReestruturacaoIdQuery, PlanoAEE>
    {
        private readonly IRepositorioPlanoAEE repositorioPlanoAEE;

        public ObterPlanoAEEPorReestruturacaoIdQueryHandler(IRepositorioPlanoAEE repositorioPlanoAEE)
        {
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
        }

        public async Task<PlanoAEE> Handle(ObterPlanoAEEPorReestruturacaoIdQuery request, CancellationToken cancellationToken)
            => await repositorioPlanoAEE.ObterPorReestruturacaoId(request.ReestruturacaoId);
    }
}
