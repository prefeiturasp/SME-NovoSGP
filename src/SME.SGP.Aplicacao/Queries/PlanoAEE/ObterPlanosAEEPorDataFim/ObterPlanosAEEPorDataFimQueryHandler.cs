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
    public class ObterPlanosAEEPorDataFimQueryHandler : IRequestHandler<ObterPlanosAEEPorDataFimQuery, IEnumerable<PlanoAEE>>
    {
        private readonly IRepositorioPlanoAEE repositorioPlanoAEE;

        public ObterPlanosAEEPorDataFimQueryHandler(IRepositorioPlanoAEE repositorioPlanoAEE)
        {
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
        }

        public async Task<IEnumerable<PlanoAEE>> Handle(ObterPlanosAEEPorDataFimQuery request, CancellationToken cancellationToken)
            => await repositorioPlanoAEE.ObterPorDataFinalVigencia(request.DataFim, request.DesconsideraPendencias, request.DesconsideraNotificados, request.Tipo);
    }
}
