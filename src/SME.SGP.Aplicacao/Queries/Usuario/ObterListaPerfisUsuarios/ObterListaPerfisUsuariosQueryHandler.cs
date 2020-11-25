using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Usuario.ObterListaPerfisUsuarios
{
    public class ObterListaPerfisUsuariosQueryHandler : IRequestHandler<ObterListaPerfisUsuariosQuery, IEnumerable<PrioridadePerfil>>
    {
        private readonly IRepositorioPrioridadePerfil repositorioPrioridadePerfil;

        public ObterListaPerfisUsuariosQueryHandler(IRepositorioPrioridadePerfil repositorioPrioridadePerfil)
        {
            this.repositorioPrioridadePerfil = repositorioPrioridadePerfil ?? throw new System.ArgumentNullException(nameof(repositorioPrioridadePerfil));
        }

        public async Task<IEnumerable<PrioridadePerfil>> Handle(ObterListaPerfisUsuariosQuery request, CancellationToken cancellationToken)
        {
            var tipoPerfilUsuario = await repositorioPrioridadePerfil.ObterTipoPerfil(request.PerfilUsuario);

            return await repositorioPrioridadePerfil.ObterPerfisPorTipo(tipoPerfilUsuario);
        }
    }
}
