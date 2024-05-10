using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanoAEEPorReestruturacaoIdQueryHandler : IRequestHandler<ObterPlanoAEEPorReestruturacaoIdQuery, PlanoAEE>
    {
        private readonly IRepositorioPlanoAEEConsulta repositorioPlanoAEE;

        public ObterPlanoAEEPorReestruturacaoIdQueryHandler(IRepositorioPlanoAEEConsulta repositorioPlanoAEE)
        {
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
        }

        public async Task<PlanoAEE> Handle(ObterPlanoAEEPorReestruturacaoIdQuery request, CancellationToken cancellationToken)
            => await repositorioPlanoAEE.ObterPorReestruturacaoId(request.ReestruturacaoId);
    }
}
