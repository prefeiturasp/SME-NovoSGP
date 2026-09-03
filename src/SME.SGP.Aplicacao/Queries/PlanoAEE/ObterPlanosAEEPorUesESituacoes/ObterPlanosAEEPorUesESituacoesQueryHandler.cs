using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPlanosAEEPorUesESituacoesQueryHandler : IRequestHandler<ObterPlanosAEEPorUesESituacoesQuery, IEnumerable<PlanoAEE>>
    {
        private readonly IRepositorioPlanoAEEConsulta repositorioPlanoAEE;

        public ObterPlanosAEEPorUesESituacoesQueryHandler(IRepositorioPlanoAEEConsulta repositorioPlanoAEE)
        {
            this.repositorioPlanoAEE = repositorioPlanoAEE;
        }

        public async Task<IEnumerable<PlanoAEE>> Handle(ObterPlanosAEEPorUesESituacoesQuery request, CancellationToken cancellationToken)
            => await repositorioPlanoAEE.ObterPlanosPorUesESituacoes(request.UesCodigos, request.Situacoes, request.ResponsavelPaaiRf);
    }
}
