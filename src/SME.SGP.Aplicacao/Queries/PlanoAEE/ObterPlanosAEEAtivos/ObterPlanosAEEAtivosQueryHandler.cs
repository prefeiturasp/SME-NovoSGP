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
    public class ObterPlanosAEEAtivosQueryHandler : IRequestHandler<ObterPlanosAEEAtivosQuery, IEnumerable<PlanoAEE>>
    {
        private readonly IRepositorioPlanoAEE repositorioPlanoAEE;

        public ObterPlanosAEEAtivosQueryHandler(IRepositorioPlanoAEE repositorioPlanoAEE)
        {
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
        }

        public async Task<IEnumerable<PlanoAEE>> Handle(ObterPlanosAEEAtivosQuery request, CancellationToken cancellationToken)
            => await repositorioPlanoAEE.ObterPlanosAtivos();
    }
}
