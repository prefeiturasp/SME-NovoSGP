using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanosAEEAtivosQueryHandler : IRequestHandler<ObterPlanosAEEAtivosQuery, IEnumerable<PlanoAEE>>
    {
        private readonly IRepositorioPlanoAEEConsulta repositorioPlanoAEE;

        public ObterPlanosAEEAtivosQueryHandler(IRepositorioPlanoAEEConsulta repositorioPlanoAEE)
        {
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
        }

        public async Task<IEnumerable<PlanoAEE>> Handle(ObterPlanosAEEAtivosQuery request, CancellationToken cancellationToken)
            => await repositorioPlanoAEE.ObterPlanosAtivos();
    }
}
