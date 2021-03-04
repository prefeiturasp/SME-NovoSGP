using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanoAEEComTurmaPorIdQueryHandler : IRequestHandler<ObterPlanoAEEComTurmaPorIdQuery, PlanoAEE>
    {
        private readonly IRepositorioPlanoAEE repositorioPlanoAEE;

        public ObterPlanoAEEComTurmaPorIdQueryHandler(IRepositorioPlanoAEE repositorioPlanoAEE)
        {
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
        }

        public async Task<PlanoAEE> Handle(ObterPlanoAEEComTurmaPorIdQuery request, CancellationToken cancellationToken)
            => await repositorioPlanoAEE.ObterPlanoComTurmaPorId(request.PlanoAEEId);
    }
}
