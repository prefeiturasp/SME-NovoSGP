using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterHierarquiaPerfisPorPerfilQueryHandler : IRequestHandler<ObterHierarquiaPerfisPorPerfilQuery, IEnumerable<PrioridadePerfil>>
    {
        private readonly IRepositorioPrioridadePerfil repositorioPrioridadePerfil;

        public ObterHierarquiaPerfisPorPerfilQueryHandler(IRepositorioPrioridadePerfil repositorioPrioridadePerfil)
        {
            this.repositorioPrioridadePerfil = repositorioPrioridadePerfil ?? throw new System.ArgumentNullException(nameof(repositorioPrioridadePerfil));
        }

        public async Task<IEnumerable<PrioridadePerfil>> Handle(ObterHierarquiaPerfisPorPerfilQuery request, CancellationToken cancellationToken)
        {
            return await repositorioPrioridadePerfil.ObterHierarquiaPerfisPorPerfil(request.PerfilUsuario);
        }
    }
}
