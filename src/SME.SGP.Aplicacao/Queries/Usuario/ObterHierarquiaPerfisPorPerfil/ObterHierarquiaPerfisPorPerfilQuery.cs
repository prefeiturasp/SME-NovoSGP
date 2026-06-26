using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterHierarquiaPerfisPorPerfilQuery : IRequest<IEnumerable<PrioridadePerfil>>
    {
        public ObterHierarquiaPerfisPorPerfilQuery(Guid perfilUsuario)
        {
            PerfilUsuario = perfilUsuario;
        }

        public Guid PerfilUsuario { get; set; }
    }
}
